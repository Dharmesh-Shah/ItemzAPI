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
    public class ItemzTraceRepository : IItemzTraceRepository, IDisposable
    {
        private readonly ItemzContext _context;
        private readonly ItemzTraceContext _itemzTraceContext;
        private readonly IPropertyMappingService _propertyMappingService;
        public ItemzTraceRepository (ItemzContext context, 
                                        ItemzTraceContext itemzTraceContext,
                                        IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _itemzTraceContext = itemzTraceContext ?? throw new ArgumentNullException(nameof(itemzTraceContext));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
        }


        public async Task<bool> SaveAsync()
        {
            return (await _itemzTraceContext.SaveChangesAsync() >= 0);
        }
        public async Task<IEnumerable<ItemzJoinItemzTrace>> GetAllTracesByItemzIdAsync(Guid itemzId)
        {
            if (itemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzId));
            }

            return await _itemzTraceContext.ItemzJoinItemzTrace!
                .Where(ijit => ijit.FromItemzId == itemzId || ijit.ToItemzId == itemzId).AsNoTracking().ToListAsync();
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

        /// <summary>
        /// Purpose of this method is to check if Trace is already found between FromItemz and ToItemz
        /// </summary>
        /// <param name="itemzTraceDTO"></param>
        /// <returns></returns>

        public async Task<bool> ItemzsTraceExistsAsync(ItemzTraceDTO itemzTraceDTO)
        {
            if (itemzTraceDTO.FromTraceItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTraceDTO.FromTraceItemzId));
            }

            if (itemzTraceDTO.ToTraceItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTraceDTO.ToTraceItemzId));
            }

            return await _itemzTraceContext.ItemzJoinItemzTrace
                            .AsNoTracking()
                            .AnyAsync(ijit => ijit.FromItemzId == itemzTraceDTO.FromTraceItemzId
                                        && ijit.ToItemzId == itemzTraceDTO.ToTraceItemzId);
        }

        /// <summary>
        /// Purpose for this method is to allow creating new Itemz Trace and saving it.
        /// </summary>
        /// <param name="itemzTraceDTO"></param>
        public async Task<bool> EstablishTraceBetweenItemzAsync(ItemzTraceDTO itemzTraceDTO)
        {
            if (itemzTraceDTO.FromTraceItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTraceDTO.FromTraceItemzId));
            }

            if (itemzTraceDTO.ToTraceItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTraceDTO.ToTraceItemzId));
            }

            if (!(await _context.Itemzs.AsNoTracking().AnyAsync(a => a.Id == itemzTraceDTO.FromTraceItemzId)))
            {
                throw new Exception(nameof(itemzTraceDTO.FromTraceItemzId));
            }

            if (!(await _context.Itemzs.AsNoTracking().AnyAsync(a => a.Id == itemzTraceDTO.ToTraceItemzId)))
            {
                throw new Exception(nameof(itemzTraceDTO.ToTraceItemzId));
            }

            var itrace = await _itemzTraceContext.ItemzJoinItemzTrace!.FindAsync(itemzTraceDTO.FromTraceItemzId, itemzTraceDTO.ToTraceItemzId);
            if(itrace == null)
            {
                var temp_itrace = new ItemzJoinItemzTrace
                {
                    FromItemzId = itemzTraceDTO.FromTraceItemzId,
                    ToItemzId = itemzTraceDTO.ToTraceItemzId
                };
                await _itemzTraceContext.ItemzJoinItemzTrace.AddAsync(temp_itrace);
            }
            return true;
        }

        public async Task<bool> ItemzExistsAsync(Guid itemzId)
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

            return await _context.Itemzs.AsNoTracking().AnyAsync(a => a.Id == itemzId);
            // return  !(_context.Itemzs.Find(itemzId) == null);
        }
    }
}
