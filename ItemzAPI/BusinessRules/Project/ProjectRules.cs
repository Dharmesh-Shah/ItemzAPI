using ItemzApp.API.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.BusinessRules.Project
{
    public class ProjectRules : IProjectRules
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<ProjectRules> _logger;
        public ProjectRules(IProjectRepository projectRepository,
                                 ILogger<ProjectRules> logger)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        /// <summary>
        /// Use this method to check if the project with given name already exists. In General, 
        /// This check shall be performed before inserting or updating project.
        /// </summary>
        /// <param name="projectName">Name of the project to be checked for uniqueness</param>
        /// <returns>true if project with projectName found otherwise false</returns>
        private bool HasProjectWithName(string projectName)
        {
            if (_projectRepository.HasProjectWithName(projectName.Trim().ToLower()))
            {
                _logger.LogDebug("Project with name \"{projectName}\" already exists in the repository", projectName);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Used for verifying if repository contains project with the 
        /// same name as the one used for inserting or updating
        /// </summary>
        /// <param name="targetProjectName">New or updated project name</param>
        /// <param name="sourceProjectName">Old project name. No need to pass this for checking rule against creating project action</param>
        /// <returns>true if project with same name exist in the repository otherwise false</returns>
        public bool UniqueProjectNameRule(string targetProjectName, string sourceProjectName = null)
        { 
            if (sourceProjectName != null )
            { // Update existing project name
                if (sourceProjectName != targetProjectName)
                { // Source and Target are different names
                    return HasProjectWithName(targetProjectName);
                }
                return false;
            }
            else
            { // Create new project action
                return HasProjectWithName(targetProjectName);
            }
        }
    }
}
