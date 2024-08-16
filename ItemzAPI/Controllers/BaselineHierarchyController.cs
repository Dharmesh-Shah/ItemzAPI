// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ItemzApp.API.Helper;
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

using Microsoft.CodeAnalysis;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    //[Route("api/Hierarchy")]
    [Route("api/[controller]")] // e.g. http://HOST:PORT/api/Hierarchy
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class BaselineHierarchyController : ControllerBase
    {
        private readonly IBaselineHierarchyRepository _baselineHierarchyRepository;
        private readonly ILogger<BaselineHierarchyController> _logger;

        public BaselineHierarchyController(IBaselineHierarchyRepository baselineHierarchyRepository,
                                    ILogger<BaselineHierarchyController> logger)
        {
            _baselineHierarchyRepository = baselineHierarchyRepository ?? throw new ArgumentNullException(nameof(baselineHierarchyRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets BaselineHierarchy Record details based on Record Id provided in GUID form.
        /// </summary>
        /// <param name="RecordId">GUID representing an unique ID of a BaselineHierarchy record</param>
        /// <returns>BaselineHierarchy record details containing various information about given Record Id</returns>
        /// <response code="200">BaselineHierarchy record details containing various information about given Record Id</response>
        /// <response code="404">BaselineHierarchy record not found in the repository for the given GUID ID</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaselineHierarchyIdRecordDetailsDTO))]
        [HttpGet("{RecordId:Guid}",
            Name = "__Get_BaselineHierarchy_Record_Details_By_GUID__")] // e.g. http://HOST:PORT/api/BaselineHierarchy/42f62a6c-fcda-4dac-a06c-406ac1c17770
        [HttpHead("{RecordId:Guid}", Name = "__HEAD_BaselineHierarchy_Record_Details_By_GUID__")]
        public async Task<ActionResult<BaselineHierarchyIdRecordDetailsDTO>> GetBaselineHierarchyRecordDetailsAsync(Guid RecordId)
        {
            _logger.LogDebug("{FormattedControllerAndActionNames}Processing request to get BaselineHierarchy record details for ID {ParentRecordId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                RecordId);

            var baselineHierarchyIdRecordDetailsDTO = new BaselineHierarchyIdRecordDetailsDTO();
            try 
            { 
                baselineHierarchyIdRecordDetailsDTO = await _baselineHierarchyRepository.GetBaselineHierarchyRecordDetailsByID(RecordId);
            }
            catch (ApplicationException appException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Exception occured while trying to get BaselineHierarchy Details : " + appException.Message,
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                var tempMessage = $"Could not produce BaselineHierarchy details for given Record Id {RecordId}" +
                    $" :: InnerException :: {appException.Message} ";
                return BadRequest(tempMessage);
            }

            if (baselineHierarchyIdRecordDetailsDTO != null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Returning BaselineHierarchy Record details for ID {ParentRecordId} " +
                    "with '{BaselineHierarchyId}' as BaselineHierarchyID and {RecordType} as Record Type.",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    RecordId,
                    baselineHierarchyIdRecordDetailsDTO.BaselineHierarchyId,
                    baselineHierarchyIdRecordDetailsDTO.RecordType);
            }
            return Ok(baselineHierarchyIdRecordDetailsDTO);
        }


        /// <summary>
        /// Verify that two Baseline Hierarchy IDs are part of same Breakdown Structure within a given Baseline.
        /// </summary>
        /// <param name="parentId">GUID representing an unique ID of a Parent BaselineHierarchy record</param>
        /// <param name="childId">GUID representing an unique ID of a Child BaselineHierarchy record</param>
        /// <returns>True if ParentHierarchyId and ChildHierarchyId are part of the same Breakdown Structure within a given Baseline. Otherwise False </returns>
        /// <response code="200">True or False based on outcome of verifying Breakdown Structure while looking for Parent and Child Baseline Hierarchy IDs</response>
        /// <response code="404">Invalid Id provided for either Parent or Child Baseline Hierarchy record</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [HttpGet("VerifyParentChildBreakdownStructure/",
            Name = "__Get_VerifyParentChild_BreakdownStructure__")] // e.g. http://HOST:PORT/api/BaselineHierarchy/42f62a6c-fcda-4dac-a06c-406ac1c17770
        [HttpHead("VerifyParentChildBreakdownStructure/", Name = "__HEAD_VerifyParentChild_BreakdownStructure__")]
        public async Task<ActionResult<bool>> VerifyParentChildBreakdownStructureAsync([FromQuery] Guid parentId, [FromQuery] Guid childId)
        {

            if (parentId == Guid.Empty)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames} Parent Baseline Hierarchy Breakdown Structure is an empty ID.",
                        ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext));
                return NotFound();
            }

            if (childId == Guid.Empty)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames} Child Baseline Hierarchy Breakdown Structure is an empty ID.",
                        ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext));
                return NotFound();
            }

            _logger.LogDebug("{FormattedControllerAndActionNames}Processing request to verify parent and child Baseline Hieararchy " +
                "Breakdown Structure between Parent ID {ParentId} and Child ID {ChildId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                , parentId
                , childId);

            var parentBaselineHierarchyIdRecordDetailsDTO = new BaselineHierarchyIdRecordDetailsDTO();
            try
            {
                parentBaselineHierarchyIdRecordDetailsDTO = await _baselineHierarchyRepository.GetBaselineHierarchyRecordDetailsByID(parentId);
            }
            catch (ApplicationException appException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Exception occured while trying to get Parent BaselineHierarchy Details : " + appException.Message,
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                var tempMessage = $"Could not produce Parent BaselineHierarchy details for given Id {parentId}" +
                    $" :: InnerException :: {appException.Message} ";
                return BadRequest(tempMessage);
            }

            var childBaselineHierarchyIdRecordDetailsDTO = new BaselineHierarchyIdRecordDetailsDTO();
            try
            {
                childBaselineHierarchyIdRecordDetailsDTO = await _baselineHierarchyRepository.GetBaselineHierarchyRecordDetailsByID(childId);
            }
            catch (ApplicationException appException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Exception occured while trying to get child BaselineHierarchy Details : " + appException.Message,
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                var tempMessage = $"Could not produce child BaselineHierarchy details for given Id {childId}" +
                    $" :: InnerException :: {appException.Message} ";
                return BadRequest(tempMessage);
            }

            if (parentBaselineHierarchyIdRecordDetailsDTO is null)
            {
                return BadRequest($"Baseline Hierarchy Id details could not be found for provided Parent ID {parentId}");
            }

            if (childBaselineHierarchyIdRecordDetailsDTO is null)
            { 
                return BadRequest($"Baseline Hierarchy Id details could not be found for provided Child ID {childId}");
            }

            if (parentBaselineHierarchyIdRecordDetailsDTO.Level < 2 )
            { 
                return BadRequest("Provided Parent Id is for record above Baseline Level.");
            }

            if (childBaselineHierarchyIdRecordDetailsDTO.Level < 2)
            {
                return BadRequest("Provided Child Id is for record above Baseline Level.");
            }

            return Ok(await
                _baselineHierarchyRepository.CheckIfPartOfSingleBaselineHierarchyBreakdownStructureAsync(parentId, childId)
            );

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
    }
}
