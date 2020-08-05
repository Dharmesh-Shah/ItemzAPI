﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using AutoMapper;
using ItemzApp.API.Helper;
using ItemzApp.API.Models;
using ItemzApp.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    [Route("api/itemzcollection")] // e.g. http://HOST:PORT/api/itemzcollection
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[Produces("application/json")]

    public class ItemzCollectionController : ControllerBase
    {
        private readonly IItemzRepository _itemzRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemzCollectionController> _logger;
        public ItemzCollectionController(IItemzRepository itemzRepository,
            IMapper mapper,
            ILogger<ItemzCollectionController> logger)
        {
            _itemzRepository = itemzRepository ?? throw new ArgumentNullException(nameof(itemzRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets collection of Itemzs without any Pagination
        /// </summary>
        /// <param name="ids">Array of Itemz Id (in GUID form) for which details has to be returned to the caller</param>
        /// <returns>Collection of Itemz that are requested via Array of Itemz Id</returns>
        /// <response code="200">Collection of Itemzs property details based on Itemz Ids that were passed in as parameter</response>
        /// <response code="500">Bad Request - Itemz Ids should be passed in as parameter</response>
        /// <response code="404">No Itemzs were found based on provided list of Itemz Ids</response>
        /// <remarks>
        /// Sample request (this request will get itemz by Ids) \
        /// GET api/ItemzCollection/(9153a516-d69e-4364-b17e-03b87442e21c,5e76f8e8-d3e7-41db-b084-f64c107c6783) 
        /// </remarks>
        [HttpGet("({ids})", Name = "__GET_Itemz_Collection_By_GUID_IDS__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetItemzCollectionAsync(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogDebug("Get multiple Itemz request cannot be processed as required parameter of IDs is NULL");
                return BadRequest();
            }

            var itemzEntities = await _itemzRepository.GetItemzsAsync(ids);

            if (ids.Count() != itemzEntities.Count())
            {
                _logger.LogDebug("One or More Itemz are not found while processing request to get multiple items");
                return NotFound();
            }

            var itemzsToReturn = _mapper.Map<IEnumerable<GetItemzDTO>>(itemzEntities);

            _logger.LogDebug("Returning response with {NumberOfItemz} number of Itemz to the requestor", itemzEntities.Count());
            return Ok(itemzsToReturn);
        }


        /// <summary>
        /// Used for creating new multiple Itemz record in the database
        /// </summary>
        /// <param name="itemzCollection">Array of CreateItemzDTO Used for populating information in the newly created itemzs in the database</param>
        /// <returns>Collection of Newly created Itemzs property details</returns>
        /// <response code="201">Collection of Newly created Itemzs property details</response>
        [HttpPost (Name = "__POST_Create_Itemz_Collection__") ]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<GetItemzDTO>>> CreateItemzCollectionAsync(
            IEnumerable<CreateItemzDTO> itemzCollection)
        {
            var itemzEntities = _mapper.Map<IEnumerable<Entities.Itemz>>(itemzCollection);
            foreach (var itemz in itemzEntities)
            {
                _itemzRepository.AddItemz(itemz);
            }
            await _itemzRepository.SaveAsync();

            var itemzCollectionToReturn = _mapper.Map<IEnumerable<GetItemzDTO>>(itemzEntities);
            var idConvertedToString = string.Join(",", itemzCollectionToReturn.Select(a => a.Id));

            _logger.LogDebug("Created {NumberOfItemzCreated} number of new Itemz", itemzCollectionToReturn.Count());
            return CreatedAtRoute("__GET_Itemz_Collection_By_GUID_IDS__",
                new { ids = idConvertedToString }, itemzCollectionToReturn);

        }

        /// <summary>
        /// Get list of supported HTTP Options for the ItemzCollection controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions(Name = "__OPTIONS_Itemz_Collection_Controller__")]
        public IActionResult GetItemzOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

    }

}
