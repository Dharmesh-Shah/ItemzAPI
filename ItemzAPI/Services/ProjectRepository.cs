// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.DbContexts;
using ItemzApp.API.Entities;
using ItemzApp.API.Helper;
using ItemzApp.API.ResourceParameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ItemzApp.API.Services
{
    public class ProjectRepository : IProjectRepository, IDisposable
    {
        private readonly ItemzContext _context;

        public ProjectRepository(ItemzContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Project GetProject(Guid ProjectId)
        {
            if (ProjectId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ProjectId));
            }

            return _context.Projects
                .Where(c => c.Id == ProjectId).AsNoTracking().FirstOrDefault();
        }
        public Project GetProjectForUpdate(Guid ProjectId)
        {
            if (ProjectId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ProjectId));
            }

            return _context.Projects

                .Where(c => c.Id == ProjectId).FirstOrDefault();
        }

        public IEnumerable<Project> GetProjects()
        {
            try
            {
                if(_context.Projects.Count<Project>()>0)
                {
                    var projectCollection = _context.Projects.AsNoTracking().AsQueryable<Project>().OrderBy(p => p.Name);

                    // TODO: We have to create simple implementation of sort by Project Name here 
                    // projectCollection = projectCollection.ApplySort("Name", null).AsNoTracking();

                    return projectCollection;
                }
                return null;
            }
            catch(Exception ex)
            {
                // TODO: It's not good that we capture Generic Exception and then 
                // return null here. Basically, I wanted to check if we have 
                // projects returned from the DB and if it does not then
                // it should simply return null back to the calling function.
                // One has to learn how to do this gracefully as part of Entity Framework 
                return null;
            }
        }

        //TODO: decide if we need GetProjects by passing in collection of projectIds
        // if yes, then we need to implement action method in ProjectController for the same
        // so that Swagger docs shows GET method under Projects section.
        public IEnumerable<Project> GetProjects(IEnumerable<Guid> projectIds)
        {
            if(projectIds == null)
            {
                throw new ArgumentNullException(nameof(projectIds));
            }

            return _context.Projects.AsNoTracking().Where(a => projectIds.Contains(a.Id))
                .OrderBy(p => p.Name)
                .ToList();
        }

        public void AddProject(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            _context.Projects.Add(project);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool ProjectExists(Guid projectId)
        {
            if(projectId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            // EXPLANATION: We expect ProjectExists to be used independently on it's own without
            // expecting it to track the Project that was found in the database. That's why it's not
            // a good idea to use "!(_context.Projects.Find(projectId) == null)" option
            // to "Find()" Project. This is because Find is designed to track the Project in the memory.
            // In "Project Delete controller method", we are first checking if ProjectExists and then 
            // we call Project Delete to actually remove it. This is going to be in the single scoped
            // DBContext. If we use "Find()" method then it will start tracking the Project and then we can't
            // get the Project once again from the DB as it's already being tracked. We have a choice here
            // to decide if we should always use Find via ProjectExists and then yet again in the subsequent
            // operations like Delete / Update or we use ProjectExists as independent method and not rely on 
            // it for subsequent operations like Delete / Update.

            return _context.Projects.AsNoTracking().Any(p => p.Id == projectId);

        }

        public void UpdateProject(Project project)
        {
            // Due to Repository Pattern implementation, 
            // there is no code in this implementation. 
        }

        public void DeleteProject(Project project)
        {
            _context.Projects.Remove(project);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

        public bool HasProjectWithName(string projectName)
        {
            return _context.Projects.AsNoTracking().Any(p => p.Name.ToLower() == projectName.ToLower());
        }
    }
}
