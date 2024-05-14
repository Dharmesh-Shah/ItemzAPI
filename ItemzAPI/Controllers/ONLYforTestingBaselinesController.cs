// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ItemzApp.API.Models;
using ItemzApp.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ItemzApp.API.BusinessRules.Baseline;
using ItemzApp.API.Helper;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")] // e.g. http://HOST:PORT/api/ONLYforTestingBaselines
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ONLYforTestingBaselinesController : ControllerBase
    {
        private readonly IBaselineRepository _baselineRepository;
        private readonly ILogger<ONLYforTestingBaselinesController> _logger;
        public ONLYforTestingBaselinesController( IBaselineRepository baselineRepository,
                                 ILogger<ONLYforTestingBaselinesController> logger
            )
        {
            _baselineRepository = baselineRepository ?? throw new ArgumentNullException(nameof(baselineRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Deleting a specific Baseline leaving Orphaned BaselineItemz behind
        /// </summary>
        /// <param name="baselineId">GUID representing an unique ID of the Baseline that you want to get</param>
        /// <returns>Status code 204 is returned without any content indicating that deletion of the specified Baseline was successful</returns>
        /// <response code="404">Baseline based on baselineId was not found</response>
        [HttpDelete("{baselineId}", Name = "__DELETE_ONLY_FOR_TESTING_Baseline_By_GUID_ID__")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteBaselineAsync(Guid baselineId)
        {
            if (!(await _baselineRepository.BaselineExistsAsync(baselineId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Cannot Delete Baseline with ID {BaselineId} as it could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    baselineId);
                return NotFound();
            }

            var baselineFromRepo = await _baselineRepository.GetBaselineForUpdateAsync(baselineId);

            if (baselineFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Cannot Delete Baseline with ID {BaselineId} as it could not be found in the Repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    baselineId);
                return NotFound();
            }

            _baselineRepository.DeleteBaseline(baselineFromRepo);
            await _baselineRepository.SaveAsync();

            _logger.LogDebug("{FormattedControllerAndActionNames}Delete request for Baseline with ID {BaselineId} processed successfully",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                baselineId);

            // await _baselineRepository.DeleteOrphanedBaselineItemzAsync(); commented this line to make sure that we don't call DeleteOrphanedBaselineItemz
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
        /// Get list of supported HTTP Options for the Baselines controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions(Name = "__OPTIONS_for_ONLY_FOR_TESTING_Baselines_Controller__")]
        public IActionResult GetBaselinesOptions()
        {
            Response.Headers.Add("Allow", "DELETE,OPTIONS");
            return Ok();
        }
    }
}
