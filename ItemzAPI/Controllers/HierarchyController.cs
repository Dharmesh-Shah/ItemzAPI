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
    public class HierarchyController : ControllerBase
    {
        private readonly IHierarchyRepository _hierarchyRepository;
        private readonly ILogger<HierarchyController> _logger;

        public HierarchyController(IHierarchyRepository hierarchyRepository,
                                    ILogger<HierarchyController> logger)
        {
            _hierarchyRepository = hierarchyRepository ?? throw new ArgumentNullException(nameof(hierarchyRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets Hierarchy Record details based on Record Id provided in GUID form.
        /// </summary>
        /// <param name="RecordId">GUID representing an unique ID of a hierarchy record</param>
        /// <returns>Hierarchy record details containing various information about given Record Id</returns>
        /// <response code="200">Hierarchy record details containing various information about given Record Id</response>
        /// <response code="404">Hierarchy record not found in the repository for the given GUID ID</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HierarchyIdRecordDetailsDTO))]
        [HttpGet("{RecordId:Guid}",
            Name = "__Get_Hierarchy_Record_Details_By_GUID__")] // e.g. http://HOST:PORT/api/Hierarchy/42f62a6c-fcda-4dac-a06c-406ac1c17770
        [HttpHead("{RecordId:Guid}", Name = "__HEAD_Hierarchy_Record_Details_By_GUID__")]
        public async Task<ActionResult<HierarchyIdRecordDetailsDTO>> GetHierarchyRecordDetailsAsync(Guid RecordId)
        {
            _logger.LogDebug("{FormattedControllerAndActionNames}Processing request to get Hierarchy record details for ID {ParentRecordId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                RecordId);

            var hierarchyIdRecordDetailsDTO = new HierarchyIdRecordDetailsDTO();
            try 
            { 
                hierarchyIdRecordDetailsDTO = await _hierarchyRepository.GetHierarchyRecordDetailsByID(RecordId);
            }
            catch (ApplicationException appException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Exception occured while trying to get Hierarchy Details : " + appException.Message,
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                var tempMessage = $"Could not produce hierarchy details for given Record Id {RecordId}" +
                    $" :: InnerException :: {appException.Message} ";
                return BadRequest(tempMessage);
            }

            if (hierarchyIdRecordDetailsDTO != null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Returning Hierarchy Record details for ID {ParentRecordId} " +
                    "with '{HierarchyId}' as HierarchyID and {RecordType} as Record Type.",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    RecordId,
                    hierarchyIdRecordDetailsDTO.HierarchyId,
                    hierarchyIdRecordDetailsDTO.RecordType);
            }
            return Ok(hierarchyIdRecordDetailsDTO);
        }

		/// <summary>
		/// Gets Hierarchy Records of immediate children under Record Id provided in GUID form.
		/// </summary>
		/// <param name="RecordId">GUID representing an unique ID of a hierarchy record</param>
		/// <returns>Collection of Immediate children Hierarchy record details </returns>
		/// <response code="200">Immediate children Hierarchy record details </response>
		/// <response code="400">Bad Request</response>
		/// <response code="404">Immediate children Hierarchy record(s) not found in the repository for the given GUID ID</response>
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HierarchyIdRecordDetailsDTO))]
		[HttpGet("GetImmediateChildren/{RecordId:Guid}"
            , Name = "__Get_Immediate_Children_Hierarchy_By_GUID__")] // e.g. http://HOST:PORT/api/Hierarchy/GetImmediateChildren/42f62a6c-fcda-4dac-a06c-406ac1c17770
		[HttpHead("GetImmediateChildren/{RecordId:Guid}", Name = "__HEAD_Immediate_Children_Hierarchy_By_GUID__")]
		[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<IEnumerable<HierarchyIdRecordDetailsDTO>>> GetImmediateChildrenOfItemzHierarchy(Guid RecordId)
		{
			_logger.LogDebug("{FormattedControllerAndActionNames}Processing request to get Immediate Children Hierarchy records for ID {ParentRecordId}",
				ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
				RecordId);

			IEnumerable<HierarchyIdRecordDetailsDTO?> immediateChildrenhierarchyRecords = [];
			try
			{
				immediateChildrenhierarchyRecords = await _hierarchyRepository.GetImmediateChildrenOfItemzHierarchy(RecordId);
			}
			catch (ApplicationException appException)
			{
				_logger.LogDebug("{FormattedControllerAndActionNames}Exception occured while trying to get Immediate Children Hierarchy records : " + appException.Message,
					ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
					);
				var tempMessage = $"Could not produce get immediate children hierarchy records for given Record Id {RecordId}" +
					$" :: InnerException :: {appException.Message} ";
				return BadRequest(tempMessage);
			}

			if (immediateChildrenhierarchyRecords.FirstOrDefault()!.RecordId != Guid.Empty)
			{
				_logger.LogDebug("{FormattedControllerAndActionNames} Returning {hirarchyChildRecordCount} Immediate Children Hierarchy Records for ID {RecordId} ",
					ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
					immediateChildrenhierarchyRecords.Count(),
					RecordId );
			}
            else
            {
                return Ok();
            }
			return Ok(immediateChildrenhierarchyRecords);

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
