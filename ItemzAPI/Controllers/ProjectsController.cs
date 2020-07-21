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
using ItemzApp.API.BusinessRules.Project;

namespace ItemzApp.API.Controllers
{
    [ApiController]
    //[Route("api/Project")]
    [Route("api/[controller]")] // e.g. http://HOST:PORT/api/itemzs/Projects
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        // private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILogger<ProjectsController> _logger;
        private readonly IProjectRules _projectRules;
        public ProjectsController(IProjectRepository projectRepository,
                                 IMapper mapper,
                                 //IPropertyMappingService propertyMappingService,
                                 ILogger<ProjectsController> logger,
                                 IProjectRules projectRules)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            //_propertyMappingService = propertyMappingService ??
            //    throw new ArgumentNullException(nameof(propertyMappingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _projectRules = projectRules ?? throw new ArgumentNullException(nameof(projectRules));


        }

        /// <summary>
        /// Get a Project by ID (represented by a GUID)
        /// </summary>
        /// <param name="ProjectId">GUID representing an unique ID of the Project that you want to get</param>
        /// <returns>A single Project record based on provided ID (GUID) </returns>
        /// <response code="200">Returns the requested Project</response>
        /// <response code="404">Requested Project not found</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProjectDTO))]
        [HttpGet("{ProjectId:Guid}",
            Name = "__Single_Project_By_GUID_ID__")] // e.g. http://HOST:PORT/api/Projects/42f62a6c-fcda-4dac-a06c-406ac1c17770
        [HttpHead("{ProjectId:Guid}", Name = "__HEAD_Project_By_GUID_ID__")]
        public ActionResult<GetProjectDTO> GetProject(Guid ProjectId)
        {
            _logger.LogDebug("Processing request to get Project for ID {ProjectId}", ProjectId);
            var projectFromRepo = _projectRepository.GetProject(ProjectId);

            if (projectFromRepo == null)
            {
                _logger.LogDebug("Project for ID {ProjectId} could not be found", ProjectId);
                return NotFound();
            }
            _logger.LogDebug("Found Project for ID {ProjectId} and now returning results", ProjectId);
            return Ok(_mapper.Map<GetProjectDTO>(projectFromRepo));
        }



        /// <summary>
        /// Gets collection of Projects
        /// </summary>
        /// <returns>Collection of Projects based on expectated sorting order.</returns>
        /// <response code="200">Returns collection of Projects based on sorting order</response>
        /// <response code="404">No Projects were found</response>
        [HttpGet(Name = "__GET_Projects__")]
        [HttpHead(Name = "__HEAD_Projects_Collection__")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<GetProjectDTO>> GetProjects(
            //[FromQuery] ItemzResourceParameter itemzResourceParameter
            )
        {
            //if (!_propertyMappingService.ValidMappingExistsFor<GetItemzDTO, Itemz>
            //    (itemzResourceParameter.OrderBy))
            //{
            //    _logger.LogWarning("Requested Order By Field {OrderByFieldName} is not found. Property Validation Failed!", itemzResourceParameter.OrderBy);
            //    return BadRequest();
            //}

            var projectsFromRepo = _projectRepository.GetProjects();
            if (projectsFromRepo == null)
            {
                _logger.LogDebug("No Projects found");
                return NotFound();
            }
            // _logger.LogDebug("In total {ProjecftsNumbers} Itemz found in the repository", projectsFromRepo.Count());
            //var previousPageLink = projectsFromRepo.HasPrevious ?
            //    CreateItemzResourceUri(itemzResourceParameter,
            //    ResourceUriType.PreviousPage) : null;

            //var nextPageLink = projectsFromRepo.HasNext ?
            //    CreateItemzResourceUri(itemzResourceParameter,
            //    ResourceUriType.NextPage) : null;

            //var paginationMetadata = new
            //{
            //    totalCount = projectsFromRepo.TotalCount,
            //    pageSize = projectsFromRepo.PageSize,
            //    currentPage = projectsFromRepo.CurrentPage,
            //    totalPages = projectsFromRepo.TotalPages,
            //    previousPageLink,
            //    nextPageLink
            //};

            // EXPLANATION : it's possible to send customer headers in the response.
            // So, before we hit 'return Ok...' statement, we can build our
            // own response header as you can see in following example.

            // TODO: Check if just passsing the header is good enough. How can we
            // document it so that consumers can use it effectively. Also, 
            // how to implement versioning of headers so that we don't break
            // existing applications using the headers after performing upgrade
            // in the future.

            //Response.Headers.Add("X-Pagination",
            //    JsonConvert.SerializeObject(paginationMetadata));

            _logger.LogDebug("Returning results for {ProjectNumbers} Projects to the caller", projectsFromRepo.Count());
            return Ok(_mapper.Map<IEnumerable<GetProjectDTO>>(projectsFromRepo));
        }



        /// <summary>
        /// Used for creating new Project record in the database
        /// </summary>
        /// <param name="createProjectDTO">Used for populating information in the newly created Project in the database</param>
        /// <returns>Newly created Project property details</returns>
        /// <response code="201">Returns newly created Projects property details</response>
        /// <response code="409">Project with the same name already exists in the repository</response>

        [HttpPost(Name = "__POST_Create_Project__")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public ActionResult<GetProjectDTO> CreateProject(CreateProjectDTO createProjectDTO)
        {
            var projectEntity = _mapper.Map<Entities.Project>(createProjectDTO);

            if (_projectRules.UniqueProjectNameRule(createProjectDTO.Name))
            {
                return Conflict($"Project with name '{createProjectDTO.Name}' already exists in the repository");

            }
            //if ((_projectRepository.HasProjectWithName(createProjectDTO.Name.Trim().ToLower())))
            //{
            //    _logger.LogDebug("Project with name \"{projectEntityName}\" already exists in the repository", projectEntity.Name);
            //    return Conflict($"Project with name '{projectEntity.Name}' already exists in the repository");
            //}

            try
            {
                _projectRepository.AddProject(projectEntity);
                _projectRepository.Save();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
            {
                _logger.LogDebug("Exception Occured while trying to add new project:" + dbUpdateException.InnerException);
                return Conflict($"Project with name '{projectEntity.Name}' already exists in the repository");
            }
            _logger.LogDebug("Created new Project with ID {ProjectId}", projectEntity.Id);
            return CreatedAtRoute("__Single_Project_By_GUID_ID__", new { ProjectId = projectEntity.Id },
                _mapper.Map<GetProjectDTO>(projectEntity) // Converting to DTO as this is going out to the consumer
                );
        }

        /// <summary>
        /// Updating exsting Project based on Project Id (GUID)
        /// </summary>
        /// <param name="projectId">GUID representing an unique ID of the Project that you want to get</param>
        /// <param name="projectToBeUpdated">required Project properties to be updated</param>
        /// <returns>No contents are returned but only Status 204 indicating that Project was updated successfully </returns>
        /// <response code="204">No content are returned but status of 204 indicated that Project was successfully updated</response>
        /// <response code="404">Project based on projectId was not found</response>
        /// <response code="409">Project with updated name already exists in the repository</response>

        [HttpPut("{projectId}", Name = "__PUT_Update_Project_By_GUID_ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public ActionResult UpdateProjectPut(Guid projectId, UpdateProjectDTO projectToBeUpdated)
        {
            if (!_projectRepository.ProjectExists(projectId))
            {
                _logger.LogDebug("HttpPut - Update request for Project for ID {ProjectId} could not be found", projectId);
                return NotFound();
            }

            var projectFromRepo = _projectRepository.GetProjectForUpdate(projectId);

            if (projectFromRepo == null)
            {
                _logger.LogDebug("HttpPut - Update request for Project for ID {ProjectId} could not be found in the Repository", projectId);
                return NotFound();
            }

            if (_projectRules.UniqueProjectNameRule(projectToBeUpdated.Name, projectFromRepo.Name))
            {
                return Conflict($"Project with name '{projectToBeUpdated.Name}' already exists in the repository");
            }

            _mapper.Map(projectToBeUpdated, projectFromRepo);
            try 
            { 
            _projectRepository.UpdateProject(projectFromRepo);
            _projectRepository.Save();

        }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
            {
                _logger.LogDebug("Exception Occured while trying to add new project:" + dbUpdateException.InnerException);
                return Conflict($"Project with name '{projectToBeUpdated.Name}' already exists in the repository");
    }
    _logger.LogDebug("HttpPut - Update request for Project for ID {ProjectId} processed successfully", projectId);
            return NoContent(); // This indicates that update was successfully saved in the DB.

        }

        /// <summary>
        /// Partially updating a single **Project**
        /// </summary>
        /// <param name="projectId">Id of the Project representated by a GUID.</param>
        /// <param name="projectPatchDocument">The set of operations to apply to the Project via JsonPatchDocument</param>
        /// <returns>an ActionResult of type Project</returns>
        /// <response code="204">No content are returned but status of 204 indicated that Project was successfully updated</response>
        /// <response code="404">Project based on projectId was not found</response>
        /// <response code="409">Project with updated name already exists in the repository</response>
        /// <response code="422">Validation problems occured during analyzing validation rules for the JsonPatchDocument </response>
        /// <remarks> Sample request (this request updates an **Project's name**)   
        /// Documentation regarding JSON Patch can be found at 
        /// *[ASP.NET Core - JSON Patch Operations](https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.1#operations)* 
        /// 
        ///     PATCH /api/Projects/{id}  
        ///     [  
        ///	        {   
        ///             "op": "replace",   
        ///             "path": "/name",   
        ///             "value": "PATCH Updated Name field"  
        ///	        }   
        ///     ]
        /// </remarks>

        [HttpPatch("{projectId}", Name = "__PATCH_Update_Project_By_GUID_ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public ActionResult UpdateProjectPatch(Guid projectId, JsonPatchDocument<UpdateProjectDTO> projectPatchDocument)
        {
            if (!_projectRepository.ProjectExists(projectId))
            {
                _logger.LogDebug("HttpPatch - Update request for Project for ID {ProjectId} could not be found", projectId);
                return NotFound();
            }

            var projectFromRepo = _projectRepository.GetProjectForUpdate(projectId);

            if (projectFromRepo == null)
            {
                _logger.LogDebug("HttpPatch - Update request for Project for ID {ProjectId} could not be found in the Repository", projectId);
                return NotFound();
            }

            var projectToPatch = _mapper.Map<UpdateProjectDTO>(projectFromRepo);

            projectPatchDocument.ApplyTo(projectToPatch, ModelState);

            // Validating Project patch document and verifying that it meets all the 
            // validation rules as expected. This will check if the data passed in the Patch Document
            // is ready to be saved in the db.

            if (!TryValidateModel(projectToPatch))
            {
                _logger.LogDebug("HttpPatch - Project Properties did not pass defined Validation Rules for ID {ProjectId}", projectId);
                return ValidationProblem(ModelState);
            }

            if (_projectRules.UniqueProjectNameRule(projectToPatch.Name, projectFromRepo.Name))
            {
                return Conflict($"Project with name '{projectToPatch.Name}' already exists in the repository");
            }

            _mapper.Map(projectToPatch, projectFromRepo);
            try
            {
                _projectRepository.UpdateProject(projectFromRepo);
                _projectRepository.Save();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
            {
                _logger.LogDebug("Exception Occured while trying to add new project:" + dbUpdateException.InnerException);
                return Conflict($"Project with name '{projectToPatch.Name}' already exists in the repository");
            }

            _logger.LogDebug("HttpPatch - Update request for Project for ID {ProjectId} processed successfully", projectId);
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
        /// Deleting a specific Project
        /// </summary>
        /// <param name="projectId">GUID representing an unique ID of the Project that you want to get</param>
        /// <returns>Status code 204 is returned without any content indicating that deletion of the specified Project was successful</returns>
        /// <response code="404">Project based on projectId was not found</response>
        [HttpDelete("{projectId}", Name = "__DELETE_Project_By_GUID_ID__")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult DeleteProject(Guid projectId)
        {
            if (!_projectRepository.ProjectExists(projectId))
            {
                _logger.LogDebug("Cannot Delete Project with ID {ProjectId} as it could not be found", projectId);
                return NotFound();
            }

            var projectFromRepo = _projectRepository.GetProjectForUpdate(projectId);

            if (projectFromRepo == null)
            {
                _logger.LogDebug("Cannot Delete Project with ID {ProjectId} as it could not be found in the Repository", projectId);
                return NotFound();
            }

            _projectRepository.DeleteProject(projectFromRepo);
            _projectRepository.Save();

            _logger.LogDebug("Delete request for Projeect with ID {ProjectId} processed successfully", projectId);
            return NoContent();
        }


        /// <summary>
        /// Get list of supported HTTP Options for the Projects controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions(Name = "__OPTIONS_for_Projects_Controller__")]
        public IActionResult GetProjectsOptions()
        {
            Response.Headers.Add("Allow", "GET,HEAD,OPTIONS,POST,PUT,PATCH,DELETE");
            return Ok();
        }



    }
}
