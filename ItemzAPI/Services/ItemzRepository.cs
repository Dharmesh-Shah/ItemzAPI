// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.DbContexts;
using ItemzApp.API.Entities;
using ItemzApp.API.Helper;
using ItemzApp.API.Models;
using ItemzApp.API.ResourceParameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Services
{

    public class ItemzRepository : IItemzRepository, IDisposable
    {
        private readonly ItemzContext _context;
        private readonly IPropertyMappingService _propertyMappingService;
        public ItemzRepository(ItemzContext context,
            IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public Itemz GetItemz(Guid ItemzId)
        {

            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }

            return _context.Itemzs
                .Where(c => c.Id == ItemzId).AsNoTracking().FirstOrDefault();
        }

        public Itemz GetItemzForUpdating(Guid ItemzId)
        {

            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }

            return _context.Itemzs
                .Where(c => c.Id == ItemzId).FirstOrDefault();
        }

        public IEnumerable<Itemz> GetItemzs(IEnumerable<Guid> itemzIds)
        {

            if (itemzIds == null)
            {
                throw new ArgumentNullException(nameof(itemzIds));
            }

            return _context.Itemzs.AsNoTracking().Where(a => itemzIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToList();

        }
        public PagedList<Itemz> GetItemzs(ItemzResourceParameter itemzResourceParameter)
        {
            // TODO: Should we check for itemzResourceParameter being null?
            // There are chances that we just want to get all the itemz and
            // consumer of the API might now pass in necessary values for pagging.

            if (itemzResourceParameter == null)
            {
                throw new ArgumentNullException(nameof(itemzResourceParameter));
            }
            try
            {
                if (_context.Itemzs.Count<Itemz>() > 0)
                {
                    var itemzCollection = _context.Itemzs.AsQueryable<Itemz>(); // as IQueryable<Itemz>;

                    if (!string.IsNullOrWhiteSpace(itemzResourceParameter.OrderBy))
                    {
                        var itemzPropertyMappingDictionary =
                                               _propertyMappingService.GetPropertyMapping<Models.GetItemzDTO, Itemz>();

                        itemzCollection = itemzCollection.ApplySort(itemzResourceParameter.OrderBy,
                            itemzPropertyMappingDictionary).AsNoTracking();  
                    }

                    // EXPLANATION: Pagging feature should be implemented at the end 
                    // just before calling ToList. This will make sure that any filtering,
                    // sorting, grouping, etc. that we implement on the data are 
                    // put in place before calling ToList. 

                    return PagedList<Itemz>.Create(itemzCollection,
                        itemzResourceParameter.PageNumber,
                        itemzResourceParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                // TODO: It's not good that we capture Generic Exception and then 
                // return null here. Basically, I wanted to check if we have 
                // itemzs returned from the DB and if it does not then
                // it should simply return null back to the calling function.
                // One has to learn how to do this gracefully as part of Entity Framework 
                return null;
            }
        }

        public PagedList<Itemz> GetItemzsByProject(Guid projectId, ItemzResourceParameter itemzResourceParameter)
        {
            // TODO: Should we check for itemzResourceParameter being null?
            // There are chances that we just want to get all the itemz and
            // consumer of the API might now pass in necessary values for pagging.

            if (itemzResourceParameter == null)
            {
                throw new ArgumentNullException(nameof(itemzResourceParameter));
            }

            if(projectId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            try
            {
                if (_context.Itemzs.Count<Itemz>() > 0)
                {
                    var itemzCollection = _context.Itemzs
                        .Include(i => i.ProjectJoinItemz)
//                        .ThenInclude(PjI => PjI.Project)
                        .Where(i => i.ProjectJoinItemz.Any(PjI => PjI.ProjectId == projectId));
                        
                   //     .Where(i => i.  . AsQueryable<Itemz>(); // as IQueryable<Itemz>;

                    if (!string.IsNullOrWhiteSpace(itemzResourceParameter.OrderBy))
                    {
                        var itemzPropertyMappingDictionary =
                                               _propertyMappingService.GetPropertyMapping<Models.GetItemzDTO, Itemz>();

                        itemzCollection = itemzCollection.ApplySort(itemzResourceParameter.OrderBy,
                            itemzPropertyMappingDictionary).AsNoTracking();
                    }

                    // EXPLANATION: Pagging feature should be implemented at the end 
                    // just before calling ToList. This will make sure that any filtering,
                    // sorting, grouping, etc. that we implement on the data are 
                    // put in place before calling ToList. 

                    return PagedList<Itemz>.Create(itemzCollection,
                        itemzResourceParameter.PageNumber,
                        itemzResourceParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                // TODO: It's not good that we capture Generic Exception and then 
                // return null here. Basically, I wanted to check if we have 
                // itemzs returned from the DB and if it does not then
                // it should simply return null back to the calling function.
                // One has to learn how to do this gracefully as part of Entity Framework 
                return null;
            }
        }
        public PagedList<Itemz> GetItemzsByItemzType(Guid itemzTypeId, ItemzResourceParameter itemzResourceParameter)
        {
            // TODO: Should we check for itemzResourceParameter being null?
            // There are chances that we just want to get all the itemz and
            // consumer of the API might now pass in necessary values for pagging.

            if (itemzResourceParameter == null)
            {
                throw new ArgumentNullException(nameof(itemzResourceParameter));
            }

            if (itemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTypeId));
            }
            try
            {
                if (_context.Itemzs.Count<Itemz>() > 0)
                {
                    var itemzCollection = _context.Itemzs
                        .Include(i => i.ItemzTypeJoinItemz)
                        //                        .ThenInclude(PjI => PjI.ItemzType)
                        .Where(i => i.ItemzTypeJoinItemz.Any(itji => itji.ItemzTypeId == itemzTypeId));

                    //     .Where(i => i.  . AsQueryable<Itemz>(); // as IQueryable<Itemz>;

                    if (!string.IsNullOrWhiteSpace(itemzResourceParameter.OrderBy))
                    {
                        var itemzPropertyMappingDictionary =
                                               _propertyMappingService.GetPropertyMapping<Models.GetItemzDTO, Itemz>();

                        itemzCollection = itemzCollection.ApplySort(itemzResourceParameter.OrderBy,
                            itemzPropertyMappingDictionary).AsNoTracking();
                    }

                    // EXPLANATION: Pagging feature should be implemented at the end 
                    // just before calling ToList. This will make sure that any filtering,
                    // sorting, grouping, etc. that we implement on the data are 
                    // put in place before calling ToList. 

                    return PagedList<Itemz>.Create(itemzCollection,
                        itemzResourceParameter.PageNumber,
                        itemzResourceParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                // TODO: It's not good that we capture Generic Exception and then 
                // return null here. Basically, I wanted to check if we have 
                // itemzs returned from the DB and if it does not then
                // it should simply return null back to the calling function.
                // One has to learn how to do this gracefully as part of Entity Framework 
                return null;
            }
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

        public void AddItemz(Itemz itemz)
        {
            if (itemz == null)
            {
                throw new ArgumentNullException(nameof(itemz));
            }

            _context.Itemzs.Add(itemz);
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void AddItemzByProject(Itemz itemz, Guid projectId)
        {
            if (itemz == null)
            {
                throw new ArgumentNullException(nameof(itemz));
            }

            if (projectId == null)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            var tempproject = _context.Projects.Find(projectId);
            _context.Itemzs.Add(itemz);
            var pji = new ProjectJoinItemz { Itemz = itemz, Project = tempproject };
            _context.ProjectJoinItemz.Add(pji);
        }

        public void AddItemzByItemzType(Itemz itemz, Guid itemzTypeId)
        {
            if (itemz == null)
            {
                throw new ArgumentNullException(nameof(itemz));
            }

            if (itemzTypeId == null)
            {
                throw new ArgumentNullException(nameof(itemzTypeId));
            }

            var tempitemzType = _context.ItemzTypes.Find(itemzTypeId);
            _context.Itemzs.Add(itemz);
            var itji = new ItemzTypeJoinItemz { Itemz = itemz, ItemzType = tempitemzType };
            _context.ItemzTypeJoinItemz.Add(itji);
        }
        public bool ItemzExists(Guid itemzId)
        {
            if (itemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzId));
            }

            // EXPLANATION: We expect ItemzExists to be used independently on it's own without
            // expecting it to track the itemz that was found in the database. That's why it's not
            // a good idea to use "!(_context.Itemzs.Find(itemzId) == null)" option
            // to "Find()" Itemz. This is because Find is designed to track the itemz in the memory.
            // In "Itemz Delete controller method", we are first checking if ItemzExists and then 
            // we call Itemz Delete to actually remove it. This is going to be in the single scoped
            // DBContext. If we use "Find()" method then it will start tracking the itemz and then we can't
            // get the itemz once again from the DB as it's already being tracked. We have a choice here
            // to decide if we should always use Find via ItemzExists and then yet again in the subsequent
            // operations like Delete / Update or we use ItemzExists as independent method and not rely on 
            // it for subsequent operations like Delete / Update.

            return _context.Itemzs.AsNoTracking().Any(a => a.Id == itemzId);
            // return  !(_context.Itemzs.Find(itemzId) == null);
        }

        public bool ProjectExists(Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            // EXPLANATION: Using ".Any()" instead of ".Find" as explained in method
            // public bool ItemzExists(Guid itemzId)

            return _context.Projects.AsNoTracking().Any(p => p.Id == projectId);
        }

        public bool ItemzTypeExists(Guid itemzTypeId)
        {
            if (itemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTypeId));
            }

            // EXPLANATION: Using ".Any()" instead of ".Find" as explained in method
            // public bool ItemzExists(Guid itemzId)

            return _context.ItemzTypes.AsNoTracking().Any(it => it.Id == itemzTypeId);
        }


        public bool ProjectItemzExists(ProjectItemzDTO projectItemzDTO)
        {
            if (projectItemzDTO == null)
            {
                throw new ArgumentNullException(nameof(projectItemzDTO));
            }

            // EXPLANATION: Using ".Any()" instead of ".Find" as explained in method
            // public bool ItemzExists(Guid itemzId)

            return _context.ProjectJoinItemz.AsNoTracking().Any(p => p.ItemzId == projectItemzDTO.ItemzId 
                                                                && p.ProjectId == projectItemzDTO.ProjectId ) ;
        }


        public bool ItemzTypeItemzExists(ItemzTypeItemzDTO itemzTypeItemzDTO)
        {
            if (itemzTypeItemzDTO == null)
            {
                throw new ArgumentNullException(nameof(itemzTypeItemzDTO));
            }

            // EXPLANATION: Using ".Any()" instead of ".Find" as explained in method
            // public bool ItemzExists(Guid itemzId)

            return _context.ItemzTypeJoinItemz.AsNoTracking().Any(itji => itji.ItemzId == itemzTypeItemzDTO.ItemzId
                                                                && itji.ItemzTypeId == itemzTypeItemzDTO.ItemzTypeId);
        }

        public void UpdateItemz(Itemz itemz)
        {
            // Due to Repository Pattern implementation, 
            // there is no code in this implementation.  
        }

        public void DeleteItemz(Itemz itemz)
        {
            _context.Itemzs.Remove(itemz);
        }

        public void RemoveItemzFromProject(ProjectItemzDTO projectItemzDTO)
        {
            var pji = _context.ProjectJoinItemz.Find(projectItemzDTO.ProjectId,projectItemzDTO.ItemzId);
            if (pji != null)
            {
                _context.ProjectJoinItemz.Remove(pji);
            }
        }

        public void RemoveItemzFromItemzType(ItemzTypeItemzDTO itemzTypeItemzDTO)
        {
            var itji = _context.ItemzTypeJoinItemz.Find(itemzTypeItemzDTO.ItemzTypeId, itemzTypeItemzDTO.ItemzId);
            if (itji != null)
            {
                _context.ItemzTypeJoinItemz.Remove(itji);
            }
        }


        public void AssociateItemzToProject(ProjectItemzDTO projectItemzDTO)
        {
            var pji = _context.ProjectJoinItemz.Find(projectItemzDTO.ProjectId, projectItemzDTO.ItemzId);
            if (pji == null)
            {
                //var tempItemz = _context.Itemzs.AsNoTracking().FirstOrDefault(a => a.Id == projectItemzDTO.ItemzId);
                //if (tempItemz == null)
                //{
                //    throw new Exception($"Itemz with ID {projectItemzDTO.ItemzId} could not be found.");
                //}
                //var tempProject = _context.Projects.AsNoTracking().FirstOrDefault(a => a.Id == projectItemzDTO.ProjectId);
                //if (tempProject == null)
                //{
                //    throw new Exception($"Project with ID {projectItemzDTO.ProjectId} could not be found.");
                //}

                var temp_pji = new ProjectJoinItemz();
                temp_pji.ItemzId = projectItemzDTO.ItemzId;
                temp_pji.ProjectId = projectItemzDTO.ProjectId;
                _context.ProjectJoinItemz.Add(temp_pji);
            }
        }

        public void AssociateItemzToItemzType(ItemzTypeItemzDTO itemzTypeItemzDTO)
        {
            var itji = _context.ItemzTypeJoinItemz.Find(itemzTypeItemzDTO.ItemzTypeId, itemzTypeItemzDTO.ItemzId);
            if (itji == null)
            {
                var temp_itji = new ItemzTypeJoinItemz
                {
                    ItemzId = itemzTypeItemzDTO.ItemzId,
                    ItemzTypeId = itemzTypeItemzDTO.ItemzTypeId
                };
                _context.ItemzTypeJoinItemz.Add(temp_itji);
            }
        }
        public void MoveItemzFromOneProjectToAnother(ProjectItemzDTO sourceProjectItemzDTO, ProjectItemzDTO targetProjectItemzDTO)
        {
            // EXPLANATION: Fow now best thing to do would be to remove unwanted itemz and project association 
            // and then find target  association and if not found then simply add it. 
            // This method should be used for moving one itemz at a time. If one would like to move
            // multiple items (i.e. 100s of them in bulk) then this method of updating one record at a time
            // is not very efficient. We will have to come-up with alternative option for 
            // Bulk updating multiple itemz and project association. 

            RemoveItemzFromProject(sourceProjectItemzDTO);
            AssociateItemzToProject(targetProjectItemzDTO);
        }
        public void MoveItemzFromOneItemzTypeToAnother(ItemzTypeItemzDTO sourceItemzTypeItemzDTO, ItemzTypeItemzDTO targetItemzTypeItemzDTO)
        {
            // EXPLANATION: Fow now best thing to do would be to remove unwanted itemz and itemzType association 
            // and then find target  association and if not found then simply add it. 
            // This method should be used for moving one itemz at a time. If one would like to move
            // multiple items (i.e. 100s of them in bulk) then this method of updating one record at a time
            // is not very efficient. We will have to come-up with alternative option for 
            // Bulk updating multiple itemz and itemzType association. 

            RemoveItemzFromItemzType(sourceItemzTypeItemzDTO);
            AssociateItemzToItemzType(targetItemzTypeItemzDTO);
        }

    }
}
