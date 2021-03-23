using ItemzApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemzApp.API.Models;
using AutoMapper;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    [Route("api/ItemzChangeHistory")] // e.g. http://HOST:PORT/api/itemzs
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ItemzChangeHistoryController : ControllerBase
    {
        private readonly IItemzChangeHistoryRepository _itemzChangeHistoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemzChangeHistoryController> _logger;

        public ItemzChangeHistoryController(IItemzChangeHistoryRepository itemzChangeHistoryRepository,
            IMapper mapper,
            ILogger<ItemzChangeHistoryController> logger)
        {
            _itemzChangeHistoryRepository = itemzChangeHistoryRepository ?? throw new ArgumentNullException(nameof(itemzChangeHistoryRepository)); ;
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get collection of Itemz Change History by Itemz ID (represented by a GUID)
        /// </summary>
        /// <param name="ItemzId">GUID representing an unique ID of the Itemz that you want to get change history for</param>
        /// <returns>A collection of Itemz Change History records based on provided Itemz ID (GUID) </returns>
        /// <response code="200">Returns the requested Itemz Change History Records</response>
        /// <response code="404">Requested Itemz and/or it's change history records not found</response>
        /// 

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetItemzChangeHistoryDTO>))]
        [HttpGet("{ItemzId:Guid}",
            Name = "__GET_ItemzChangeHistory_By_GUID_ItemzID__")] // e.g. http://HOST:PORT/api/ItemzChangeHistory/9153a516-d69e-4364-b17e-03b87442e21c
        [HttpHead("{ItemzId:Guid}", Name = "__HEAD_ItemzChangeHistory_By_GUID_ItemzID__")]
        public async Task<ActionResult<IEnumerable<GetItemzChangeHistoryDTO>>> GetItemzChangeHistoryAsync(Guid ItemzId)
        {
            _logger.LogDebug("Processing request to get ItemzChangeHistory for ID {ItemzId}", ItemzId);
            var itemzChangeHistoryFromRepo = await _itemzChangeHistoryRepository.GetItemzChangeHistoryAsync(ItemzId);

            if (itemzChangeHistoryFromRepo == null)
            {
                _logger.LogDebug("ItemzChangeHistory for ID {ItemzId} could not be found", ItemzId);
                return NotFound();
            }
            _logger.LogDebug("Found ItemzChangeHistory for ID {ItemzId} and now returning results", ItemzId);
            return Ok(_mapper.Map<IEnumerable<GetItemzChangeHistoryDTO>>(itemzChangeHistoryFromRepo)); 
        }
    }
}
