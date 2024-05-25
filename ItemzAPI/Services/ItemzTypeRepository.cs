// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.DbContexts;
using ItemzApp.API.Entities;
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

        public async Task<ItemzType> GetItemzTypeAsync(Guid ItemzTypeId)
        {
            if (ItemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzTypeId));
            }

            return await _context.ItemzTypes!
                .Where(c => c.Id == ItemzTypeId).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<ItemzType> GetItemzTypeForUpdateAsync(Guid ItemzTypeId)
        {
            if (ItemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzTypeId));
            }

            return await _context.ItemzTypes!
                .Where(c => c.Id == ItemzTypeId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ItemzType>?> GetItemzTypesAsync()
        {
            try
            {
                if (await _context.ItemzTypes.CountAsync<ItemzType>() > 0)
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

            _context.ItemzTypes!.Add(itemzType);
        }

        public async Task AddNewItemzTypeHierarchyAsync(ItemzType itemzTypeEntity)
        {
            if (itemzTypeEntity == null)
            {
                throw new ArgumentNullException(nameof(itemzTypeEntity));
            }

            var rootProjectItemz = _context.ItemzHierarchy!.AsNoTracking()
                            .Where(ih => ih.Id == itemzTypeEntity.ProjectId);

            if (rootProjectItemz.Count() != 1)
            {
                throw new ApplicationException("Either no Root Project Repository Hierarchy record " +
                    "found OR multiple Root Project Repository Hierarchy records found in the system");
            }

            // EXPLANATION : We are using SQL Server HierarchyID field type. Now we can use EF Core special
            // methods to query for all Decendents as per below. We are actually finding all Decendents by saying
            // First find the ItemzHierarchy record where ID matches RootProject ID. This is expected to be the
            // Project ID itself which is the root OR parent to newly ItemzType.
            // Then we find all desendents of Repository which is nothing but existing ItemzType(s). 

            var itemzTypeHierarchyRecords = await _context.ItemzHierarchy!
                    .AsNoTracking()
                    .Where(ih => ih.ItemzHierarchyId!.GetAncestor(1) == rootProjectItemz.FirstOrDefault()!.ItemzHierarchyId!)
                    .OrderByDescending(ih => ih.ItemzHierarchyId!)
                    .ToListAsync();

            var tempItemzTypeHierarchy = new Entities.ItemzHierarchy
            {
                Id = itemzTypeEntity.Id,
                RecordType = "ItemzType",
                ItemzHierarchyId = rootProjectItemz.FirstOrDefault()!.ItemzHierarchyId!
                                    .GetDescendant(itemzTypeHierarchyRecords.Count() > 0
                                                        ? itemzTypeHierarchyRecords.FirstOrDefault()!.ItemzHierarchyId
                                                        : null
                                                   , null),
            };

            _context.ItemzHierarchy!.Add(tempItemzTypeHierarchy);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

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
            _context.ItemzTypes!.Remove(itemzType);
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

        public async Task<bool> HasItemzTypeWithNameAsync(Guid projectId, string itemzTypeName)
        {
            return await _context.ItemzTypes.AsNoTracking().AnyAsync(it => it.ProjectId.ToString().ToLower() == projectId.ToString().ToLower() && it.Name!.ToLower() == itemzTypeName.ToLower());
        }
    }
}
