// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;

namespace ItemzApp.API.Services
{
    public interface IItemzTypeRepository
    {
        // public ItemzType GetItemzType(Guid ItemzTypeId);

        public Task<ItemzType> GetItemzTypeAsync(Guid ItemzTypeId);

        // public ItemzType GetItemzTypeForUpdate(Guid ItemzTypeId);

        public Task<ItemzType> GetItemzTypeForUpdateAsync(Guid ItemzTypeId);
        //public IEnumerable<ItemzType> GetItemzTypes();

        public Task<IEnumerable<ItemzType>> GetItemzTypesAsync();

        // public IEnumerable<ItemzType> GetItemzTypes(IEnumerable<Guid> itemzTypeIds);
        public Task<IEnumerable<ItemzType>> GetItemzTypesAsync(IEnumerable<Guid> itemzTypeIds);

        
        public void AddItemzType(ItemzType itemzType);

        // public bool Save();
        public Task<bool> SaveAsync();

        //public bool ItemzTypeExists(Guid ItemzTypeId);

        public Task<bool> ItemzTypeExistsAsync(Guid ItemzTypeId);

        public void UpdateItemzType(ItemzType itemzType);

        public void DeleteItemzType(ItemzType itemzType);

        // public bool HasItemzTypeWithName(Guid projectId, string itemzTypeName);

        public Task<bool> HasItemzTypeWithNameAsync(Guid projectId, string itemzTypeName);
    }
}
