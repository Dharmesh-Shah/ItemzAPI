// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;

#nullable enable

namespace ItemzApp.API.Services
{
    public interface IItemzTypeRepository
    {
        public Task<ItemzType> GetItemzTypeAsync(Guid ItemzTypeId);

        public Task<ItemzType> GetItemzTypeForUpdateAsync(Guid ItemzTypeId);

        public Task<IEnumerable<ItemzType>?> GetItemzTypesAsync();

        public Task<IEnumerable<ItemzType>> GetItemzTypesAsync(IEnumerable<Guid> itemzTypeIds);
                
        public void AddItemzType(ItemzType itemzType);

        public Task<bool> SaveAsync();

        public Task<bool> ItemzTypeExistsAsync(Guid ItemzTypeId);

        public void UpdateItemzType(ItemzType itemzType);

        public void DeleteItemzType(ItemzType itemzType);

        public Task<bool> HasItemzTypeWithNameAsync(Guid projectId, string itemzTypeName);
    }
}

#nullable disable
