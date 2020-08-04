// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemzApp.API.Entities;
using ItemzApp.API.Helper;
using ItemzApp.API.Models;
using ItemzApp.API.ResourceParameters;


namespace ItemzApp.API.Services
{
    public interface IItemzRepository
    {
        // Itemz GetItemz(Guid ItemzId);
        Task<Itemz> GetItemzAsync(Guid ItemzId);

        // Itemz GetItemzForUpdating(Guid ItemzId);
        Task<Itemz> GetItemzForUpdatingAsync(Guid ItemzId);

        PagedList<Itemz> GetItemzs(ItemzResourceParameter itemzResourceParameter);

        //PagedList<Itemz> GetItemzsByProject(Guid projectId, ItemzResourceParameter itemzResourceParameter);
        public PagedList<Itemz> GetItemzsByItemzType(Guid itemzTypeId, ItemzResourceParameter itemzResourceParameter);

        //public IEnumerable<Itemz> GetItemzs(IEnumerable<Guid> itemzIds);
        public Task<IEnumerable<Itemz>> GetItemzsAsync(IEnumerable<Guid> itemzIds);

        void AddItemz(Itemz itemz);

        //Task AddItemzAsync(Itemz itemz);

        //void AddItemzByProject(Itemz itemz, Guid projectId);

        public void AddItemzByItemzType(Itemz itemz, Guid itemzTypeId);

        // bool Save();
        Task<bool> SaveAsync();

        // public bool ItemzExists(Guid itemzId);

        public Task<bool> ItemzExistsAsync(Guid itemzId);

        //public bool ProjectExists(Guid projectId);

        // public bool ItemzTypeExists(Guid itemzTypeId);
        public Task<bool> ItemzTypeExistsAsync(Guid itemzTypeId);

        public void UpdateItemz(Itemz itemz);

        //public bool ProjectItemzExists(ProjectItemzDTO projectItemzDTO);

        // public bool ItemzTypeItemzExists(ItemzTypeItemzDTO itemzTypeItemzDTO);
        public Task<bool> ItemzTypeItemzExistsAsync(ItemzTypeItemzDTO itemzTypeItemzDTO);

        //public void RemoveItemzFromProject(ProjectItemzDTO projectItemzDTO);

        public void RemoveItemzFromItemzType(ItemzTypeItemzDTO itemzTypeItemzDTO);

        void DeleteItemz(Itemz itemz);

        //public void AssociateItemzToProject(ProjectItemzDTO projectItemzDTO);

        public void AssociateItemzToItemzType(ItemzTypeItemzDTO itemzTypeItemzDTO);

        //public void MoveItemzFromOneProjectToAnother(ProjectItemzDTO sourceProjectItemzDTO, 
        //                                             ProjectItemzDTO targetProjectItemzDTO);

        public void MoveItemzFromOneItemzTypeToAnother(ItemzTypeItemzDTO sourceItemzTypeItemzDTO,
                                                       ItemzTypeItemzDTO targetItemzTypeItemzDTO);

    }
}
