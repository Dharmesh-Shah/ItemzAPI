// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.DbContexts;
using ItemzApp.API.Entities;
using ItemzApp.API.Helper;
using ItemzApp.API.ResourceParameters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Services
{
    public class ItemzTypeRepository : IItemzTypeRepository, IDisposable
    {
        private readonly ItemzContext _context;

        public ItemzTypeRepository(ItemzContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //public ItemzType GetItemzType(Guid ItemzTypeId)
        //{
        //    if (ItemzTypeId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(ItemzTypeId));
        //    }

        //    return _context.ItemzTypes
        //        .Where(c => c.Id == ItemzTypeId).AsNoTracking().FirstOrDefault();
        // }

        public async Task<ItemzType> GetItemzTypeAsync(Guid ItemzTypeId)
        {
            if (ItemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzTypeId));
            }

            return await _context.ItemzTypes
                .Where(c => c.Id == ItemzTypeId).AsNoTracking().FirstOrDefaultAsync();
        }
        //public ItemzType GetItemzTypeForUpdate(Guid ItemzTypeId)
        //{
        //    if (ItemzTypeId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(ItemzTypeId));
        //    }

        //    return _context.ItemzTypes

        //        .Where(c => c.Id == ItemzTypeId).FirstOrDefault();
        //}

        public async Task<ItemzType> GetItemzTypeForUpdateAsync(Guid ItemzTypeId)
        {
            if (ItemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzTypeId));
            }

            return await _context.ItemzTypes
                .Where(c => c.Id == ItemzTypeId).FirstOrDefaultAsync();
        }

        //public IEnumerable<ItemzType> GetItemzTypes()
        //{
        //    try
        //    {
        //        if(_context.ItemzTypes.Count<ItemzType>()>0)
        //        {
        //            var itemzTypeCollection = _context.ItemzTypes.AsNoTracking().AsQueryable<ItemzType>().OrderBy(p => p.Name);

        //            // TODO: We have to create simple implementation of sort by ItemzType Name here 
        //            // ItemzTypeCollection = ItemzTypeCollection.ApplySort("Name", null).AsNoTracking();

        //            return itemzTypeCollection;
        //        }
        //        return null;
        //    }
        //    catch(Exception ex)
        //    {
        //        // TODO: It's not good that we capture Generic Exception and then 
        //        // return null here. Basically, I wanted to check if we have 
        //        // itemzTypes returned from the DB and if it does not then
        //        // it should simply return null back to the calling function.
        //        // One has to learn how to do this gracefully as part of Entity Framework 
        //        return null;
        //    }
        //}

        public async Task<IEnumerable<ItemzType>> GetItemzTypesAsync()
        {
            try
            {
                if (_context.ItemzTypes.Count<ItemzType>() > 0)
                {
                    var itemzTypeCollection = await _context.ItemzTypes.AsNoTracking().AsQueryable<ItemzType>().OrderBy(p => p.Name).ToListAsync();

                    // TODO: We have to create simple implementation of sort by ItemzType Name here 
                    // ItemzTypeCollection = ItemzTypeCollection.ApplySort("Name", null).AsNoTracking();

                    return itemzTypeCollection;
                }
                return null;
            }
            catch (Exception ex)
            {
                // TODO: It's not good that we capture Generic Exception and then 
                // return null here. Basically, I wanted to check if we have 
                // itemzTypes returned from the DB and if it does not then
                // it should simply return null back to the calling function.
                // One has to learn how to do this gracefully as part of Entity Framework 
                return null;
            }
        }

        //public IEnumerable<ItemzType> GetItemzTypes(IEnumerable<Guid> itemzTypeIds)
        //{
        //    if(itemzTypeIds == null)
        //    {
        //        throw new ArgumentNullException(nameof(itemzTypeIds));
        //    }

        //    return _context.ItemzTypes.AsNoTracking().Where(a => itemzTypeIds.Contains(a.Id))
        //        .OrderBy(p => p.Name)
        //        .ToList();
        //}

        public async Task<IEnumerable<ItemzType>> GetItemzTypesAsync(IEnumerable<Guid> itemzTypeIds)
        {
            if (itemzTypeIds == null)
            {
                throw new ArgumentNullException(nameof(itemzTypeIds));
            }

            return await _context.ItemzTypes.AsNoTracking().Where(a => itemzTypeIds.Contains(a.Id))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public void AddItemzType(ItemzType itemzType)
        {
            if (itemzType ==null)
            {
                throw new ArgumentNullException(nameof(itemzType));
            }

            _context.ItemzTypes.Add(itemzType);
        }

        //public bool Save()
        //{
        //    return (_context.SaveChanges() >= 0);
        //}

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        //public bool ItemzTypeExists(Guid itemzTypeId)
        //{
        //    if(itemzTypeId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(itemzTypeId));
        //    }

        //    // EXPLANATION: Using ".Any()" instead of ".Find" as explained in method
        //    // public bool ItemzExists(Guid itemzId)
        //    // Above method is found in ItemzRepository.cs

        //    return _context.ItemzTypes.AsNoTracking().Any(p => p.Id == itemzTypeId);

        //}
        public async Task<bool> ItemzTypeExistsAsync(Guid itemzTypeId)
        {
            if (itemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTypeId));
            }

            // EXPLANATION: Using ".Any()" instead of ".Find" as explained in method
            // public bool ItemzExists(Guid itemzId)
            // Above method is found in ItemzRepository.cs

            return await _context.ItemzTypes.AsNoTracking().AnyAsync(p => p.Id == itemzTypeId);

        }

        public void UpdateItemzType(ItemzType itemzType)
        {
            // Due to Repository Pattern implementation, 
            // there is no code in this implementation. 
        }

        public void DeleteItemzType(ItemzType itemzType)
        {
            _context.ItemzTypes.Remove(itemzType);
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
        //public bool HasItemzTypeWithName(Guid projectId, string itemzTypeName)
        //{
        //    return _context.ItemzTypes.AsNoTracking().Any(it => it.ProjectId.ToString().ToLower() == projectId.ToString().ToLower() && it.Name.ToLower() == itemzTypeName.ToLower());
        //}

        public async Task<bool> HasItemzTypeWithNameAsync(Guid projectId, string itemzTypeName)
        {
            return await _context.ItemzTypes.AsNoTracking().AnyAsync(it => it.ProjectId.ToString().ToLower() == projectId.ToString().ToLower() && it.Name.ToLower() == itemzTypeName.ToLower());
        }


    }
}
