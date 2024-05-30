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
    [Route("api/[controller]")] // e.g. http://HOST:PORT/api/ONLYforTestingProjects
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ONLYforTestingProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<ONLYforTestingProjectsController> _logger;

        public ONLYforTestingProjectsController(IProjectRepository projectRepository, ILogger<ONLYforTestingProjectsController> logger)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// <summary>
        /// This option is designed only to be used for adding a new record while testing ItemzApp API.
        /// Used for creating new Project record in the database that also 
        /// accepts Project ID as part of input parameter
        /// </summary>
        /// <param name="project">Parameter that contains necessary properties for creating new project in the database</param>
        /// <returns>Newly created Project property details</returns>
        /// <response code="201">Returns newly created project's property details</response>
        [HttpPost(Name = "__POST_ONLY_FOR_TESTING_Create_Project__")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetProjectDTO>> CreateProjectAsync(Project project)
        {
            if (!(await _projectRepository.ProjectExistsAsync(project.Id)))
            {
                _projectRepository.AddProject(project);
                await _projectRepository.SaveAsync();
                _logger.LogDebug("{FormattedControllerAndActionNames}Created new Project with ID {ProjectId} via __POST_ONLY_FOR_TESTING_Create_Project__",
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext), 
                    project.Id);
            }


            // TODO: Try and Catch logic here is not clear and it might add project
            // in the DB even if adding hierarchy record fails. In such cases 
            // we need both this steps to be included in one single transaction. 
            // If there is an issue to add Project into hierarchy table then we will not be
            // able to work with it's ItemzType and Itemz which are expected to be childrens.

            try
            {
                await _projectRepository.AddNewProjectHierarchyAsync(project);
                await _projectRepository.SaveAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbUpdateException)
            {
                _logger.LogDebug("{FormattedControllerAndActionNames}Exception Occured while trying to add new project hierarchy:" + dbUpdateException.InnerException,
                    ControllerAndActionNames.GetFormattedControllerAndActionNames(ControllerContext)
                    );
                return Conflict($"Could not add hierarchy for newly created project '{project.Name}' ");
            }

            return CreatedAtRoute("__Single_Project_By_GUID_ID__", new { Controller = "Projects", ProjectId = project.Id }, await _projectRepository.GetProjectAsync(project.Id));
        }
        /// <summary>
        /// Get list of supported HTTP Options for the ONLYforTestingProjects controller.
        /// </summary>
        /// <returns>Custom response header with key as "Allow" and value as different HTTP options that are allowed</returns>
        /// <response code="200">Custom response header with key as "Allow" and value as different HTTP options that are allowed</response>

        [HttpOptions(Name = "__OPTIONS_ONLY_FOR_TESTING_Get_Project__")]
        public IActionResult GetProjectsOptions()
        {
            Response.Headers.Add("Allow", "OPTIONS,POST");
            return Ok();
        }


    }
}
