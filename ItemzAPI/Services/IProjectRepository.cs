// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;

namespace ItemzApp.API.Services
{
    public interface IProjectRepository
    {
        // public Project GetProject(Guid ProjectId);

        public Task<Project> GetProjectAsync(Guid ProjectId);

        //public Project GetProjectForUpdate(Guid ProjectId);

        public Task<Project> GetProjectForUpdateAsync(Guid ProjectId);

        // public IEnumerable<Project> GetProjects();

        public Task<IEnumerable<Project>> GetProjectsAsync();

        // public IEnumerable<Project> GetProjects(IEnumerable<Guid> projectIds);

        public Task<IEnumerable<Project>> GetProjectsAsync(IEnumerable<Guid> projectIds);

        public void AddProject(Project project);

        //public bool Save();

        public Task<bool> SaveAsync();

        // public bool ProjectExists(Guid projectId);
        
        public Task<bool> ProjectExistsAsync(Guid projectId);

        public void UpdateProject(Project project);

        public void DeleteProject(Project project);

        // public bool HasProjectWithName(string projectName);
        
        public Task<bool> HasProjectWithNameAsync(string projectName);

    }
}
