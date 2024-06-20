﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ItemzApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using ItemzApp.API.Entities;
using AutoMapper;
using ItemzApp.API.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ItemzApp.API.ResourceParameters;
using ItemzApp.API.Helper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    [Route("api/Itemzs")] // e.g. http://HOST:PORT/api/itemzs
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ItemzsController : ControllerBase
    {
        private readonly IItemzRepository _itemzRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILogger<ItemzsController> _logger;

        public ItemzsController(IItemzRepository itemzRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService,
            ILogger<ItemzsController> logger)
        {
            _itemzRepository = itemzRepository ?? throw new ArgumentNullException(nameof(itemzRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        /// <summary>
        /// Get an Itemz by ID (represented by a GUID)
        /// </summary>
        /// <param name="ItemzId">GUID representing an unique ID of the Itemz that you want to get</param>
        /// <returns>A single Itemz record based on provided ID (GUID) </returns>
        /// <response code="200">Returns the requested Itemz</response>
        /// <response code="404">Requested Itemz not found</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetItemzDTO))]
        [HttpGet("{ItemzId:Guid}",
            Name = "__Single_Itemz_By_GUID_ID__")] // e.g. http://HOST:PORT/api/Itemzs/9153a516-d69e-4364-b17e-03b87442e21c
        [HttpHead("{ItemzId:Guid}",Name ="__HEAD_Itemz_By_GUID_ID__")]
        public async Task<ActionResult<GetItemzDTO>> GetItemzAsync(Guid ItemzId)
        {

            _logger.LogDebug("{FormattedControllerAndActionNames}Processing request to get Itemz for ID {ItemzId}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    ItemzId);
            var itemzFromRepo = await _itemzRepository.GetItemzAsync(ItemzId);

            if (itemzFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz for ID {ItemzId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    ItemzId);
                return NotFound();
            }
            _logger.LogDebug("{FormattedControllerAndActionNames}Found Itemz for ID {ItemzId} and now returning results",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    ItemzId);
            return Ok(_mapper.Map<GetItemzDTO>(itemzFromRepo));
        }

        /// <summary>
        /// Gets collection of Itemzs
        /// </summary>
        /// <param name="itemzResourceParameter">Pass in information related to Pagination and Sorting Order via this parameter</param>
        /// <returns>Collection of Itemz based on expectated pagination and sorting order.</returns>
        /// <response code="200">Returns collection of Itemzs based on pagination</response>
        /// <response code="404">No Itemzs were found</response>
        [HttpGet(Name = "__GET_Itemzs_Collection__")]
        [HttpHead (Name ="__HEAD_Itemzs_Collection__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<GetItemzDTO>> GetItemzs(
            [FromQuery] ItemzResourceParameter itemzResourceParameter)
        {
            if(!_propertyMappingService.ValidMappingExistsFor<GetItemzDTO, Itemz>
                (itemzResourceParameter.OrderBy))
            {
                _logger.LogWarning("{FormattedControllerAndActionNames}Requested Order By Field {OrderByFieldName} is not found. Property Validation Failed!",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzResourceParameter.OrderBy);
                return BadRequest();
            }

            var itemzsFromRepo = _itemzRepository.GetItemzs(itemzResourceParameter);

            // EXPLANATION : Check if list is IsNullOrEmpty
            // By default we don't have option baked in the .NET to check
            // for null or empty for List type. In the following code we are first checking
            // for nullable itemzsFromRepo? and then for count great then zero via Any()
            // If any of above is true then we return true. This way we log that no itemz were
            // found in the database.
            // Ref: https://stackoverflow.com/a/54549818
            if (!itemzsFromRepo?.Any() ?? true)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}No Items found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return NotFound();
            }
            _logger.LogDebug("{FormattedControllerAndActionNames}In total {ItemzNumbers} Itemz found in the repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzsFromRepo!.TotalCount);
            var previousPageLink = itemzsFromRepo.HasPrevious ?
                CreateItemzResourceUri(itemzResourceParameter,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = itemzsFromRepo.HasNext ?
                CreateItemzResourceUri(itemzResourceParameter,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = itemzsFromRepo.TotalCount,
                pageSize = itemzsFromRepo.PageSize,
                currentPage = itemzsFromRepo.CurrentPage,
                totalPages = itemzsFromRepo.TotalPages,
                previousPageLink,
                nextPageLink
            };

            // EXPLANATION : it's possible to send customer headers in the response.
            // So, before we hit 'return Ok...' statement, we can build our
            // own response header as you can see in following example.

            // TODO: Check if just passsing the header is good enough. How can we
            // document it so that consumers can use it effectively. Also, 
            // how to implement versioning of headers so that we don't break
            // existing applications using the headers after performing upgrade
            // in the future.

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationMetadata));

            _logger.LogDebug("{FormattedControllerAndActionNames}Returning results for {ItemzNumbers} Itemzs to the caller",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzsFromRepo.TotalCount);
            return Ok(_mapper.Map<IEnumerable<GetItemzDTO>>(itemzsFromRepo));
        }

        /// <summary>
        /// Gets collection of Orphaned Itemzs
        /// </summary>
        /// <param name="itemzResourceParameter">Pass in information related to Pagination and Sorting Order via this parameter</param>
        /// <returns>Collection of orphaned Itemz based on expectated pagination and sorting order.</returns>
        /// <response code="200">Returns collection of orphaned Itemzs based on pagination</response>
        /// <response code="404">No Itemzs were found</response>
        [HttpGet("GetOrphan/", Name = "__GET_Orphan_Itemzs_Collection__")]
        [HttpHead("GetOrphan/", Name = "__HEAD_Orphan_Itemzs_Collection__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<GetItemzDTO>> GetOrphanItemzs(
            [FromQuery] ItemzResourceParameter itemzResourceParameter)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<GetItemzDTO, Itemz>
                (itemzResourceParameter.OrderBy))
            {
                _logger.LogWarning("{FormattedControllerAndActionNames}Requested Order By Field {OrderByFieldName} is not found. Property Validation Failed!",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzResourceParameter.OrderBy);
                return BadRequest();
            }

            var itemzsFromRepo = _itemzRepository.GetOrphanItemzs(itemzResourceParameter);
        
            if (!itemzsFromRepo?.Any() ?? true)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}No orphan Itemz found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return NotFound();
            }
            _logger.LogDebug("{FormattedControllerAndActionNames}In total {ItemzNumbers} orphan Itemz found in the repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzsFromRepo!.TotalCount);
            var previousPageLink = itemzsFromRepo.HasPrevious ?
                CreateItemzResourceUri(itemzResourceParameter,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = itemzsFromRepo.HasNext ?
                CreateItemzResourceUri(itemzResourceParameter,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = itemzsFromRepo.TotalCount,
                pageSize = itemzsFromRepo.PageSize,
                currentPage = itemzsFromRepo.CurrentPage,
                totalPages = itemzsFromRepo.TotalPages,
                previousPageLink,
                nextPageLink
            };

            // EXPLANATION : it's possible to send customer headers in the response.
            // So, before we hit 'return Ok...' statement, we can build our
            // own response header as you can see in following example.

            // TODO: Check if just passsing the header is good enough. How can we
            // document it so that consumers can use it effectively. Also, 
            // how to implement versioning of headers so that we don't break
            // existing applications using the headers after performing upgrade
            // in the future.

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(paginationMetadata));

            _logger.LogDebug("{FormattedControllerAndActionNames}Returning results for {ItemzNumbers} orphan Itemzs",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzsFromRepo.TotalCount);
            return Ok(_mapper.Map<IEnumerable<GetItemzDTO>>(itemzsFromRepo));
        }

        /// <summary>
        /// Gets count of Orphaned Itemzs in the repository
        /// </summary>
        /// <returns>Number of Orphaned itemzs found in the repository</returns>
        /// <response code="200">Returns collection of orphaned Itemzs based on pagination</response>
        [HttpGet("GetOrphanCount/", Name = "__GET_Orphan_Itemzs_Count__")]
        [HttpHead("GetOrphanCount/", Name = "__HEAD_Orphan_Itemzs_Count__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetOrphanItemzCount()
        {
            var foundNumberOfOrphanItemz = await _itemzRepository.GetOrphanItemzsCount();
            if (foundNumberOfOrphanItemz > 0)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Found {foundNumberOfOrphanItemz} records of orphan Itemz",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    foundNumberOfOrphanItemz);
            }
            else
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}No orphan Itemz records found.",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext));
            }
            return Ok(foundNumberOfOrphanItemz);
        }

        /// <summary>
        /// Used for creating new Itemz record in the database
        /// </summary>
        /// <param name="createItemzDTO">Used for populating information in the newly created itemz in the database</param>
        /// <param name="parentItemzId">Used as parent for adding new Itemz as children</param>
        /// <param name="AtBottomOfChildNodes">Indicates if we should add new Itemz at TOP or BOTTOM of child Itemz nodes list</param>
        /// <returns>Newly created Itemz property details</returns>
        /// <response code="201">Returns newly created itemzs property details</response>
        /// <response code="404">Expected Parent Itemz not found</response>
        [HttpPost(Name = "__POST_Create_Itemz__")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetItemzDTO>> CreateItemzAsync(
                    [FromBody] CreateItemzDTO createItemzDTO
                    , [FromQuery] Guid parentItemzId
                    , [FromQuery] bool AtBottomOfChildNodes = true)
        {
            Itemz itemzEntity;
            try
            {
                itemzEntity = _mapper.Map<Entities.Itemz>(createItemzDTO);
            }
            catch (AutoMapper.AutoMapperMappingException amm_ex)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Could not create new Itemz due to issue with value provided for {fieldname}",
                        ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                        amm_ex.MemberMap.DestinationName);
                return ValidationProblem();
            }
            _itemzRepository.AddItemz(itemzEntity);

            if (!(parentItemzId.Equals(Guid.Empty)))
            {
                var itemzFromRepo = await _itemzRepository.GetItemzAsync(parentItemzId);

                if (itemzFromRepo == null)
                {
                    _logger.LogDebug("{FormattedControllerAndActionNames}Expected parent Itemz with ID {parentItemzId} could not be found",
                        ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                        parentItemzId);
                    return NotFound();
                }

                //await _itemzRepository.AddNewItemzHierarchyAsync(parentItemzId, itemzEntity.Id, atBottomOfChildNodes: AtBottomOfChildNodes);
                await _itemzRepository.MoveItemzHierarchyAsync(itemzEntity.Id, parentItemzId, atBottomOfChildNodes: AtBottomOfChildNodes);
            }
            await _itemzRepository.SaveAsync();

            _logger.LogDebug("{FormattedControllerAndActionNames}Created new Itemz with ID {ItemzId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                itemzEntity.Id);
            return CreatedAtRoute("__Single_Itemz_By_GUID_ID__", new { ItemzId = itemzEntity.Id }, 
                _mapper.Map<GetItemzDTO>(itemzEntity) // Converting to DTO as this is going out to the consumer
                );
        }

        /// <summary>
        /// Used for creating new Itemz record in the database
        /// </summary>
        /// <param name="createItemzDTO">Used for populating information in the newly created itemz in the database</param>
        /// <param name="firstItemzId">Used as first Itemz for adding new Itemz between existing two Itemz</param>
        /// <param name="secondItemzId">Used as second Itemz for adding new Itemz between existing two Itemz</param>
        /// <returns>Newly created Itemz property details</returns>
        /// <response code="201">Returns newly created itemzs property details</response>
        /// <response code="404">Expected first or second from between Itemz not found</response>
        [HttpPost("CreateItemzBetweenExistingItemz/", Name = "__POST_Create_Itemz_Between_Existing_Itemz__")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetItemzDTO>> CreateItemzBetweenExistingItemzAsync([FromBody] CreateItemzDTO createItemzDTO, [FromQuery] Guid firstItemzId, [FromQuery] Guid secondItemzId)
        {
            if (firstItemzId == Guid.Empty)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}First ItemzID from between two Itemz is an empty ID.",
                        ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext));
                return NotFound();
            }
            if (secondItemzId == Guid.Empty)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Second ItemzID from between two Itemz is an empty ID.",
                        ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext));
                return NotFound();
            }

            Itemz itemzEntity;

            try
            {
                itemzEntity = _mapper.Map<Entities.Itemz>(createItemzDTO);
            }
            catch (AutoMapper.AutoMapperMappingException amm_ex)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Could not create new Itemz due to issue with value provided for {fieldname}",
                        ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                        amm_ex.MemberMap.DestinationName);
                return ValidationProblem();
            }
            _itemzRepository.AddItemz(itemzEntity);

            var firstItemz = await _itemzRepository.GetItemzAsync(firstItemzId);

            if (firstItemz == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Expected first Itemz with ID {firstItemzId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    firstItemzId);
                return NotFound();
            }

            var secondItemz = await _itemzRepository.GetItemzAsync(secondItemzId);

            if (secondItemz == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Expected second Itemz with ID {secondItemzId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    secondItemzId);
                return NotFound();
            }

            try { 
            await _itemzRepository.AddNewItemzBetweenTwoHierarchyRecordsAsync(firstItemzId, secondItemzId, itemzEntity.Id);
            await _itemzRepository.SaveAsync();
            }
            catch (ApplicationException appException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Exception Occured while trying to add Itemz between two existing Itemz :" + appException.Message,
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                var tempMessage = $"Could not create new Itemz between Itemz '{firstItemzId}' " +
                    $"and '{secondItemzId}'. " +
                    $":: InnerException :: {appException.Message} ";
                return BadRequest(tempMessage);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}DBUpdateException Occured while trying to add Itemz between two existing Itemz :" + dbUpdateException.Message,
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return Conflict( $"DBUpdateException : Could not create new Itemz between Itemz " +
                    $"'{firstItemzId}' and '{secondItemzId}'. DB Error reported, check the log file. " +
                    $":: InnerException :: '{dbUpdateException.Message}' ");
            }
            catch(Microsoft.SqlServer.Types.HierarchyIdException hierarchyIDException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}HierarchyIdException Occured while trying to add Itemz between two existing Itemz :" + hierarchyIDException.Message,
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return Conflict($"HierarchyIdException : Could not create new Itemz between Itemz " +
                    $"'{firstItemzId}' and '{secondItemzId}'. DB Error reported, check the log file. " +
                    $":: InnerException :: '{hierarchyIDException.Message}' ");
            }

            _logger.LogDebug("{FormattedControllerAndActionNames}Created new Itemz with ID {ItemzId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                itemzEntity.Id);
            return CreatedAtRoute("__Single_Itemz_By_GUID_ID__", new { ItemzId = itemzEntity.Id },
                _mapper.Map<GetItemzDTO>(itemzEntity) // Converting to DTO as this is going out to the consumer
                );
        }

        /// <summary>
        /// Updating exsting Itemz based on Itemz Id (GUID)
        /// </summary>
        /// <param name="itemzId">GUID representing an unique ID of the Itemz that you want to get</param>
        /// <param name="itemzToBeUpdated">required Itemz properties to be updated</param>
        /// <returns>No contents are returned but only Status 204 indicating that Item was updated successfully </returns>
        /// <response code="204">No content are returned but status of 204 indicated that item was successfully updated</response>
        /// <response code="404">Itemz based on itemzId was not found</response>

        [HttpPut("{itemzId}", Name = "__PUT_Update_Itemz_By_GUID_ID__ ")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateItemzPutAsync(Guid itemzId, UpdateItemzDTO itemzToBeUpdated)
        {
            if (!(await _itemzRepository.ItemzExistsAsync(itemzId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Itemz for ID {ItemzId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
                return NotFound();
            }

            var itemzFromRepo = await _itemzRepository.GetItemzForUpdatingAsync(itemzId);

            if (itemzFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Itemz for ID {ItemzId} could not be found in the Repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
                return NotFound();
            }
            try
            {
                _mapper.Map(itemzToBeUpdated, itemzFromRepo);
            }
            catch (AutoMapper.AutoMapperMappingException amm_ex)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Could not update Itemz for ID {ItemzId} due to issue with value provided for {fieldname}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId, amm_ex.MemberMap.DestinationName);
                return ValidationProblem();
            }

            _itemzRepository.UpdateItemz(itemzFromRepo);
            await _itemzRepository.SaveAsync();

            _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Itemz for ID {ItemzId} processed successfully",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
            return NoContent(); // This indicates that update was successfully saved in the DB.

        }


        /// <summary>
        /// Partially updating a single **Itemz**
        /// </summary>
        /// <param name="itemzId">Id of the Itemz representated by a GUID.</param>
        /// <param name="itemzPatchDocument">The set of operations to apply to the Itemz via JsonPatchDocument</param>
        /// <returns>an ActionResult of type Itemz</returns>
        /// <response code="204">No content are returned but status of 204 indicated that itemz was successfully updated</response>
        /// <response code="404">Itemz based on itemzId was not found</response>
        /// <response code="422">Validation problems occured during analyzing validation rules for the JsonPatchDocument </response>
        /// <remarks> Sample request (this request updates an **Itemz's name**)   
        /// Documentation regarding JSON Patch can be found at 
        /// *[ASP.NET Core - JSON Patch Operations](https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.1#operations)* 
        /// 
        ///     PATCH /api/Itemzs/{id}  
        ///     [  
        ///	        {   
        ///             "op": "replace",   
        ///             "path": "/name",   
        ///             "value": "PATCH Updated Name field"  
        ///	        }   
        ///     ]
        /// </remarks>

        [HttpPatch("{itemzId}",Name = "__PATCH_Update_Itemz_By_GUID_ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateItemzPatchAsync(Guid itemzId, JsonPatchDocument<UpdateItemzDTO> itemzPatchDocument)
        {
            if (!(await _itemzRepository.ItemzExistsAsync(itemzId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Itemz for ID {ItemzId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
                return NotFound();
            }

            var itemzFromRepo = await _itemzRepository.GetItemzForUpdatingAsync(itemzId);

            if (itemzFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Itemz for ID {ItemzId} could not be found in the Repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
                return NotFound();
            }

            var itemzToPatch = _mapper.Map<UpdateItemzDTO>(itemzFromRepo);

            itemzPatchDocument.ApplyTo(itemzToPatch, ModelState);

            // Validating Itemz patch document and verifying that it meets all the 
            // validation rules as expected. This will check if the data passed in the Patch Document
            // is ready to be saved in the db.

            if (!TryValidateModel(itemzToPatch))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz Properties did not pass defined Validation Rules for ID {ItemzId}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
                return ValidationProblem(ModelState);
            }

            try
            {
                _mapper.Map(itemzToPatch, itemzFromRepo);
            }
            catch (AutoMapper.AutoMapperMappingException amm_ex)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Could not update Itemz for ID {ItemzId} due to issue with value provided for {fieldname}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId, amm_ex.MemberMap.DestinationName);
                return ValidationProblem();
            }

            _itemzRepository.UpdateItemz(itemzFromRepo);
            await _itemzRepository.SaveAsync();

            _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Itemz for ID {ItemzId} processed successfully",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
            return NoContent();
        }

        // We have configured in startup class our own custom implementation of 
        // problem Details. Now we are overriding ValidationProblem method that is defined in ControllerBase
        // class to make sure that we use that custom problem details builder. 
        // Instead of passing 400 it will pass back 422 code with more details.
        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }









        /// <summary>
        /// Move Itemz to a new location in the repository including all its sub-Itemz
        /// </summary>
        /// <param name="MovingItemzId">GUID representing an unique ID of the moving Itemz</param>
        /// <param name="TargetId">Details about target ID under which Itemz will be moving</param>
        /// <param name="AtBottomOfChildNodes">Boolean value where by true means at the bottom of the existing nodes and false means at the top</param>
        /// <returns>No contents are returned when Itemz gets moved to its new desired location</returns>
        /// <response code="204">No content are returned but status of 204 indicating that Itemz has successfully moved to its desired location</response>
        /// <response code="404">Either Itemz or ItemzType was not found</response>
        [HttpPost("{MovingItemzId}", Name = "__POST_Move_Itemz__")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> MoveItemzAsync([FromRoute] Guid MovingItemzId, [FromQuery] Guid TargetId, [FromQuery] bool AtBottomOfChildNodes = true)
        {
            if (!(await _itemzRepository.ItemzExistsAsync(MovingItemzId)))// Check if Itemz exists

            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz for ID {MovingItemzId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    MovingItemzId);
                return NotFound();
            }

            // TODO :: WE HAVE TO ALSO CHECK IN THE HIERARCHY TABLE IF TARGET EXISTITS OTHERWISE THERE IS NO
            // POINT IN MOVING ITEMZ UNDER AN ORPHANED ITEMZ.

            if (!((await _itemzRepository.ItemzTypeExistsAsync(TargetId)) || (await _itemzRepository.ItemzExistsAsync(TargetId))))  // Check if Target ItemzType or Itemz Exists
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Target ID {TargetId} could not be found for either 'ItemzType' or 'Itemz'",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    TargetId);
                return NotFound();
            }

            await _itemzRepository.MoveItemzHierarchyAsync(MovingItemzId, TargetId, atBottomOfChildNodes: AtBottomOfChildNodes);
            await _itemzRepository.SaveAsync();

            _logger.LogDebug("{FormattedControllerAndActionNames}Itemz ID {MovingItemzId} successfully moved under Target ID {TargetId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                MovingItemzId,
                TargetId);
            return NoContent(); // This indicates that update was successfully saved in the DB.
        }









        /// <summary>
        /// Deleting a specific Itemz
        /// </summary>
        /// <param name="itemzId">GUID representing an unique ID of the Itemz that you want to get</param>
        /// <returns>Status code 204 is returned without any content indicating that deletion of the specified Itemz was successful</returns>
        /// <response code="404">Itemz based on itemzId was not found</response>
        [HttpDelete("{itemzId}",Name ="__DELETE_Itemz_By_GUID_ID__")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteItemzAsync(Guid itemzId)
        {
            if (!(await _itemzRepository.ItemzExistsAsync(itemzId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Cannot Delete Itemz with ID {ItemzId} as it could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
                return NotFound();
            }

            //var itemzFromRepo = await _itemzRepository.ItemzExistsAsync(itemzId);

            //if (itemzFromRepo == false)
            //{
            //    _logger.LogDebug("{FormattedControllerAndActionNames}Cannot Delete Itemz with ID {ItemzId} as it could not be found in the Repository",
            //        ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
            //        itemzId);
            //    return NotFound();
            //}

            await _itemzRepository.DeleteItemzAsync(itemzId);
            // await _itemzRepository.SaveAsync();

            _logger.LogDebug("{FormattedControllerAndActionNames}Delete request for Itemz with ID {ItemzId} processed successfully",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
            return NoContent();
        }

        /// <summary>
        /// Get list of supported HTTP Options for the Itemz controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions (Name ="__OPTIONS_for_Itemz_Controller__")]
        public IActionResult GetItemzOptions()
        {
            Response.Headers.Add("Allow","GET,HEAD,OPTIONS,POST,PUT,PATCH,DELETE");
            return Ok();
        }

        private string CreateItemzResourceUri(
            ItemzResourceParameter itemzResourceParameter,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("__GET_Itemzs__",
                        new
                        {
                            orderBy = itemzResourceParameter.OrderBy,
                            pageNumber = itemzResourceParameter.PageNumber - 1,
                            pageSize = itemzResourceParameter.PageSize
                        })!;
                case ResourceUriType.NextPage:
                    return Url.Link("__GET_Itemzs__",
                        new
                        {
                            orderBy = itemzResourceParameter.OrderBy,
                            pageNumber = itemzResourceParameter.PageNumber + 1,
                            pageSize = itemzResourceParameter.PageSize
                        })!;
                default:
                    return Url.Link("__GET_Itemzs__",
                        new
                        {
                            orderBy = itemzResourceParameter.OrderBy,
                            pageNumber = itemzResourceParameter.PageNumber,
                            pageSize = itemzResourceParameter.PageSize
                        })!;
            }
        }
    }
}
