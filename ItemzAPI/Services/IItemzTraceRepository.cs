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
    public interface IItemzTraceRepository
    {

        public Task<bool> SaveAsync();

        public Task<IEnumerable<ItemzJoinItemzTrace>> GetAllTracesByItemzIdAsync(Guid itemzId);

        public Task<bool> ItemzsTraceExistsAsync(ItemzTraceDTO itemzTraceDTO);

        public Task<bool> EstablishTraceBetweenItemzAsync(ItemzTraceDTO itemzTraceDTO);

        public Task<bool> ItemzExistsAsync(Guid itemzId);

        public Task<bool> RemoveItemzTraceAsync(ItemzTraceDTO itemzTraceDTO);
    }
}
