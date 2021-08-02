// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ItemzApp.API.Helper;
using ItemzApp.API.Models;
using ItemzApp.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    //[Route("api/BaselineItemz")]
    [Route("api/[controller]")] // e.g. http://HOST:PORT/api/BaselineItemz
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class BaselineItemzController : ControllerBase
    {
        private readonly IBaselineItemzRepository _baselineItemzRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BaselineItemzController> _logger;

        public BaselineItemzController(IBaselineItemzRepository baselineItemzRepository,
                                    IMapper mapper,
                                     ILogger<BaselineItemzController> logger
                                    )
        {
            _baselineItemzRepository = baselineItemzRepository ?? throw new ArgumentNullException(nameof(baselineItemzRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get a BaselineItemz by ID (represented by a GUID)
        /// </summary>
        /// <param name="BaselineItemzId">GUID representing an unique ID of the BaselineItemz that you want to get</param>
        /// <returns>A single BaselineItemz record based on provided ID (GUID) </returns>
        /// <response code="200">Returns the requested BaselineItemz</response>
        /// <response code="404">Requested BaselineItemz not found</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetBaselineItemzDTO))]
        [HttpGet("{BaselineItemzId:Guid}",
            Name = "__Single_BaselineItemz_By_GUID_ID__")] // e.g. http://HOST:PORT/api/BaselineItemz/42f62a6c-fcda-4dac-a06c-406ac1c17770
        [HttpHead("{BaselineItemzId:Guid}", Name = "__HEAD_BaselineItemz_By_GUID_ID__")]
        public async Task<ActionResult<GetBaselineItemzDTO>> GetBaselineItemzAsync(Guid BaselineItemzId)
        {
            _logger.LogDebug("{FormattedControllerAndActionNames}Processing request to get BaselineItemz for ID {BaselineItemzId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                BaselineItemzId);
            var BaselineItemzFromRepo = await _baselineItemzRepository.GetBaselineItemzAsync(BaselineItemzId);

            if (BaselineItemzFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}BaselineItemz for ID {BaselineItemzId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    BaselineItemzId);
                return NotFound();
            }
            _logger.LogDebug("{FormattedControllerAndActionNames}Found BaselineItemz for ID {BaselineItemzId} and now returning results",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                BaselineItemzId);
            return Ok(_mapper.Map<GetBaselineItemzDTO>(BaselineItemzFromRepo));
        }

        /// <summary>
        /// Gets collection of BaselineItemzs for the given ItemzID
        /// </summary>
        /// <returns>Collection of BaselineItemzs based on given ItemzID</returns>
        /// <response code="200">Returns collection of BaselineItemzs based on given ItemzID</response>
        /// <response code="404">No BaselineItemzs were found for the given ItemzID</response>

        [HttpGet("GetBaselineItemzs/{ItemzId:Guid}", Name = "__GET_BaselineItemzs_By_Itemz__")] // e.g. http://HOST:PORT/api/BaselineItemz/GetBaselineItemzs/42f62a6c-fcda-4dac-a06c-406ac1c17770
        [HttpHead("GetBaselineItemzs/{ItemzId:Guid}", Name = "__HEAD_BaselineItemzs_By_Itemz__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<GetBaselineItemzDTO>>> GetBaselineItemzsByItemzIdAsync(Guid ItemzId)
        {
            var baselineItemzsFromRepo = await _baselineItemzRepository.GetBaselineItemzByItemzIdAsync(ItemzId);
            if (baselineItemzsFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}No BaselineItemzs found for ItemzID {ItemzId}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    ItemzId);
                return NotFound();
            }

            _logger.LogDebug("{FormattedControllerAndActionNames}Returning results for {BaselineItemzNumbers} BaselineItemzs based on ItemzID {ItemzId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                baselineItemzsFromRepo.Count(),
                ItemzId);
            return Ok(_mapper.Map<IEnumerable<GetBaselineItemzDTO>>(baselineItemzsFromRepo));
        }

        /// <summary>
        /// Get total number of BaselineItemz by ItemzId
        /// </summary>
        /// <param name="itemzId">Provide ItemzId representated in GUID form</param>
        /// <returns>Number of BaselineItemz found for the given ItemzId. Zero if none found.</returns>
        /// <response code="200">Returns number of BaselineItemz count that were associated with a given ItemzId</response>
        [HttpGet("GetBaselineItemzCount/{ItemzId:Guid}", Name = "__GET_BaselineItemz_Count_By_ItemzId__")]
        [HttpHead("GetBaselineItemzCount/{ItemzId:Guid}", Name = "__HEAD_BaselineItemz_Count_By_ItemzId__")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> GetBaselineItemzCountByItemzIdAsync(Guid itemzId)
        {
            var foundBaselineItemzCountByItemzId = await _baselineItemzRepository.GetBaselineItemzCountByItemzIdAsync(itemzId);
            _logger.LogDebug("{FormattedControllerAndActionNames} Found {foundBaselineItemzCountByItemzId} BaselineItemz records for Itemz with ID {itemzId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                foundBaselineItemzCountByItemzId,
                itemzId);
            return foundBaselineItemzCountByItemzId;
        }

        /// <summary>
        /// Gets collection of BaselineItemzs
        /// </summary>
        /// <param name="baselineItemzids">Array of BaselineItemz Id (in GUID form) for which details has to be returned to the caller</param>
        /// <returns>Collection of BaselineItemz that are requested via Array of BaselineItemz Id</returns>
        /// <response code="200">Collection of BaselineItemzs property details based on BaselineItemz Ids that were passed in as parameter</response>
        /// <response code="500">Bad Request - BaselineItemz Ids should be passed in as parameter</response>
        /// <response code="404">No BaselineItemzs were found based on provided list of BaselineItemz Ids</response>
        /// <remarks>
        /// Sample request (this request will get BaselineItemz by Ids) \
        /// GET api/BaselineItemz/(9153a516-d69e-4364-b17e-03b87442e21c,5e76f8e8-d3e7-41db-b084-f64c107c6783) 
        /// </remarks>
        [HttpGet("({baselineItemzids})", Name = "__GET_BaselineItemz_Collection_By_GUID_IDS__")]
        [HttpHead("({baselineItemzids})", Name = "__HEAD_BaselineItemz_Collection_By_GUID_IDS__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<GetBaselineItemzDTO>>> GetBaselineItemzCollectionAsync(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> baselineItemzids)
        {
            if (baselineItemzids == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Get multiple BaselineItemz request cannot be processed as required parameter of IDs is NULL",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return BadRequest();
            }

            var baselineItemzEntities = await _baselineItemzRepository.GetBaselineItemzsAsync(baselineItemzids);

            if (baselineItemzids.Count() != baselineItemzEntities.Count())
            {
                // TODO: We should identify which baselineItemzids were not found in the repository
                // rather then just returning generinc response saying "One or More BaselineItemz are not found ..."

                _logger.LogDebug("{FormattedControllerAndActionNames}One or More BaselineItemz are not found while processing request to get multiple items",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return NotFound();
            }

            var baselineItemzsToReturn = _mapper.Map<IEnumerable<GetBaselineItemzDTO>>(baselineItemzEntities);

            _logger.LogDebug("{FormattedControllerAndActionNames}Returning response with {NumberOfBaselineItemz} number of BaselineItemz to the requestor",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                baselineItemzEntities.Count());
            return Ok(baselineItemzsToReturn);
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
        /// Get list of supported HTTP Options for the BaselineItemz controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions(Name = "__OPTIONS_for_BaselineItemz_Controller__")]
        public IActionResult GetBaselineItemzsOptions()
        {
            Response.Headers.Add("Allow","GET,HEAD,OPTIONS");
            return Ok();
        }
    }
}
