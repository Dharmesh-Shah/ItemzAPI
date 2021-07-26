// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
using ItemzApp.API.BusinessRules.Baseline;
using ItemzApp.API.Helper;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    //[Route("api/Baselines")]
    [Route("api/[controller]")] // e.g. http://HOST:PORT/api/itemzs/Baselines
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class BaselinesController : ControllerBase
    {
        private readonly IBaselineRepository _baselineRepository;
        private readonly IMapper _mapper;
        // private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILogger<BaselinesController> _logger;
        private readonly IBaselineRules _baselineRules;
        public BaselinesController( IBaselineRepository baselineRepository,
                                 IMapper mapper,
                                 //IPropertyMappingService propertyMappingService,
                                 ILogger<BaselinesController> logger,
                                 IBaselineRules baselineRules
            )
        {
            _baselineRepository = baselineRepository ?? throw new ArgumentNullException(nameof(baselineRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            //_propertyMappingService = propertyMappingService ??
            //    throw new ArgumentNullException(nameof(propertyMappingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _baselineRules = baselineRules ?? throw new ArgumentNullException(nameof(baselineRules));


        }

        /// <summary>
        /// Get a Baseline by ID (represented by a GUID)
        /// </summary>
        /// <param name="BaselineId">GUID representing an unique ID of the Baseline that you want to get</param>
        /// <returns>A single Baseline record based on provided ID (GUID) </returns>
        /// <response code="200">Returns the requested Baseline</response>
        /// <response code="404">Requested Baseline not found</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetBaselineDTO))]
        [HttpGet("{BaselineId:Guid}",
            Name = "__Single_Baseline_By_GUID_ID__")] // e.g. http://HOST:PORT/api/Baselines/42f62a6c-fcda-4dac-a06c-406ac1c17770
        [HttpHead("{BaselineId:Guid}", Name = "__HEAD_Baseline_By_GUID_ID__")]
        public async Task<ActionResult<GetBaselineDTO>> GetBaselineAsync(Guid BaselineId)
        {
            _logger.LogDebug("{FormattedControllerAndActionNames}Processing request to get Baseline for ID {BaselineId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                BaselineId);
            var baselineFromRepo = await _baselineRepository.GetBaselineAsync(BaselineId);

            if (baselineFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Baseline for ID {BaselineId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    BaselineId);
                return NotFound();
            }
            _logger.LogDebug("{FormattedControllerAndActionNames}Found Baseline for ID {BaselineId} and now returning results",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                BaselineId);
            return Ok(_mapper.Map<GetBaselineDTO>(baselineFromRepo));
        }


        /// <summary>
        /// Gets collection of Baselines
        /// </summary>
        /// <returns>Collection of Baselines based on expectated sorting order.</returns>
        /// <response code="200">Returns collection of Baselines based on sorting order</response>
        /// <response code="404">No Baselines were found</response>
        [HttpGet(Name = "__GET_Baselines__")]
        [HttpHead(Name = "__HEAD_Baselines_Collection__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<GetBaselineDTO>>> GetBaselinesAsync()
        {
            var baselinesFromRepo = await _baselineRepository.GetBaselinesAsync();
            if (baselinesFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}No Baselines found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return NotFound();
            }

            _logger.LogDebug("{FormattedControllerAndActionNames}Returning results for {BaselineNumbers} Baselines to the caller",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                baselinesFromRepo.Count());
            return Ok(_mapper.Map<IEnumerable<GetBaselineDTO>>(baselinesFromRepo));
        }







        /// <summary>
        /// Used for creating new Baseline record in the database
        /// </summary>
        /// <param name="createBaselineDTO">Used for populating information in the newly created Baseline in the database</param>
        /// <returns>Newly created Baseline property details</returns>
        /// <response code="201">Returns newly created Baselines property details</response>
        /// <response code="404">Expected Project with ID was not found in the repository</response>
        /// <response code="409">Baseline with the same name already exists in the repository</response>

        [HttpPost(Name = "__POST_Create_Baseline__")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetBaselineDTO>> CreateBaselineAsync(CreateBaselineDTO createBaselineDTO)
        {
            if (!(await _baselineRepository.ProjectExistsAsync(createBaselineDTO.ProjectId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Project with {ProjectId} could not be found while creating new Baseline in the repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    createBaselineDTO.ProjectId);
                return NotFound();
            }

            var baselineEntity = _mapper.Map<Entities.Baseline>(createBaselineDTO);

            if (await _baselineRules.UniqueBaselineNameRuleAsync(createBaselineDTO.ProjectId, createBaselineDTO.Name!))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Baseline with name {createBaselineDTO_Name} already exists in the project with Id {createBaselineDTO_ProjectId}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    createBaselineDTO.Name,
                    createBaselineDTO.ProjectId);
                return Conflict($"Baseline with name '{createBaselineDTO.Name}' already exists in the project with Id '{createBaselineDTO.ProjectId}'");

            }

            try
            {
                baselineEntity.Id = await _baselineRepository.AddBaselineAsync(baselineEntity);
                // await _baselineRepository.SaveAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Exception Occured while trying to add new baseline:" + dbUpdateException.InnerException,
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return Conflict($"Baseline with name '{baselineEntity.Name}' already exists in the repository");
            }
            _logger.LogDebug("{FormattedControllerAndActionNames}Created new Baseline with ID {BaselineId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                baselineEntity.Id);
            return CreatedAtRoute("__Single_Baseline_By_GUID_ID__", new { BaselineId = baselineEntity.Id },
                _mapper.Map<GetBaselineDTO>(baselineEntity) // Converting to DTO as this is going out to the consumer
                );
        }

        /// <summary>
        /// Updating exsting Baseline based on Baseline Id (GUID)
        /// </summary>
        /// <param name="baselineId">GUID representing an unique ID of the Baseline that you want to get</param>
        /// <param name="baselineToBeUpdated">required Baseline properties to be updated</param>
        /// <returns>No contents are returned but only Status 204 indicating that Baseline was updated successfully </returns>
        /// <response code="204">No content are returned but status of 204 indicated that Baseline was successfully updated</response>
        /// <response code="404">Baseline based on baselineId was not found</response>
        /// <response code="409">Baseline with updated name already exists in the repository</response>

        [HttpPut("{baselineId}", Name = "__PUT_Update_Baseline_By_GUID_ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateBaselinePutAsync(Guid baselineId, UpdateBaselineDTO baselineToBeUpdated)
        {
            if (!(await _baselineRepository.BaselineExistsAsync(baselineId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Baseline for ID {BaselineId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                    baselineId);
                return NotFound();
            }

            var baselineFromRepo = await _baselineRepository.GetBaselineForUpdateAsync(baselineId);

            if (baselineFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Baseline for ID {BaselineId} could not be found in the Repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                    baselineId);
                return NotFound();
            }

            if (await _baselineRules.UniqueBaselineNameRuleAsync(baselineFromRepo.ProjectId, baselineToBeUpdated.Name!, baselineFromRepo.Name))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Baseline with name {baselineToBeUpdated_Name} already exists in the project with Id {ItemzTypeFromRepo_ProjectId}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    baselineToBeUpdated.Name);
                return Conflict($"Baseline with name '{baselineToBeUpdated.Name}' already exists in the project with Id '{baselineFromRepo.ProjectId}'");
            }

            _mapper.Map(baselineToBeUpdated, baselineFromRepo);
            try 
            { 
            _baselineRepository.UpdateBaseline(baselineFromRepo);
            await _baselineRepository.SaveAsync();

            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Exception Occured while trying to add new baseline:" + dbUpdateException.InnerException,
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return Conflict($"Baseline with name '{baselineToBeUpdated.Name}' already exists in the project with Id '{baselineFromRepo.ProjectId}'");
            }
            _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Baseline for ID {BaselineId} processed successfully",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                baselineId);
            return NoContent(); // This indicates that update was successfully saved in the DB.
        }

        /// <summary>
        /// Partially updating a single **Baseline**
        /// </summary>
        /// <param name="baselineId">Id of the Baseline representated by a GUID.</param>
        /// <param name="baselinePatchDocument">The set of operations to apply to the Baseline via JsonPatchDocument</param>
        /// <returns>an ActionResult of type Baseline</returns>
        /// <response code="204">No content are returned but status of 204 indicated that Baseline was successfully updated</response>
        /// <response code="404">Baseline based on baselineId was not found</response>
        /// <response code="409">Baseline with updated name already exists in the repository</response>
        /// <response code="422">Validation problems occured during analyzing validation rules for the JsonPatchDocument </response>
        /// <remarks> Sample request (this request updates an **Baseline's name**)   
        /// Documentation regarding JSON Patch can be found at 
        /// *[ASP.NET Core - JSON Patch Operations](https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.1#operations)* 
        /// 
        ///     PATCH /api/Baselines/{id}  
        ///     [  
        ///	        {   
        ///             "op": "replace",   
        ///             "path": "/name",   
        ///             "value": "PATCH Updated Name field"  
        ///	        }   
        ///     ]
        /// </remarks>

        [HttpPatch("{baselineId}", Name = "__PATCH_Update_Baseline_By_GUID_ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateBaselinePatchAsync(Guid baselineId, JsonPatchDocument<UpdateBaselineDTO> baselinePatchDocument)
        {
            if (!(await _baselineRepository.BaselineExistsAsync(baselineId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Baseline for ID {BaselineId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                    baselineId);
                return NotFound();
            }

            var baselineFromRepo = await _baselineRepository.GetBaselineForUpdateAsync(baselineId);

            if (baselineFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Baseline for ID {BaselineId} could not be found in the Repository",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                    baselineId);
                return NotFound();
            }

            var baselineToPatch = _mapper.Map<UpdateBaselineDTO>(baselineFromRepo);

            baselinePatchDocument.ApplyTo(baselineToPatch, ModelState);

            // Validating Baseline patch document and verifying that it meets all the 
            // validation rules as expected. This will check if the data passed in the Patch Document
            // is ready to be saved in the db.

            if (!TryValidateModel(baselineToPatch))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Baseline Properties did not pass defined Validation Rules for ID {BaselineId}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                    baselineId);
                return ValidationProblem(ModelState);
            }
            if (await _baselineRules.UniqueBaselineNameRuleAsync(baselineFromRepo.ProjectId, baselineToPatch.Name!, baselineFromRepo.Name))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Baseline with name {baselineToPatch_Name} already exists in the project with Id {BaselineFromRepo_ProjectId}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    baselineToPatch.Name);
                return Conflict($"Baseline with name '{baselineToPatch.Name}' already exists in the project with Id '{baselineFromRepo.ProjectId}'");
            }

            _mapper.Map(baselineToPatch, baselineFromRepo);
            try
            {
                _baselineRepository.UpdateBaseline(baselineFromRepo);
                await _baselineRepository.SaveAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Exception Occured while trying to add new baseline:" + dbUpdateException.InnerException,
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return Conflict($"Baseline with name '{baselineToPatch.Name}' already exists in the project with Id '{baselineFromRepo.ProjectId}'");
            }

            _logger.LogDebug("{FormattedControllerAndActionNames}Update request for Baseline for ID {BaselineId} processed successfully",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                baselineId);
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
        /// Deleting a specific Baseline
        /// </summary>
        /// <param name="baselineId">GUID representing an unique ID of the Baseline that you want to get</param>
        /// <returns>Status code 204 is returned without any content indicating that deletion of the specified Baseline was successful</returns>
        /// <response code="404">Baseline based on baselineId was not found</response>
        [HttpDelete("{baselineId}", Name = "__DELETE_Baseline_By_GUID_ID__")]
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

            _logger.LogDebug("{FormattedControllerAndActionNames}Delete request for Projeect with ID {BaselineId} processed successfully",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                baselineId);

            await _baselineRepository.DeleteOrphanedBaselineItemzAsync();
            return NoContent();
        }

        /// <summary>
        /// Get total number of BaselineItemz by Baseline
        /// </summary>
        /// <param name="baselineId">Provide BaselineID representated in GUID form</param>
        /// <returns>Number of BaselineItemz found for the given BaselineID. Zero if none found.</returns>
        /// <response code="200">Returns number of BaselineItemz count that were associated with a given Baseline</response>
        /// <response code="404">Baseline based on baselineId was not found</response>
        [HttpGet("GetBaselineItemzCount/{BaselineId:Guid}", Name = "__GET_BaselineItemz_Count_By_Baseline__")]
        [HttpHead("GetBaselineItemzCount/{BaselineId:Guid}", Name = "__HEAD_BaselineItemz_Count_By_Baseline__")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int>> GetBaselineItemzCountByBaselineAsync(Guid baselineId)
        {
            if (!(await _baselineRepository.BaselineExistsAsync(baselineId)))
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Cannot find count of BaselineItemz as Baseline with ID {BaselineId} could not be found",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    baselineId);
                return NotFound();
            }

            var foundItemzCountByBaselineId = await _baselineRepository.GetBaselineItemzCountByBaselineAsync(baselineId);
            _logger.LogDebug("{FormattedControllerAndActionNames} Found {foundItemzCountByBaselineId} Itemz records for Baseline with ID {baselineId}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                foundItemzCountByBaselineId,
                baselineId);
            return foundItemzCountByBaselineId;
        }

        /// <summary>
        /// Gets collection of BaselineItemzTypes for the given BaselineID
        /// </summary>
        /// <returns>Collection of BaselineItemzTypes based on sorting order for the given BaselineID</returns>
        /// <response code="200">Returns collection of BaselineItemzTypes based on sorting order for the given BaselineID</response>
        /// <response code="404">No BaselineItemzTypes were found for the given BaselineID</response>

        [HttpGet("GetBaselineItemzTypes/{BaselineId:Guid}", Name = "__GET_BaselineItemzTypes_By_Baseline__")] // e.g. http://HOST:PORT/api/Baselines/GetItemzTypes/42f62a6c-fcda-4dac-a06c-406ac1c17770

        [HttpHead("GetBaselineItemzTypes/{BaselineId:Guid}", Name = "__HEAD_BaselineItemzTypes_By_Baseline__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<GetBaselineItemzTypeDTO>>> GetBaselineItemzTypesByBaselineIdAsync(Guid BaselineId)
        {
            var baselineItemzTypesFromRepo = await _baselineRepository.GetBaselineItemzTypesAsync(BaselineId);
            if (baselineItemzTypesFromRepo == null)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}No BaselineItemzTypes found for BaselineID {BaselineID}",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                    BaselineId);
                return NotFound();
            }

            _logger.LogDebug("{FormattedControllerAndActionNames}Returning results for {BaselineItemzTypeNumbers} BaselineItemzTypes based on BaselineID {BaselineID}",
                ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext),
                baselineItemzTypesFromRepo.Count(),
                BaselineId);
            return Ok(_mapper.Map<IEnumerable<GetBaselineItemzTypeDTO>>(baselineItemzTypesFromRepo));
        }



        /// <summary>
        /// Get list of supported HTTP Options for the Baselines controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions(Name = "__OPTIONS_for_Baselines_Controller__")]
        public IActionResult GetBaselinesOptions()
        {
            Response.Headers.Add("Allow", "GET,HEAD,OPTIONS,POST,PUT,PATCH,DELETE");
            return Ok();
        }
    }
}
