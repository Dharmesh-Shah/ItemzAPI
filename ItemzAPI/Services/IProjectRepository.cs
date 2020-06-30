// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using ItemzApp.API.Entities;

namespace ItemzApp.API.Services
{
    public interface IProjectRepository
    {
        public Project GetProject(Guid ProjectId);
        public Project GetProjectForUpdate(Guid ProjectId);
        
        public IEnumerable<Project> GetProjects();

        public IEnumerable<Project> GetProjects(IEnumerable<Guid> projectIds);

        public void AddProject(Project project);

        public bool Save();

        public bool ProjectExists(Guid projectId);

        public void UpdateProject(Project project);

        public void DeleteProject(Project project);

    }
}
