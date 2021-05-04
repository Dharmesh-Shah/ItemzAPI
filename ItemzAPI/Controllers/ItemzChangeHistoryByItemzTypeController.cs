﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.Helper;
using ItemzApp.API.Models;
using ItemzApp.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    [Route("api/ItemzChangeHistoryByItemzType")] // e.g. http://HOST:PORT/api/ItemzChangeHistoryByItemzType
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ItemzChangeHistoryByItemzTypeController : ControllerBase
    {
        private readonly IItemzChangeHistoryByItemzTypeRepository _itemzChangeHistoryByItemzTypeRepository;
        private readonly ILogger<ItemzChangeHistoryByItemzTypeController> _logger;

        public ItemzChangeHistoryByItemzTypeController(
            IItemzChangeHistoryByItemzTypeRepository itemzChangeHistoryByItemzTypeRepository,
            ILogger<ItemzChangeHistoryByItemzTypeController> logger)
        {
            _itemzChangeHistoryByItemzTypeRepository = itemzChangeHistoryByItemzTypeRepository;
            _logger = logger;
        }

        /// <summary>
        /// Deleting ItemzChangeHistory for all the Itemz that are associated with given ItemzType ID upto provided Date and Time.
        /// </summary>
        /// <param name="deleteItemzChangeHistoryByItemzTypeDTO">Provide ItemzTypeID representated in GUID form along with Upto Date Time indicating till the time associated Itemz Change History data has to be deleted.</param>
        /// <returns>Status code 200 is returned without any content indicating that action to delete Itemz Change History by Itemz Type was successful. Either it found older records to be deleted or it did not find any records to be deleted.</returns>
        /// <response code="200">Returns number of Itemz Change History records that were deleted by Itemz Type</response>
        [HttpDelete(Name = "__DELETE_Itemz_Change_History_By_ItemzType_GUID_ID__")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> DeleteItemzChangeHistoryAsync(DeleteChangeHistoryDTO deleteItemzChangeHistoryByItemzTypeDTO)
        {
            var numberOfDeletedRecords = await _itemzChangeHistoryByItemzTypeRepository.DeleteItemzChangeHistoryByItemzTypeAsync(deleteItemzChangeHistoryByItemzTypeDTO.Id, deleteItemzChangeHistoryByItemzTypeDTO.UptoDateTime);

            _logger.LogDebug("{FormattedControllerAndActionNames}Deleted {numberOfDeletedRecords} record(s) from Itemz Change History associated with Itemz Type ID {Id} upto Date Time {UptoDateTime}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                numberOfDeletedRecords, 
                deleteItemzChangeHistoryByItemzTypeDTO.Id,
                deleteItemzChangeHistoryByItemzTypeDTO.UptoDateTime);
            return Ok(numberOfDeletedRecords);
        }

        /// <summary>
        /// Number of ItemzChangeHistory records for all the Itemz that are associated with given ItemzType ID
        /// </summary>
        /// <param name="ItemzTypeId">Provide ItemzTypeID representated in GUID form</param>
        /// <returns>Number of records found for ItemzChangeHistory indirectly associated with a given ItemzTypeID</returns>
        /// <response code="200">Returns number of Itemz Change History records that were indirectly associated with a given Itemz Type</response>
        [HttpGet("{ItemzTypeId:Guid}", Name = "__GET_Number_of_ItemzChangeHistory_By_ItemzType")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<ActionResult<int>> GetNumberOfItemzChangeHistoryByItemzTypeAsync(Guid ItemzTypeId)
        {
            var foundItemzChangeHistoryByItemzTypeId = await _itemzChangeHistoryByItemzTypeRepository.TotalNumberOfItemzChangeHistoryByItemzTypeAsync(ItemzTypeId);
            _logger.LogDebug("{FormattedControllerAndActionNames} Found {foundItemzChangeHistoryByItemzTypeId} ItemzChangeHistory records for ItemzType with ID {ItemzTypeId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                foundItemzChangeHistoryByItemzTypeId,
                ItemzTypeId);
            return foundItemzChangeHistoryByItemzTypeId;
        }

        /// <summary>
        /// Number of ItemzChangeHistory records for all the Itemz that are associated with given ItemzType ID upto provided Date and Time.
        /// </summary>
        /// <param name="getItemzChangeHistoryByItemzTypeDTO">Provide ItemzTypeID representated in GUID form along with cut off upto DateTime.</param>
        /// <returns>Number of records found for ItemzChangeHistory indirectly associated with a given ItemzTypeID</returns>
        /// <response code="200">Returns number of Itemz Change History records that were indirectly associated with a given Itemz Type upto provided Date and Time.</response>
        [HttpGet(Name = "__GET_Number_of_ItemzChangeHistory_By_ItemzType_Upto_DateTime")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<ActionResult<int>> GetNumberOfItemzChangeHistoryByItemzTypeUptoDateTimeAsync(GetNumberOfChangeHistoryDTO getItemzChangeHistoryByItemzTypeDTO)
        {
            var foundItemzChangeHistoryByItemzTypeId = await _itemzChangeHistoryByItemzTypeRepository.TotalNumberOfItemzChangeHistoryByItemzTypeUptoDateTimeAsync(
                    getItemzChangeHistoryByItemzTypeDTO.Id,
                    getItemzChangeHistoryByItemzTypeDTO.UptoDateTime);
            _logger.LogDebug("{FormattedControllerAndActionNames} Found {foundItemzChangeHistoryByItemzTypeId} ItemzChangeHistory records for ItemzType with ID {ItemzTypeId} upto Date Time {UptoDateTime}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                foundItemzChangeHistoryByItemzTypeId,
                getItemzChangeHistoryByItemzTypeDTO.Id,
                getItemzChangeHistoryByItemzTypeDTO.UptoDateTime);
            return foundItemzChangeHistoryByItemzTypeId;
        }
    }
}
