// Licensed under the Apache License, Version 2.0. See License.txt in the itemzType root for license information.

using System;
using ItemzApp.API.Entities;
using ItemzApp.API.Models;
using ItemzApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ItemzApp.API.Helper;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")] // e.g. http://HOST:PORT/api/ONLYforTestingItemzTypes
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ONLYforTestingItemzTypesController : ControllerBase
    {
        private readonly IItemzTypeRepository _itemzTypeRepository;
        private readonly ILogger<ONLYforTestingItemzTypesController> _logger;

        public ONLYforTestingItemzTypesController(IItemzTypeRepository itemzTypeRepository, ILogger<ONLYforTestingItemzTypesController> logger)
        {
            _itemzTypeRepository = itemzTypeRepository ?? throw new ArgumentNullException(nameof(itemzTypeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// This option is designed only to be used for adding a new record while testing ItemzApp API.
        /// Used for creating new ItemzType record in the database that also 
        /// accepts ItemzType ID as part of input parameter
        /// </summary>
        /// <param name="itemzType">Parameter that contains necessary properties for creating new itemzType in the database</param>
        /// <returns>Newly created ItemzType property details</returns>
        /// <response code="201">Returns newly created itemzType's property details</response>
        [HttpPost(Name = "__POST_ONLY_FOR_TESTING_Create_ItemzType__")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetItemzTypeDTO>> CreateItemzTypeAsync(ItemzType itemzType)
        {
            if (!(await _itemzTypeRepository.ItemzTypeExistsAsync(itemzType.Id)))
            {
                _itemzTypeRepository.AddItemzType(itemzType);
                await _itemzTypeRepository.SaveAsync();
                _logger.LogDebug("{FormattedControllerAndActionNames}Created new ItemzType with ID {ItemzTypeId} via __POST_ONLY_FOR_TESTING_Create_ItemzType__",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                    itemzType.Id);
            }
            return CreatedAtRoute("__Single_ItemzType_By_GUID_ID__", new { Controller = "ItemzTypes", ItemzTypeId = itemzType.Id }, await _itemzTypeRepository.GetItemzTypeAsync(itemzType.Id));
        }
        /// <summary>
        /// Get list of supported HTTP Options for the ONLYforTestingItemzTypes controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions(Name = "__OPTIONS_ONLY_FOR_TESTING_Get_ItemzType__")]
        public IActionResult GetItemzTypesOptions()
        {
            Response.Headers.Add("Allow", "OPTIONS,POST");
            return Ok();
        }


    }
}
