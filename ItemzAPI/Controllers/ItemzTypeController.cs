﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ItemzApp.API.Models;
using ItemzApp.API.ResourceParameters;
using ItemzApp.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    //[Route("api/ItemzType")]
    [Route("api/[controller]")] // e.g. http://HOST:PORT/api/itemzs/ItemzTypes
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ItemzTypesController : ControllerBase
    {
        private readonly IItemzTypeRepository _ItemzTypeRepository;
        private readonly IMapper _mapper;
        // private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILogger<ItemzTypesController> _logger;
        public ItemzTypesController(IItemzTypeRepository ItemzTypeRepository,
                                 IMapper mapper,
                                 //IPropertyMappingService propertyMappingService,
                                 ILogger<ItemzTypesController> logger)
        {
            _ItemzTypeRepository = ItemzTypeRepository ?? throw new ArgumentNullException(nameof(ItemzTypeRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            //_propertyMappingService = propertyMappingService ??
            //    throw new ArgumentNullException(nameof(propertyMappingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        /// <summary>
        /// Get a ItemzType by ID (represented by a GUID)
        /// </summary>
        /// <param name="ItemzTypeId">GUID representing an unique ID of the ItemzType that you want to get</param>
        /// <returns>A single ItemzType record based on provided ID (GUID) </returns>
        /// <response code="200">Returns the requested ItemzType</response>
        /// <response code="404">Requested ItemzType not found</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetItemzTypeDTO))]
        [HttpGet("{ItemzTypeId:Guid}",
            Name = "__Single_ItemzType_By_GUID_ID__")] // e.g. http://HOST:PORT/api/ItemzTypes/42f62a6c-fcda-4dac-a06c-406ac1c17770
        [HttpHead("{ItemzTypeId:Guid}", Name = "__HEAD_ItemzType_By_GUID_ID__")]
        public ActionResult<GetItemzTypeDTO> GetItemzType(Guid ItemzTypeId)
        {
            _logger.LogDebug("Processing request to get ItemzType for ID {ItemzTypeId}", ItemzTypeId);
            var ItemzTypeFromRepo = _ItemzTypeRepository.GetItemzType(ItemzTypeId);

            if (ItemzTypeFromRepo == null)
            {
                _logger.LogDebug("ItemzType for ID {ItemzTypeId} could not be found", ItemzTypeId);
                return NotFound();
            }
            _logger.LogDebug("Found ItemzType for ID {ItemzTypeId} and now returning results", ItemzTypeId);
            return Ok(_mapper.Map<GetItemzTypeDTO>(ItemzTypeFromRepo));
        }



        /// <summary>
        /// Gets collection of ItemzTypes
        /// </summary>
        /// <returns>Collection of ItemzTypes based on expectated sorting order.</returns>
        /// <response code="200">Returns collection of ItemzTypes based on sorting order</response>
        /// <response code="404">No ItemzTypes were found</response>
        [HttpGet(Name = "__GET_ItemzTypes__")]
        [HttpHead(Name = "__HEAD_ItemzTypes_Collection__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<GetItemzTypeDTO>> GetItemzTypes(
            //[FromQuery] ItemzResourceParameter itemzResourceParameter
            )
        {
            //if (!_propertyMappingService.ValidMappingExistsFor<GetItemzDTO, Itemz>
            //    (itemzResourceParameter.OrderBy))
            //{
            //    _logger.LogWarning("Requested Order By Field {OrderByFieldName} is not found. Property Validation Failed!", itemzResourceParameter.OrderBy);
            //    return BadRequest();
            //}

            var ItemzTypesFromRepo = _ItemzTypeRepository.GetItemzTypes();
            if (ItemzTypesFromRepo == null)
            {
                _logger.LogDebug("No ItemzTypes found");
                return NotFound();
            }
            // _logger.LogDebug("In total {ProjecftsNumbers} Itemz found in the repository", ItemzTypesFromRepo.Count());
            //var previousPageLink = ItemzTypesFromRepo.HasPrevious ?
            //    CreateItemzResourceUri(itemzResourceParameter,
            //    ResourceUriType.PreviousPage) : null;

            //var nextPageLink = ItemzTypesFromRepo.HasNext ?
            //    CreateItemzResourceUri(itemzResourceParameter,
            //    ResourceUriType.NextPage) : null;

            //var paginationMetadata = new
            //{
            //    totalCount = ItemzTypesFromRepo.TotalCount,
            //    pageSize = ItemzTypesFromRepo.PageSize,
            //    currentPage = ItemzTypesFromRepo.CurrentPage,
            //    totalPages = ItemzTypesFromRepo.TotalPages,
            //    previousPageLink,
            //    nextPageLink
            //};

            // EXPLANATION : it's possible to send customer headers in the response.
            // So, before we hit 'return Ok...' statement, we can build our
            // own response header as you can see in following example.

            // TODO: Check if just passsing the header is good enough. How can we
            // document it so that consumers can use it effectively. Also, 
            // how to implement versioning of headers so that we don't break
            // existing applications using the headers after performing upgrade
            // in the future.

            //Response.Headers.Add("X-Pagination",
            //    JsonConvert.SerializeObject(paginationMetadata));

            _logger.LogDebug("Returning results for {ItemzTypeNumbers} ItemzTypes to the caller", ItemzTypesFromRepo.Count());
            return Ok(_mapper.Map<IEnumerable<GetItemzTypeDTO>>(ItemzTypesFromRepo));
        }



        /// <summary>
        /// Used for creating new ItemzType record in the database
        /// </summary>
        /// <param name="createItemzTypeDTO">Used for populating information in the newly created ItemzType in the database</param>
        /// <returns>Newly created ItemzType property details</returns>
        /// <response code="201">Returns newly created ItemzTypes property details</response>

        [HttpPost(Name = "__POST_Create_ItemzType__")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public ActionResult<GetItemzTypeDTO> CreateItemzType(CreateItemzTypeDTO createItemzTypeDTO)
        {
            var ItemzTypeEntity = _mapper.Map<Entities.ItemzType>(createItemzTypeDTO);
            _ItemzTypeRepository.AddItemzType(ItemzTypeEntity);
            _ItemzTypeRepository.Save();

            _logger.LogDebug("Created new ItemzType with ID {ItemzTypeId}", ItemzTypeEntity.Id);
            return CreatedAtRoute("__Single_ItemzType_By_GUID_ID__", new { ItemzTypeId = ItemzTypeEntity.Id },
                _mapper.Map<GetItemzTypeDTO>(ItemzTypeEntity) // Converting to DTO as this is going out to the consumer
                );


        }


        /// <summary>
        /// Updating exsting ItemzType based on ItemzType Id (GUID)
        /// </summary>
        /// <param name="ItemzTypeId">GUID representing an unique ID of the ItemzType that you want to get</param>
        /// <param name="ItemzTypeToBeUpdated">required ItemzType properties to be updated</param>
        /// <returns>No contents are returned but only Status 204 indicating that ItemzType was updated successfully </returns>
        /// <response code="204">No content are returned but status of 204 indicated that ItemzType was successfully updated</response>
        /// <response code="404">ItemzType based on ItemzTypeId was not found</response>

        [HttpPut("{ItemzTypeId}", Name = "__PUT_Update_ItemzType_By_GUID_ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult UpdateItemzTypePut(Guid ItemzTypeId, UpdateItemzTypeDTO ItemzTypeToBeUpdated)
        {
            if (!_ItemzTypeRepository.ItemzTypeExists(ItemzTypeId))
            {
                _logger.LogDebug("HttpPut - Update request for ItemzType for ID {ItemzTypeId} could not be found", ItemzTypeId);
                return NotFound();
            }

            var ItemzTypeFromRepo = _ItemzTypeRepository.GetItemzTypeForUpdate(ItemzTypeId);

            if (ItemzTypeFromRepo == null)
            {
                _logger.LogDebug("HttpPut - Update request for ItemzType for ID {ItemzTypeId} could not be found in the Repository", ItemzTypeId);
                return NotFound();
            }

            _mapper.Map(ItemzTypeToBeUpdated, ItemzTypeFromRepo);
            _ItemzTypeRepository.UpdateItemzType(ItemzTypeFromRepo);
            _ItemzTypeRepository.Save();

            _logger.LogDebug("HttpPut - Update request for ItemzType for ID {ItemzTypeId} processed successfully", ItemzTypeId);
            return NoContent(); // This indicates that update was successfully saved in the DB.

        }


        /// <summary>
        /// Partially updating a single **ItemzType**
        /// </summary>
        /// <param name="ItemzTypeId">Id of the ItemzType representated by a GUID.</param>
        /// <param name="ItemzTypePatchDocument">The set of operations to apply to the ItemzType via JsonPatchDocument</param>
        /// <returns>an ActionResult of type ItemzType</returns>
        /// <response code="204">No content are returned but status of 204 indicated that ItemzType was successfully updated</response>
        /// <response code="404">ItemzType based on ItemzTypeId was not found</response>
        /// <response code="422">Validation problems occured during analyzing validation rules for the JsonPatchDocument </response>
        /// <remarks> Sample request (this request updates an **ItemzType's name**)   
        /// Documentation regarding JSON Patch can be found at 
        /// *[ASP.NET Core - JSON Patch Operations](https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.1#operations)* 
        /// 
        ///     PATCH /api/ItemzTypes/{id}  
        ///     [  
        ///	        {   
        ///             "op": "replace",   
        ///             "path": "/name",   
        ///             "value": "PATCH Updated Name field"  
        ///	        }   
        ///     ]
        /// </remarks>

        [HttpPatch("{ItemzTypeId}", Name = "__PATCH_Update_ItemzType_By_GUID_ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult UpdateItemzTypePatch(Guid ItemzTypeId, JsonPatchDocument<UpdateItemzTypeDTO> ItemzTypePatchDocument)
        {
            if (!_ItemzTypeRepository.ItemzTypeExists(ItemzTypeId))
            {
                _logger.LogDebug("HttpPatch - Update request for ItemzType for ID {ItemzTypeId} could not be found", ItemzTypeId);
                return NotFound();
            }

            var ItemzTypeFromRepo = _ItemzTypeRepository.GetItemzTypeForUpdate(ItemzTypeId);

            if (ItemzTypeFromRepo == null)
            {
                _logger.LogDebug("HttpPatch - Update request for ItemzType for ID {ItemzTypeId} could not be found in the Repository", ItemzTypeId);
                return NotFound();
            }

            var ItemzTypeToPatch = _mapper.Map<UpdateItemzTypeDTO>(ItemzTypeFromRepo);

            ItemzTypePatchDocument.ApplyTo(ItemzTypeToPatch, ModelState);

            // Validating ItemzType patch document and verifying that it meets all the 
            // validation rules as expected. This will check if the data passed in the Patch Document
            // is ready to be saved in the db.

            if (!TryValidateModel(ItemzTypeToPatch))
            {
                _logger.LogDebug("HttpPatch - ItemzType Properties did not pass defined Validation Rules for ID {ItemzTypeId}", ItemzTypeId);
                return ValidationProblem(ModelState);
            }

            _mapper.Map(ItemzTypeToPatch, ItemzTypeFromRepo);
            _ItemzTypeRepository.UpdateItemzType(ItemzTypeFromRepo);
            _ItemzTypeRepository.Save();

            _logger.LogDebug("HttpPatch - Update request for ItemzType for ID {ItemzTypeId} processed successfully", ItemzTypeId);
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
        /// Deleting a specific ItemzType
        /// </summary>
        /// <param name="ItemzTypeId">GUID representing an unique ID of the ItemzType that you want to get</param>
        /// <returns>Status code 204 is returned without any content indicating that deletion of the specified ItemzType was successful</returns>
        /// <response code="404">ItemzType based on ItemzTypeId was not found</response>
        [HttpDelete("{ItemzTypeId}", Name = "__DELETE_ItemzType_By_GUID_ID__")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult DeleteItemzType(Guid ItemzTypeId)
        {
            if (!_ItemzTypeRepository.ItemzTypeExists(ItemzTypeId))
            {
                _logger.LogDebug("Cannot Delete ItemzType with ID {ItemzTypeId} as it could not be found", ItemzTypeId);
                return NotFound();
            }

            var ItemzTypeFromRepo = _ItemzTypeRepository.GetItemzTypeForUpdate(ItemzTypeId);

            if (ItemzTypeFromRepo == null)
            {
                _logger.LogDebug("Cannot Delete ItemzType with ID {ItemzTypeId} as it could not be found in the Repository", ItemzTypeId);
                return NotFound();
            }

            _ItemzTypeRepository.DeleteItemzType(ItemzTypeFromRepo);
            _ItemzTypeRepository.Save();

            _logger.LogDebug("Delete request for Projeect with ID {ItemzTypeId} processed successfully", ItemzTypeId);
            return NoContent();
        }


        /// <summary>
        /// Get list of supported HTTP Options for the ItemzTypes controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions(Name = "__OPTIONS_for_ItemzTypes_Controller__")]
        public IActionResult GetItemzTypesOptions()
        {
            Response.Headers.Add("Allow","GET,HEAD,OPTIONS,POST,PUT,PATCH,DELETE");
            return Ok();
        }



    }
}