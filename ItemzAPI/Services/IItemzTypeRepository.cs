// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using ItemzApp.API.Entities;

namespace ItemzApp.API.Services
{
    public interface IItemzTypeRepository
    {
        public ItemzType GetItemzType(Guid ItemzTypeId);
        public ItemzType GetItemzTypeForUpdate(Guid ItemzTypeId);
        
        public IEnumerable<ItemzType> GetItemzTypes();

        public IEnumerable<ItemzType> GetItemzTypes(IEnumerable<Guid> itemzTypeIds);

        public void AddItemzType(ItemzType itemzType);

        public bool Save();

        public bool ItemzTypeExists(Guid ItemzTypeId);

        public void UpdateItemzType(ItemzType itemzType);

        public void DeleteItemzType(ItemzType itemzType);

        public bool HasItemzTypeWithName(Guid projectId, string itemzTypeName);
    }
}
