// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    [Route("api/ONLYforTestingItemz")] // e.g. http://HOST:PORT/api/ONLYforTestingItemz
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ONLYforTestingItemzController : ControllerBase
    {
        private readonly IItemzRepository _itemzRepository;
        private readonly ILogger<ONLYforTestingItemzController> _logger;

        public ONLYforTestingItemzController(IItemzRepository itemzRepository, ILogger<ONLYforTestingItemzController> logger)
        {
            _itemzRepository = itemzRepository ?? throw new ArgumentNullException(nameof(itemzRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// This option is designed only to be used for adding a new record while testing ItemzApp API.
        /// Used for creating new Itemz record in the database that also 
        /// accepts Itemz ID as part of input parameter
        /// </summary>
        /// <param name="itemz">Parameter that contains necessary properties for creating new Itemz in the database</param>
        /// <returns>Newly created Itemz property details</returns>
        /// <response code="201">Returns newly created itemzs property details</response>
        [HttpPost(Name = "__POST_ONLY_FOR_TESTING_Create_Itemz__")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetItemzDTO>> CreateItemzAsync(Itemz itemz)
        {
            if (!(await _itemzRepository.ItemzExistsAsync(itemz.Id)))
            {
                _itemzRepository.AddItemz(itemz);
                await _itemzRepository.SaveAsync();
                _logger.LogDebug("{FormattedControllerAndActionNames}Created new Itemz with ID {ItemzId} via __POST_ONLY_FOR_TESTING_Create_Itemz__",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                    itemz.Id);
            }
            //            return CreatedAtRoute("__Single_Itemz_By_GUID_ID__", new { Controller = "Itemzs", ItemzId = itemz.Id }, _itemzRepository.GetItemz(itemz.Id));
            return CreatedAtRoute("__Single_Itemz_By_GUID_ID__", new { Controller = "Itemzs", ItemzId = itemz.Id }, itemz);
        }
        /// <summary>
        /// Get list of supported HTTP Options for the ONLYforTestingItemz controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions (Name = "__OPTIONS_ONLY_FOR_TESTING_Get_Itemz__")]
        public IActionResult GetItemzOptions()
        {
            Response.Headers.Add("Allow", "OPTIONS,POST");
            return Ok();
        }
    }
}
