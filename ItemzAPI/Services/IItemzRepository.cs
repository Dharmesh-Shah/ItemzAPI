// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItemzApp.API.Entities;
using ItemzApp.API.Helper;
using ItemzApp.API.ResourceParameters;

namespace ItemzApp.API.Services
{
    public interface IItemzRepository
    {
        Itemz GetItemz(Guid ItemzId);

        Itemz GetItemzForUpdating(Guid ItemzId);

        PagedList<Itemz> GetItemzs(ItemzResourceParameter itemzResourceParameter);

        public IEnumerable<Itemz> GetItemzs(IEnumerable<Guid> itemzIds);

        void AddItemz(Itemz itemz);

        bool Save();

        public bool ItemzExists(Guid itemzId);

        public void UpdateItemz(Itemz itemz);

        void DeleteItemz(Itemz itemz);

    }
}
