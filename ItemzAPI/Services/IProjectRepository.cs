// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;

namespace ItemzApp.API.Services
{
    public interface IProjectRepository
    {
        public Task<Project?> GetProjectAsync(Guid ProjectId);

        public Task<Project?> GetProjectForUpdateAsync(Guid ProjectId);

        public Task<IEnumerable<Project>?> GetProjectsAsync();
        
        public Task<IEnumerable<ItemzType>?> GetItemzTypesAsync(Guid ProjectId);
        
        public Task<IEnumerable<Project>> GetProjectsAsync(IEnumerable<Guid> projectIds);

        public void AddProject(Project project);

        public Task<bool> SaveAsync();
        
        public Task<bool> ProjectExistsAsync(Guid projectId);

        public void UpdateProject(Project project);

        public void DeleteProject(Project project);

        Task<int> GetItemzCountByProjectAsync(Guid ProjectId);

        public Task<bool> HasProjectWithNameAsync(string projectName);
    }
}
