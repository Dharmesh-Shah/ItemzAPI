// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ItemzApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using ItemzApp.API.Entities;
using AutoMapper;
using ItemzApp.API.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ItemzApp.API.ResourceParameters;
using ItemzApp.API.Helper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    [Route("api/ItemzTypeItemzs")] // e.g. http://HOST:PORT/api/ItemzTypeItemzs
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ItemzTypeItemzsController : ControllerBase
    {
        private readonly IItemzRepository _itemzRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILogger<ItemzTypeItemzsController> _logger;

        public ItemzTypeItemzsController(IItemzRepository itemzRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService,
            ILogger<ItemzTypeItemzsController> logger)
        {
            _itemzRepository = itemzRepository ?? throw new ArgumentNullException(nameof(itemzRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        /// <summary>
        /// Gets collection of Itemzs by ItemzType ID
        /// </summary>
        /// <param name="ItemzTypeId">ItemzType ID for which Itemz are queried</param>
        /// <param name="itemzResourceParameter">Pass in information related to Pagination and Sorting Order via this parameter</param>
        /// <returns>Collection of Itemz based on expectated pagination and sorting order.</returns>
        /// <response code="200">Returns collection of Itemzs based on pagination</response>
        /// <response code="404">No Itemzs were found</response>
        [HttpGet("{ItemzTypeId:Guid}", Name = "__GET_Itemzs_By_ItemzType__")]
        [HttpHead("{ItemzTypeId:Guid}", Name = "__HEAD_Itemzs_By_ItemzType__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<GetItemzDTO>> GetItemzsByItemzType(Guid ItemzTypeId,
            [FromQuery] ItemzResourceParameter itemzResourceParameter)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<GetItemzDTO, Itemz>
                (itemzResourceParameter.OrderBy))
            {
                _logger.LogWarning("Requested Order By Field {OrderByFieldName} is not found. Property Validation Failed!", itemzResourceParameter.OrderBy);
                return BadRequest();
            }

            if(!_itemzRepository.ItemzTypeExists(ItemzTypeId))
            {
                _logger.LogDebug("ItemzType with ID {ItemzTypeID} was not found in the repository", ItemzTypeId);
                return NotFound();
            }

            var itemzsFromRepo = _itemzRepository.GetItemzsByItemzType(ItemzTypeId, itemzResourceParameter);
            // EXPLANATION : Check if list is IsNullOrEmpty
            // By default we don't have option baked in the .NET to check
            // for null or empty for List type. In the following code we are first checking
            // for nullable itemzsFromRepo? and then for count great then zero via Any()
            // If any of above is true then we return true. This way we log that no itemz were
            // found in the database.
            // Ref: https://stackoverflow.com/a/54549818
            if (!itemzsFromRepo?.Any() ?? true)
            {
                _logger.LogDebug("No Items found in ItemzType with ID {ItemzTypeID}", ItemzTypeId);
                // TODO: If no itemz are found in a ItemzType then shall we return an error back to the calling client?
                return NotFound();
            }
            _logger.LogDebug("In total {ItemzNumbers} Itemz found in ItemzType with ID {ItemzTypeId}", itemzsFromRepo.TotalCount, ItemzTypeId);
            var previousPageLink = itemzsFromRepo.HasPrevious ?
                CreateItemzTypeItemzResourceUri(itemzResourceParameter,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = itemzsFromRepo.HasNext ?
                CreateItemzTypeItemzResourceUri(itemzResourceParameter,
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

            _logger.LogDebug("Returning results for {ItemzNumbers} Itemzs to the caller", itemzsFromRepo.TotalCount);
            return Ok(_mapper.Map<IEnumerable<GetItemzDTO>>(itemzsFromRepo));
        }

        /// <summary>
        /// Check if specific ItemzType and Itemz association exists
        /// </summary>
        /// <param name="ItemzTypeId">Provide ItemzType Id</param>
        /// <param name="itemzId">Provide Itemz Id</param>
        /// <returns>GetItemzDTO for the Itemz that has specified ItemzType association</returns>
        /// <response code="200">Returns GetItemzDTO for the Itemz that has specified ItemzType association</response>
        /// <response code="404">No ItemzType and Itemzs association was found</response>
        [HttpGet("CheckExists/", Name = "__GET_Check_ItemzType_Itemz_Association_Exists__")]
        [HttpHead("CheckExists/", Name = "__HEAD_Check_ItemzType_Itemz_Association_Exists__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]

        public ActionResult<GetItemzDTO> CheckItemzTypeItemzAssociationExists([FromQuery] Guid ItemzTypeId, Guid itemzId) // TODO: Try from Query.
        {
            var tempItemzTypeItemzDTO = new ItemzTypeItemzDTO();

            tempItemzTypeItemzDTO.ItemzTypeId = ItemzTypeId;
            tempItemzTypeItemzDTO.ItemzId = itemzId;
            if (!_itemzRepository.ItemzTypeItemzExists(tempItemzTypeItemzDTO))  // Check if ItemzTypeItemz association exists or not
            {
                _logger.LogDebug("HttpGet - ItemzType ID {ItemzTypeId} and Itemz ID {ItemzId} association could not be found",
                    tempItemzTypeItemzDTO.ItemzTypeId,
                    tempItemzTypeItemzDTO.ItemzId);
                return NotFound();
            }
            _logger.LogDebug("HttpGet - ItemzType ID {ItemzTypeId} and Itemz ID {ItemzId} association was found",
                            tempItemzTypeItemzDTO.ItemzTypeId,
                            tempItemzTypeItemzDTO.ItemzId);
            return RedirectToRoute("__Single_Itemz_By_GUID_ID__", new { Controller = "Itemzs", ItemzId = tempItemzTypeItemzDTO.ItemzId });

        }

        /// <summary>
        /// Used for creating new Itemz record in the database by ItemzType ID
        /// </summary>
        /// <param name="ItemzTypeId">ItemzType ID in Guid Form. New Itemzs will be associated with provided ItemzType Id</param>
        /// <param name="itemzCollection">Used for populating information in the newly created itemz in the database by ItemzType ID</param>
        /// <returns>Newly created Itemzs property details</returns>
        /// <response code="201">Returns newly created itemzs property details</response>
        [HttpPost("{ItemzTypeId:Guid}", Name = "__POST_Create_Itemz_Collecction_By_ItemzType__")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<GetItemzDTO>> CreateItemzCollectionByItemzType(
             Guid ItemzTypeId,
            IEnumerable<CreateItemzDTO> itemzCollection)
        {
            if (!_itemzRepository.ItemzTypeExists(ItemzTypeId))
            {
                _logger.LogDebug("ItemzType with ID {ItemzTypeID} was not found in the repository", ItemzTypeId);
                return NotFound();
            }

            var itemzEntities = _mapper.Map<IEnumerable<Entities.Itemz>>(itemzCollection);
            foreach (var itemz in itemzEntities)
            {
                _itemzRepository.AddItemzByItemzType(itemz, ItemzTypeId);
            }
            _itemzRepository.Save();

            var itemzCollectionToReturn = _mapper.Map<IEnumerable<GetItemzDTO>>(itemzEntities);
            var idConvertedToString = string.Join(",", itemzCollectionToReturn.Select(a => a.Id));

            _logger.LogDebug("Created {NumberOfItemzCreated} number of new Itemz and associated to ItemzType Id {ItemzTypeId}"
                , itemzCollectionToReturn.Count()
                , ItemzTypeId);
            return CreatedAtRoute("__GET_Itemz_Collection_By_GUID_IDS__",
                new { Controller = "ItemzCollection", ids = idConvertedToString }, itemzCollectionToReturn);

        }

        /// <summary>
        /// Used for Associating Itemz to ItemzType 
        /// </summary>
        /// <param name="ItemzTypeItemzDTO">Used for Associating Itemz to ItemzType through ItemzId and ItemzTypeId Respectively</param>
        /// <returns>GetItemzDTO for the Itemz that has specified ItemzType association</returns>
        /// <response code="200">Itemz to ItemzType association was either found or added successfully</response>
        /// <response code="404">Either Itemz or ItemzType was not found </response>
        [HttpPost(Name = "__POST_Associate_Itemz_To_ItemzType__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<GetItemzDTO> AssociateItemzToItemzType(ItemzTypeItemzDTO ItemzTypeItemzDTO)
        {
            if (!_itemzRepository.ItemzTypeExists(ItemzTypeItemzDTO.ItemzTypeId))
            {
                _logger.LogDebug("ItemzType with ID {ItemzTypeID} was not found in the repository", ItemzTypeItemzDTO.ItemzTypeId);
                return NotFound();
            }
            if (!_itemzRepository.ItemzExists(ItemzTypeItemzDTO.ItemzId))
            {
                _logger.LogDebug("Itemz with ID {itemzID} was not found in the repository", ItemzTypeItemzDTO.ItemzId);
                return NotFound();
            }

            _itemzRepository.AssociateItemzToItemzType(ItemzTypeItemzDTO);
            _itemzRepository.Save();
            _logger.LogDebug("HttpPost - ItemzType Itemz Association was either created or found for ItemzType ID {ItemzTypeID}" +
                " and Itemz Id {itemzId}", ItemzTypeItemzDTO.ItemzTypeId, ItemzTypeItemzDTO.ItemzId);

            return RedirectToRoute("__Single_Itemz_By_GUID_ID__", new { Controller = "Itemzs", ItemzId = ItemzTypeItemzDTO.ItemzId });
        }

        /// <summary>
        /// Move Itemz from one ItemzType to another
        /// </summary>
        /// <param name="ItemzTypeId">GUID representing an unique ID of the Target ItemzType for moving Itemz into</param>
        /// <param name="targetItemzTypeItemzDTO">Details about target ItemzType and Itemz association</param>
        /// <returns>No contents are returned when expected ItemzType and Itemz association is established</returns>
        /// <response code="204">No content are returned but status of 204 indicated that expected ItemzType and Itemz association is established</response>
        /// <response code="404">Either Itemz or ItemzType was not found</response>
        [HttpPut("{ItemzTypeId}", Name = "__PUT_Move_Itemz_Between_ItemzTypes__")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult MoveItemzBetweenItemzTypes(Guid ItemzTypeId, ItemzTypeItemzDTO targetItemzTypeItemzDTO)
        {
            if (!_itemzRepository.ItemzExists(targetItemzTypeItemzDTO.ItemzId))// Check if Itemz exists

            {
                _logger.LogDebug("HttpPut - Itemz for ID {ItemzId} could not be found", targetItemzTypeItemzDTO.ItemzId);
                return NotFound();
            }
            if (!_itemzRepository.ItemzTypeExists(targetItemzTypeItemzDTO.ItemzTypeId))  // Check if Target ItemzType Exists
            {
                _logger.LogDebug("HttpPut - Target ItemzType for ID {ItemzTypeId} could not be found", targetItemzTypeItemzDTO.ItemzTypeId);
                return NotFound();
            }

            var sourceItemzTypeItemzDTO = new ItemzTypeItemzDTO();
            sourceItemzTypeItemzDTO.ItemzId = targetItemzTypeItemzDTO.ItemzId;
            sourceItemzTypeItemzDTO.ItemzTypeId = ItemzTypeId;

            if (!_itemzRepository.ItemzTypeItemzExists(sourceItemzTypeItemzDTO))  // Check if Source ItemzTypeItemz association exists or not
            {
                _logger.LogDebug("HttpPut - Source ItemzType ID {ItemzTypeId} and Itemz ID {ItemzId} association could not be found",
                    sourceItemzTypeItemzDTO.ItemzTypeId,
                    sourceItemzTypeItemzDTO.ItemzId);

            }
            _itemzRepository.MoveItemzFromOneItemzTypeToAnother(sourceItemzTypeItemzDTO, targetItemzTypeItemzDTO);
            _itemzRepository.Save();

            _logger.LogDebug("HttpPut - Itemz ID {ItemzId} move from Source ItemzType ID {sourceItemzTypeID} " +
                "to Target ItemzType ID {targetItemzTypeID} was successfully completed", 
                sourceItemzTypeItemzDTO.ItemzId,
                sourceItemzTypeItemzDTO.ItemzTypeId,
                targetItemzTypeItemzDTO.ItemzTypeId);
            return NoContent(); // This indicates that update was successfully saved in the DB.

        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }


        /// <summary>
        /// Deleting a specific Itemz and ItemzType association. This will not delete Itemz or ItemzType from the database,
        /// instead it will only remove their association if found. 
        /// </summary>
        /// <returns>Status code 204 is returned without any content indicating that deletion of the specified ItemzType and Itemz association was successful</returns>
        /// <response code="404">ItemzType and Itemz association not found</response>
        [HttpDelete(Name = "__DELETE_ItemzType_and_Itemz_Association__")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult DeleteItemzTypeAndItemzAssociation(ItemzTypeItemzDTO ItemzTypeItemzDTO)
        {
            if (!_itemzRepository.ItemzTypeItemzExists(ItemzTypeItemzDTO))
            {
                _logger.LogDebug("Cannot find ItemzType and Itemz asscoaition for ItemzType ID " +
                    "{ItemzTypeId} and Itemz ID {ItemzId}", 
                    ItemzTypeItemzDTO.ItemzTypeId,
                    ItemzTypeItemzDTO.ItemzId);
                return NotFound();
            }

            _itemzRepository.RemoveItemzFromItemzType(ItemzTypeItemzDTO);
            _itemzRepository.Save();

            _logger.LogDebug("Delete ItemzType and Itemz asscoaition for ItemzType ID " +
                "{ItemzTypeId} and Itemz ID {ItemzId}", 
                ItemzTypeItemzDTO.ItemzTypeId, 
                ItemzTypeItemzDTO.ItemzId);
            return NoContent();
        }

        /// <summary>
        /// Get list of supported HTTP Options for the Itemz controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions (Name ="__OPTIONS_for_ItemzType_Itemz_Controller__")]
        public IActionResult GetItemzTypeItemzOptions()
        {
            Response.Headers.Add("Allow","GET,HEAD,POST,PUT,DELETE,OPTIONS");
            return Ok();
        }

        private string CreateItemzTypeItemzResourceUri(
            ItemzResourceParameter itemzResourceParameter,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("__GET_Itemzs_By_ItemzType__",
                        new
                        {
                            orderBy = itemzResourceParameter.OrderBy,
                            pageNumber = itemzResourceParameter.PageNumber - 1,
                            pageSize = itemzResourceParameter.PageSize
                        });
                case ResourceUriType.NextPage:
                    return Url.Link("__GET_Itemzs_By_ItemzType__",
                        new
                        {
                            orderBy = itemzResourceParameter.OrderBy,
                            pageNumber = itemzResourceParameter.PageNumber + 1,
                            pageSize = itemzResourceParameter.PageSize
                        });
                default:
                    return Url.Link("__GET_Itemzs_By_ItemzType__",
                        new
                        {
                            orderBy = itemzResourceParameter.OrderBy,
                            pageNumber = itemzResourceParameter.PageNumber,
                            pageSize = itemzResourceParameter.PageSize
                        });
            }
        }
    }
}