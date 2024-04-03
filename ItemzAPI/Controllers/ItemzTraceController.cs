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
using System.Threading.Tasks;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    [Route("api/ItemzTrace")] // e.g. http://HOST:PORT/api/ItemzTypeItemzs
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ItemzTraceController : ControllerBase
    {
        private readonly IItemzTraceRepository _itemzTraceRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILogger<ItemzTypeItemzsController> _logger;

        public ItemzTraceController(IItemzTraceRepository itemzTraceRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService,
            ILogger<ItemzTypeItemzsController> logger)
        {
            _itemzTraceRepository = itemzTraceRepository ?? throw new ArgumentNullException(nameof(itemzTraceRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }



        /// <summary>
        /// Check if specific Itemz Trace association exists
        /// </summary>
        /// <param name="fromTraceItemzId">Provide From Trace Itemz Id</param>
        /// <param name="toTraceItemzId">Provide To Trace Itemz Id</param>
        /// <returns>ItemzTraceDTO for the Itemz that has specified Itemz Trace</returns>
        /// <response code="200">Returns ItemzTraceDTO for the From and To Itemz Trace</response>
        /// <response code="404">Itemz Trace was not found</response>
        [HttpGet("CheckExists/", Name = "__GET_Check_Itemz_Trace_Exists__")]
        [HttpHead("CheckExists/", Name = "__HEAD_Check_Itemz_Trace_Exists__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]

        public async Task<ActionResult<ItemzTraceDTO>> CheckItemzTraceExistsAsync([FromQuery] Guid fromTraceItemzId, Guid toTraceItemzId) // TODO: Try from Query.
        {
            var tempItemzTraceDTO = new ItemzTraceDTO();

            tempItemzTraceDTO.FromTraceItemzId = fromTraceItemzId;
            tempItemzTraceDTO.ToTraceItemzId = toTraceItemzId;
            if (!(await _itemzTraceRepository.ItemzsTraceExistsAsync(tempItemzTraceDTO)))  // Check if ItemzTrace association exists or not
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}From Itemz ID {fromTraceItemzId} and To Itemz ID {toTraceItemzId} Trace could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    tempItemzTraceDTO.FromTraceItemzId,
                    tempItemzTraceDTO.ToTraceItemzId);
                return NotFound();
            }
            _logger.LogDebug("{FormattedControllerAndActionNames}From Itemz ID {fromTraceItemzId} and To Itemz ID {toTraceItemzId} Trace was found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    tempItemzTraceDTO.FromTraceItemzId,
                    tempItemzTraceDTO.ToTraceItemzId);
            return Ok(tempItemzTraceDTO);
        }


        /// <summary>
        /// Used for Establishing Trace link between Itemz 
        /// </summary>
        /// <param name="itemzTraceDTO">Used for Associating Trace between two Itemz </param>
        /// <returns>ItemzTraceDTO for the Itemz Trace Association</returns>
        /// <response code="200">Itemz Trace association was either found or added successfully</response>
        /// <response code="404">Either FromItemz or ToItemz was not found </response>
        [HttpPost(Name = "__POST_Establish_Trace_Between_Itemz__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ItemzTraceDTO>> EstablishTraceBetweenItemzAsync(ItemzTraceDTO itemzTraceDTO)
        {
            if (!(await _itemzTraceRepository.ItemzExistsAsync(itemzTraceDTO.FromTraceItemzId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz with ID {ItemzTraceFromItemzID} was not found in the repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzTraceDTO.FromTraceItemzId);
                return NotFound();
            }
            if (!(await _itemzTraceRepository.ItemzExistsAsync(itemzTraceDTO.ToTraceItemzId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz with ID {ItemzTraceToItemzID} was not found in the repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzTraceDTO.ToTraceItemzId);
                return NotFound();
            }

            await _itemzTraceRepository.EstablishTraceBetweenItemzAsync(itemzTraceDTO);
            await _itemzTraceRepository.SaveAsync();
            _logger.LogDebug("{FormattedControllerAndActionNames}Itemz Trace was either created or found between " +
                "Parent FromItemz {FromItemzTrace} and Child ToItemz {ToItemzTrace}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                itemzTraceDTO.FromTraceItemzId, 
                itemzTraceDTO.ToTraceItemzId);

            return itemzTraceDTO;
        }


        /// <summary>
        /// Used for deleting Trace link between Itemz. This will not delete Itemz from the database,
        /// instead it will only remove their trace association if found. 
        /// </summary>
        /// <returns>Status code 204 is returned without any content indicating that deletion of the specified FromItemz and ToItemz trace association was successful</returns>
        /// <response code="404">FromItemz and ToItemz Trace not found</response>
        [HttpDelete(Name = "__DELETE_Itemz_Trace__")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteItemzTraceAsync(ItemzTraceDTO itemzTraceDTO)
        {
            if (!(await _itemzTraceRepository.ItemzsTraceExistsAsync(itemzTraceDTO)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Cannot find trace asscoaition between " +
                    "Itemz ID {FromItemz} and Itemz ID {ToItemz}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzTraceDTO.FromTraceItemzId,
                    itemzTraceDTO.ToTraceItemzId);
                return NotFound();
            }

            await _itemzTraceRepository.RemoveItemzTraceAsync(itemzTraceDTO);
            await _itemzTraceRepository.SaveAsync();

            _logger.LogDebug("{FormattedControllerAndActionNames}Deleted Itemz Trace between " +
                "Itemz ID {FromItemz} and Itemz ID {ToItemz}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                itemzTraceDTO.FromTraceItemzId,
                itemzTraceDTO.ToTraceItemzId);
            return NoContent();
        }

        /// <summary>
        /// Gets collection of Itemz Traces by Itemz ID
        /// </summary>
        /// <param name="itemzId">Itemz ID for which Itemz Traces are queried</param>
        /// <returns>Collection of Itemz Traces by Itemz ID</returns>
        /// <response code="200">Returns Collection of Itemz Traces by Itemz ID</response>
        /// <response code="404">Either ItemzID was not found or No Itemz Traces were found for given ItemzID</response>
        [HttpGet("{itemzId:Guid}", Name = "__GET_Itemz_Traces_By_ItemzID__")]
        [HttpHead("{itemzId:Guid}", Name = "__HEAD_Itemz_Traces_By_ItemzID__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<ItemzTraceDTO>>> GetItemzTracesByItemzIDAsync(Guid itemzId)
        {
            if (!(await _itemzTraceRepository.ItemzExistsAsync(itemzId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz with ID {itemzId} was not found in the repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
                return NotFound();
            }

            var itemzTracesFromRepo = await  _itemzTraceRepository.GetAllTracesByItemzIdAsync(itemzId);

            // EXPLANATION : Check if list is IsNullOrEmpty
            // By default we don't have option baked in the .NET to check
            // for null or empty for List type. In the following code we are first checking
            // for nullable itemzsFromRepo? and then for count great then zero via Any()
            // If any of above is true then we return true. This way we log that no itemz traces were
            // found in the database.
            // Ref: https://stackoverflow.com/a/54549818
            if (!itemzTracesFromRepo?.Any() ?? true)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}No Itemz Traces found for Itemz with ID {ItemzId}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
                // TODO: If no Itemz Traces are found for an ItemzID then shall we return an error back to the calling client?
                return NotFound();
            }
            _logger.LogDebug("{FormattedControllerAndActionNames}In total {ItemzTraceNumbers} Itemz Traces found in Itemz with ID {ItemzId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                itemzTracesFromRepo?.Count(), itemzId);

            _logger.LogDebug("{FormattedControllerAndActionNames}Returning results for {ItemzNumbers} Itemzs to the caller",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                itemzTracesFromRepo?.Count());
            return Ok(_mapper.Map<IEnumerable<ItemzTraceDTO>>(itemzTracesFromRepo));
        }


        /// <summary>
        /// Gets All Parent and Child Itemz Traces byItemz ID
        /// </summary>
        /// <param name="itemzId">Itemz ID for which Parent and Child Itemz Traces are returned.</param>
        /// <returns>Collection of all Parent and Child Itemz Traces by Itemz ID</returns>
        /// <response code="200">Returns Collection of all Parent and Child Itemz Traces by Itemz ID</response>
        /// <response code="404">ItemzID was not found in the repository</response>
        [HttpGet("AllItemzTraces/{itemzId:Guid}", Name = "__GET_All_Parent_and_Child_Itemz_Traces_By_ItemzID__")]
        [HttpHead("AllItemzTraces/{itemzId:Guid}", Name = "__HEAD_All_Parent_and_Child_Itemz_Traces_By_ItemzID__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ItemzParentAndChildTraceDTO>> GetAllParentAndChildTracesByItemzIdAsync(Guid itemzId)
        {
            if (!(await _itemzTraceRepository.ItemzExistsAsync(itemzId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz with ID {itemzId} was not found in the repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    itemzId);
                return NotFound();
            }

            var itemzParentAndChildTraceDTO = await _itemzTraceRepository.GetAllParentAndChildTracesByItemzIdAsync(itemzId);

            _logger.LogDebug("{FormattedControllerAndActionNames}In total {ParentItemzTraceCount} Parent Itemz Traces found for Itemz with ID {ItemzId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                itemzParentAndChildTraceDTO.Itemz?.ParentItemz?.Count, itemzId);

            _logger.LogDebug("{FormattedControllerAndActionNames}In total {ChildItemzTraceCount} Child Itemz Traces found for Itemz with ID {ItemzId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                itemzParentAndChildTraceDTO.Itemz?.ChildItemz?.Count, itemzId);

            return Ok(itemzParentAndChildTraceDTO);
        }



        /// <summary>
        /// Get count of FromItemz Traces associated with ItemzID
        /// </summary>
        /// <param name="ItemzId">Provide ItemzId in GUID form</param>
        /// <returns>Integer representing total number of direct From Itemz Traces associated with ItemzID</returns>
        /// <response code="200">Count of From Itemz Traces associated with ItemzID. ZERO means no From Itemz Traces were found for targeted ItemzID</response>
        /// <response code="404">Itemz for given ID could not be found</response>
        [HttpGet("GetFromItemzTraceCount/{ItemzId:Guid}", Name = "__GET_FromItemz_Count_By_ItemzID__")]
        [HttpHead("GetFromItemzTraceCount/{ItemzId:Guid}", Name = "__HEAD_FromItemz_Count_By_ItemzID__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]

        public async Task<ActionResult<int>> GetFromItemzTraceCountByItemzID(Guid ItemzId)
        {
            if (!(await _itemzTraceRepository.ItemzExistsAsync(ItemzId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz with ID {itemzId} was not found in the repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    ItemzId);
                return NotFound();
            }
            int countOfFromItemzTraces = await _itemzTraceRepository.GetFromTraceCountByItemz(ItemzId);
            _logger.LogDebug("{FormattedControllerAndActionNames}In total {countOfFromItemzTracess} From Itemz Traces were found associated with ItemzID {ItemzID}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                countOfFromItemzTraces,
                ItemzId);
            return Ok(countOfFromItemzTraces);
        }


        //        /// <summary>
        //        /// Get count of Itemzs associated with ItemzType
        //        /// </summary>
        //        /// <param name="ItemzTypeId">Provide ItemzType Id in GUID form</param>
        //        /// <returns>Integer representing total number of Itemzs associated with ItemzType</returns>
        //        /// <response code="200">Count of Itemzs associated with ItemzType. ZERO means no Itemz were found for targeted ItemzType</response>
        //        /// <response code="404">ItemzType for given ID could not be found</response>
        //        [HttpGet("GetItemzCount/{ItemzTypeId:Guid}", Name = "__GET_Itemz_Count_By_ItemzType__")]
        //        [HttpHead("GetItemzCount/{ItemzTypeId:Guid}", Name = "__HEAD_Itemz_Count_By_ItemzType__")]
        //        [ProducesResponseType(StatusCodes.Status200OK)]
        //        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //        [ProducesDefaultResponseType]

        //        public async Task<ActionResult<int>> GetItemzCountByItemzType(Guid ItemzTypeId)
        //        {

        //            if (!(await _itemzRepository.ItemzTypeExistsAsync(ItemzTypeId)))
        //            {
        //                _logger.LogDebug("{FormattedControllerAndActionNames}ItemzType with ID {ItemzTypeID} was not found in the repository",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
        //                    ItemzTypeId);
        //                return NotFound();
        //            }
        //            var countOfItemzs = -1;
        //            countOfItemzs = await _itemzRepository.GetItemzsCountByItemzType(ItemzTypeId);
        //            _logger.LogDebug("{FormattedControllerAndActionNames}In total {countOfItemzs} Itemzs were found associated with ItemzType with ID {ItemzTypeID}",
        //                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
        //                countOfItemzs,
        //                ItemzTypeId);
        //            return countOfItemzs;
        //        }

        //            /// <summary>
        //            /// Check if specific ItemzType and Itemz association exists
        //            /// </summary>
        //            /// <param name="ItemzTypeId">Provide ItemzType Id</param>
        //            /// <param name="itemzId">Provide Itemz Id</param>
        //            /// <returns>GetItemzDTO for the Itemz that has specified ItemzType association</returns>
        //            /// <response code="200">Returns GetItemzDTO for the Itemz that has specified ItemzType association</response>
        //            /// <response code="404">No ItemzType and Itemzs association was found</response>
        //            [HttpGet("CheckExists/", Name = "__GET_Check_ItemzType_Itemz_Association_Exists__")]
        //        [HttpHead("CheckExists/", Name = "__HEAD_Check_ItemzType_Itemz_Association_Exists__")]
        //        [ProducesResponseType(StatusCodes.Status200OK)]
        //        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //        [ProducesDefaultResponseType]

        //        public async Task<ActionResult<GetItemzDTO>> CheckItemzTypeItemzAssociationExistsAsync([FromQuery] Guid ItemzTypeId, Guid itemzId) // TODO: Try from Query.
        //        {
        //            var tempItemzTypeItemzDTO = new ItemzTypeItemzDTO();

        //            tempItemzTypeItemzDTO.ItemzTypeId = ItemzTypeId;
        //            tempItemzTypeItemzDTO.ItemzId = itemzId;
        //            if (!(await _itemzRepository.ItemzTypeItemzExistsAsync(tempItemzTypeItemzDTO)))  // Check if ItemzTypeItemz association exists or not
        //            {
        //                _logger.LogDebug("{FormattedControllerAndActionNames}ItemzType ID {ItemzTypeId} and Itemz ID {ItemzId} association could not be found",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
        //                    tempItemzTypeItemzDTO.ItemzTypeId,
        //                    tempItemzTypeItemzDTO.ItemzId);
        //                return NotFound();
        //            }
        //            _logger.LogDebug("{FormattedControllerAndActionNames}ItemzType ID {ItemzTypeId} and Itemz ID {ItemzId} association was found",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
        //                    tempItemzTypeItemzDTO.ItemzTypeId,
        //                    tempItemzTypeItemzDTO.ItemzId);
        //            return RedirectToRoute("__Single_Itemz_By_GUID_ID__", new { Controller = "Itemzs", ItemzId = tempItemzTypeItemzDTO.ItemzId });

        //        }

        //        /// <summary>
        //        /// Used for creating new Itemz record in the database by ItemzType ID
        //        /// </summary>
        //        /// <param name="ItemzTypeId">ItemzType ID in Guid Form. New Itemzs will be associated with provided ItemzType Id</param>
        //        /// <param name="itemzCollection">Used for populating information in the newly created itemz in the database by ItemzType ID</param>
        //        /// <returns>Newly created Itemzs property details</returns>
        //        /// <response code="201">Returns newly created itemzs property details</response>
        //        [HttpPost("{ItemzTypeId:Guid}", Name = "__POST_Create_Itemz_Collecction_By_ItemzType__")]
        //        [ProducesResponseType(StatusCodes.Status201Created)]
        //        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //        [ProducesDefaultResponseType]
        //        public async Task<ActionResult<IEnumerable<GetItemzDTO>>> CreateItemzCollectionByItemzTypeAsync(
        //             Guid ItemzTypeId,
        //            IEnumerable<CreateItemzDTO> itemzCollection)
        //        {
        //            if (!(await _itemzRepository.ItemzTypeExistsAsync(ItemzTypeId)))
        //            {
        //                _logger.LogDebug("{FormattedControllerAndActionNames}ItemzType with ID {ItemzTypeID} was not found in the repository",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
        //                    ItemzTypeId);
        //                return NotFound();
        //            }

        //            var itemzEntities = _mapper.Map<IEnumerable<Entities.Itemz>>(itemzCollection);
        //            foreach (var itemz in itemzEntities)
        //            {
        //                _itemzRepository.AddItemzByItemzType(itemz, ItemzTypeId);
        //            }
        //            await _itemzRepository.SaveAsync();

        //            var itemzCollectionToReturn = _mapper.Map<IEnumerable<GetItemzDTO>>(itemzEntities);
        //            var idConvertedToString = string.Join(",", itemzCollectionToReturn.Select(a => a.Id));

        //            _logger.LogDebug("{FormattedControllerAndActionNames}Created {NumberOfItemzCreated} number of new Itemz and associated to ItemzType Id {ItemzTypeId}",
        //                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
        //                itemzCollectionToReturn.Count(),
        //                ItemzTypeId);
        //            return CreatedAtRoute("__GET_Itemz_Collection_By_GUID_IDS__",
        //                new { Controller = "ItemzCollection", ids = idConvertedToString }, itemzCollectionToReturn);

        //        }

        //        /// <summary>
        //        /// Used for Associating Itemz to ItemzType 
        //        /// </summary>
        //        /// <param name="ItemzTypeItemzDTO">Used for Associating Itemz to ItemzType through ItemzId and ItemzTypeId Respectively</param>
        //        /// <returns>GetItemzDTO for the Itemz that has specified ItemzType association</returns>
        //        /// <response code="200">Itemz to ItemzType association was either found or added successfully</response>
        //        /// <response code="404">Either Itemz or ItemzType was not found </response>
        //        [HttpPost(Name = "__POST_Associate_Itemz_To_ItemzType__")]
        //        [ProducesResponseType(StatusCodes.Status200OK)]
        //        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //        [ProducesDefaultResponseType]
        //        public async Task<ActionResult<GetItemzDTO>> AssociateItemzToItemzTypeAsync(ItemzTypeItemzDTO ItemzTypeItemzDTO)
        //        {
        //            if (!(await _itemzRepository.ItemzTypeExistsAsync(ItemzTypeItemzDTO.ItemzTypeId)))
        //            {
        //                _logger.LogDebug("{FormattedControllerAndActionNames}ItemzType with ID {ItemzTypeID} was not found in the repository",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
        //                    ItemzTypeItemzDTO.ItemzTypeId);
        //                return NotFound();
        //            }
        //            if (!(await _itemzRepository.ItemzExistsAsync(ItemzTypeItemzDTO.ItemzId)))
        //            {
        //                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz with ID {itemzID} was not found in the repository",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
        //                    ItemzTypeItemzDTO.ItemzId);
        //                return NotFound();
        //            }

        //            _itemzRepository.AssociateItemzToItemzType(ItemzTypeItemzDTO);
        //            await _itemzRepository.SaveAsync();
        //            _logger.LogDebug("{FormattedControllerAndActionNames}ItemzType Itemz Association was either created or found for ItemzType ID {ItemzTypeID}" +
        //                " and Itemz Id {itemzId}",
        //                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
        //                ItemzTypeItemzDTO.ItemzTypeId, ItemzTypeItemzDTO.ItemzId);

        //            return RedirectToRoute("__Single_Itemz_By_GUID_ID__", new { Controller = "Itemzs", ItemzId = ItemzTypeItemzDTO.ItemzId });
        //        }

        //        /// <summary>
        //        /// Move Itemz from one ItemzType to another
        //        /// </summary>
        //        /// <param name="ItemzTypeId">GUID representing an unique ID of the Source ItemzType from which Itemz has to be moved</param>
        //        /// <param name="targetItemzTypeItemzDTO">Details about target ItemzType and Itemz association</param>
        //        /// <returns>No contents are returned when expected ItemzType and Itemz association is established</returns>
        //        /// <response code="204">No content are returned but status of 204 indicated that expected ItemzType and Itemz association is established</response>
        //        /// <response code="404">Either Itemz or ItemzType was not found</response>
        //        [HttpPut("{ItemzTypeId}", Name = "__PUT_Move_Itemz_Between_ItemzTypes__")]
        //        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //        [ProducesDefaultResponseType]
        //        public async Task<ActionResult> MoveItemzBetweenItemzTypesAsync(Guid ItemzTypeId, ItemzTypeItemzDTO targetItemzTypeItemzDTO)
        //        {
        //            if (!(await _itemzRepository.ItemzExistsAsync(targetItemzTypeItemzDTO.ItemzId)))// Check if Itemz exists

        //            {
        //                _logger.LogDebug("{FormattedControllerAndActionNames}Itemz for ID {ItemzId} could not be found",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
        //                    targetItemzTypeItemzDTO.ItemzId);
        //                return NotFound();
        //            }
        //            if (!(await _itemzRepository.ItemzTypeExistsAsync(targetItemzTypeItemzDTO.ItemzTypeId)))  // Check if Target ItemzType Exists
        //            {
        //                _logger.LogDebug("{FormattedControllerAndActionNames}Target ItemzType for ID {ItemzTypeId} could not be found",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
        //                    targetItemzTypeItemzDTO.ItemzTypeId);
        //                return NotFound();
        //            }

        //            var sourceItemzTypeItemzDTO = new ItemzTypeItemzDTO();
        //            sourceItemzTypeItemzDTO.ItemzId = targetItemzTypeItemzDTO.ItemzId;
        //            sourceItemzTypeItemzDTO.ItemzTypeId = ItemzTypeId;

        //            if (!(await _itemzRepository.ItemzTypeItemzExistsAsync(sourceItemzTypeItemzDTO)))  // Check if Source ItemzTypeItemz association exists or not
        //            {
        //                _logger.LogDebug("{FormattedControllerAndActionNames}Source ItemzType ID {ItemzTypeId} and Itemz ID {ItemzId} association could not be found",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
        //                    sourceItemzTypeItemzDTO.ItemzTypeId,
        //                    sourceItemzTypeItemzDTO.ItemzId);

        //            }
        //            _itemzRepository.MoveItemzFromOneItemzTypeToAnother(sourceItemzTypeItemzDTO, targetItemzTypeItemzDTO);
        //            await _itemzRepository.SaveAsync();

        //            _logger.LogDebug("{FormattedControllerAndActionNames}Itemz ID {ItemzId} move from Source ItemzType ID {sourceItemzTypeID} " +
        //                "to Target ItemzType ID {targetItemzTypeID} was successfully completed",
        //                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
        //                sourceItemzTypeItemzDTO.ItemzId,
        //                sourceItemzTypeItemzDTO.ItemzTypeId,
        //                targetItemzTypeItemzDTO.ItemzTypeId);
        //            return NoContent(); // This indicates that update was successfully saved in the DB.

        //        }

        //        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        //        {
        //            var options = HttpContext.RequestServices
        //                .GetRequiredService<IOptions<ApiBehaviorOptions>>();

        //            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        //        }


        //        /// <summary>
        //        /// Deleting a specific Itemz and ItemzType association. This will not delete Itemz or ItemzType from the database,
        //        /// instead it will only remove their association if found. 
        //        /// </summary>
        //        /// <returns>Status code 204 is returned without any content indicating that deletion of the specified ItemzType and Itemz association was successful</returns>
        //        /// <response code="404">ItemzType and Itemz association not found</response>
        //        [HttpDelete(Name = "__DELETE_ItemzType_and_Itemz_Association__")]
        //        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //        [ProducesDefaultResponseType]
        //        public async Task<ActionResult> DeleteItemzTypeAndItemzAssociationAsync(ItemzTypeItemzDTO ItemzTypeItemzDTO)
        //        {
        //            if (!(await _itemzRepository.ItemzTypeItemzExistsAsync(ItemzTypeItemzDTO)))
        //            {
        //                _logger.LogDebug("{FormattedControllerAndActionNames}Cannot find ItemzType and Itemz asscoaition for ItemzType ID " +
        //                    "{ItemzTypeId} and Itemz ID {ItemzId}",
        //                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
        //                    ItemzTypeItemzDTO.ItemzTypeId,
        //                    ItemzTypeItemzDTO.ItemzId);
        //                return NotFound();
        //            }

        //            _itemzRepository.RemoveItemzFromItemzType(ItemzTypeItemzDTO);
        //            await _itemzRepository.SaveAsync();

        //            _logger.LogDebug("{FormattedControllerAndActionNames}Delete ItemzType and Itemz asscoaition for ItemzType ID " +
        //                "{ItemzTypeId} and Itemz ID {ItemzId}",
        //                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
        //                ItemzTypeItemzDTO.ItemzTypeId, 
        //                ItemzTypeItemzDTO.ItemzId);
        //            return NoContent();
        //        }

        //        /// <summary>
        //        /// Get list of supported HTTP Options for the Itemz controller.
        //        /// </summary>
        //        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        //        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        //        [HttpOptions (Name ="__OPTIONS_for_ItemzType_Itemz_Controller__")]
        //        public IActionResult GetItemzTypeItemzOptions()
        //        {
        //            Response.Headers.Add("Allow","GET,HEAD,POST,PUT,DELETE,OPTIONS");
        //            return Ok();
        //        }

        //        private string CreateItemzTypeItemzResourceUri(
        //            ItemzResourceParameter itemzResourceParameter,
        //            ResourceUriType type)
        //        {
        //            switch (type)
        //            {
        //                case ResourceUriType.PreviousPage:
        //                    return Url.Link("__GET_Itemzs_By_ItemzType__",
        //                        new
        //                        {
        //                            orderBy = itemzResourceParameter.OrderBy,
        //                            pageNumber = itemzResourceParameter.PageNumber - 1,
        //                            pageSize = itemzResourceParameter.PageSize
        //                        })!;
        //                case ResourceUriType.NextPage:
        //                    return Url.Link("__GET_Itemzs_By_ItemzType__",
        //                        new
        //                        {
        //                            orderBy = itemzResourceParameter.OrderBy,
        //                            pageNumber = itemzResourceParameter.PageNumber + 1,
        //                            pageSize = itemzResourceParameter.PageSize
        //                        })!;
        //                default:
        //                    return Url.Link("__GET_Itemzs_By_ItemzType__",
        //                        new
        //                        {
        //                            orderBy = itemzResourceParameter.OrderBy,
        //                            pageNumber = itemzResourceParameter.PageNumber,
        //                            pageSize = itemzResourceParameter.PageSize
        //                        })!;
        //            }
        //        }
    }
}
