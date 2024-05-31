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
        Task<Itemz?> GetItemzAsync(Guid ItemzId);

        Task<Itemz?> GetItemzForUpdatingAsync(Guid ItemzId);

        Task<int> GetItemzsCountByItemzType(Guid itemzTypeId); 

        PagedList<Itemz>? GetItemzs(ItemzResourceParameter itemzResourceParameter);

        PagedList<Itemz>? GetOrphanItemzs(ItemzResourceParameter itemzResourceParameter);

        Task<int> GetOrphanItemzsCount();

        public PagedList<Itemz>? GetItemzsByItemzType(Guid itemzTypeId, ItemzResourceParameter itemzResourceParameter);

        public Task<IEnumerable<Itemz>> GetItemzsAsync(IEnumerable<Guid> itemzIds);

        void AddItemz(Itemz itemz);

        public void AddItemzByItemzType(Itemz itemz, Guid itemzTypeId);

        public Task AddNewItemzHierarchyAsync(Itemz itemz, Guid itemzTypeId);

        Task<bool> SaveAsync();

        public Task<bool> ItemzExistsAsync(Guid itemzId);

        public Task<bool> ItemzTypeExistsAsync(Guid itemzTypeId);

        public Task<bool> IsOrphanedItemzAsync(Guid ItemzId);

        public void UpdateItemz(Itemz itemz);

        public Task<bool> ItemzTypeItemzExistsAsync(ItemzTypeItemzDTO itemzTypeItemzDTO);

        public void RemoveItemzFromItemzType(ItemzTypeItemzDTO itemzTypeItemzDTO);

        public Task DeleteItemzAsync(Guid itemzId);

        public void AssociateItemzToItemzType(ItemzTypeItemzDTO itemzTypeItemzDTO);

        public void MoveItemzFromOneItemzTypeToAnother(ItemzTypeItemzDTO sourceItemzTypeItemzDTO,
                                                       ItemzTypeItemzDTO targetItemzTypeItemzDTO);

    }
}
