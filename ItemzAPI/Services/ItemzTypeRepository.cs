// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.DbContexts;
using ItemzApp.API.Entities;
using ItemzApp.API.Helper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

        public async Task<bool> DeleteItemzTypeItemzHierarchyAsync(Guid itemzTypeId)
        {
            bool returnValue;
            var OUTPUT_Success = new SqlParameter
            {
                ParameterName = "OUTPUT_Success",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Bit,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "ItemzTypeId",
                    Value = itemzTypeId,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                }
            };

            sqlParameters = sqlParameters.Append(OUTPUT_Success).ToArray();

            var _ = await _context.Database.ExecuteSqlRawAsync(sql: "EXEC userProcDeleteItemzHierarchyRecordsByItemzTypeId  @ItemzTypeId, @OUTPUT_Success  = @OUTPUT_Success OUT", parameters: sqlParameters);
            returnValue = (bool)OUTPUT_Success.Value;
            return returnValue;
        }

        public async Task<string?> GetTopItemzHierarchyID(Guid parentItemzTypeId)
        {
            if (parentItemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(parentItemzTypeId));
            }

            var rootItemzType = _context.ItemzHierarchy!.AsNoTracking()
                            .Where(ih => ih.Id == parentItemzTypeId);

            if (rootItemzType.Count() != 1)
            {
                throw new ApplicationException("Either no Root ItemzType Hierarchy record " +
                    "found OR multiple Root ItemzType Hierarchy records found in the system. " +
                    "Please contact your System Administrator.");
            }

            // EXPLANATION : We are using SQL Server HierarchyID field type. Now we can use EF Core special
            // methods to query for all Decendents as per below. We are actually finding all Decendents by saying
            // First find the ItemzHierarchy record where ID matches RootItemz ID. This is expected to be the
            // repository ID itself which is the root. then we find all desendents of Repository which is nothing but Project(s). 

            var itemzTypeHierarchyItemz = await _context.ItemzHierarchy!
                    .AsNoTracking()
                    .Where(ih => ih.ItemzHierarchyId!.GetAncestor(1) == rootItemzType.FirstOrDefault()!.ItemzHierarchyId!)
                    .OrderBy(ih => ih.ItemzHierarchyId!)
                    .ToListAsync();

            //return itemzTypeHierarchyItemz.Count > 0 ?
            //                HierarchyIdStringHelper.ManuallyGenerateHierarchyIdNumberString(
            //                itemzTypeHierarchyItemz.FirstOrDefault()!.ItemzHierarchyId!.ToString()
            //                , diffValue: -1
            //                , addDecimal: false)
            //             : null;

            return itemzTypeHierarchyItemz.Count > 0 ?
                            itemzTypeHierarchyItemz.FirstOrDefault()!.ItemzHierarchyId!.ToString()
                            : null;
        }
        public async Task<string?> GetLastItemzHierarchyID(Guid parentItemzTypeId)
        {

            if (parentItemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(parentItemzTypeId));
            }

            var rootItemzType = _context.ItemzHierarchy!.AsNoTracking()
                            .Where(ih => ih.Id == parentItemzTypeId);


            if (rootItemzType.Count() != 1)
            {
                throw new ApplicationException("Either no Root ItemzType Hierarchy record " +
                    "found OR multiple Root ItemzType Hierarchy records found in the system. " +
                    "Please contact your System Administrator.");
            }

            // EXPLANATION : We are using SQL Server HierarchyID field type. Now we can use EF Core special
            // methods to query for all Decendents as per below. We are actually finding all Decendents by saying
            // First find the ItemzHierarchy record where ID matches RootItemz ID. This is expected to be the
            // repository ID itself which is the root. then we find all desendents of Repository which is nothing but Project(s). 

            var itemzTypeHierarchyItemz = await _context.ItemzHierarchy!
                    .AsNoTracking()
                    .Where(ih => ih.ItemzHierarchyId!.GetAncestor(1) == rootItemzType.FirstOrDefault()!.ItemzHierarchyId!)
                    .OrderByDescending(ih => ih.ItemzHierarchyId!)
                    .ToListAsync();

            return itemzTypeHierarchyItemz.Count > 0 ? itemzTypeHierarchyItemz.FirstOrDefault()!.ItemzHierarchyId!.ToString() : null;
        }


        public async Task MoveItemzTypeToAnotherProjectAsync(Guid movingItemzTypeId, Guid targetProjectId, bool atBottomOfChildNodes = true)
        {
            if (movingItemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(movingItemzTypeId));
            }

            if (targetProjectId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(targetProjectId));
            }

            var movingItemzTypeHierarchyRecordList = _context.ItemzHierarchy!
                .Where(ih => ih.Id == movingItemzTypeId);
            var foundMovingItemzHierarchyRecordCount = movingItemzTypeHierarchyRecordList.Count();
            if (foundMovingItemzHierarchyRecordCount != 1)
            {
                throw new ApplicationException($"{movingItemzTypeHierarchyRecordList.Count()} records found for the " +
                    $"moving Itemz Id {movingItemzTypeId} in the system. " +
                    $"Expected 1 record but instead found {movingItemzTypeHierarchyRecordList.Count()}");
            }

            var movingItemzHierarchyRecord = new ItemzHierarchy();
            string originalItemzHierarchyIdString = "";
            List<ItemzHierarchy> allDescendentItemzHierarchyRecord = new List<ItemzHierarchy>();


            movingItemzHierarchyRecord = movingItemzTypeHierarchyRecordList.FirstOrDefault();
            if (movingItemzHierarchyRecord!.ItemzHierarchyId!.GetLevel() != 2)
            {
                throw new ApplicationException($"Expected {movingItemzHierarchyRecord.Id} " +
                    $"to be 'ItemzType' but instead it's found in Itemz Hierarchy Record " +
                    $"as '{movingItemzHierarchyRecord.RecordType}' ");
            }

            var currentProjectHierarchyRecord = _context.ItemzHierarchy!.AsNoTracking()  
                .Where(ih => ih.ItemzHierarchyId == movingItemzHierarchyRecord.ItemzHierarchyId!.GetAncestor(1));

            if (currentProjectHierarchyRecord.Any())
            {

                if (currentProjectHierarchyRecord.FirstOrDefault()!.Id == targetProjectId)
                {
                    throw new ApplicationException($"Current parent Project ID for {movingItemzTypeId} is {currentProjectHierarchyRecord.FirstOrDefault().Id} " +
                        $"which is same as target Project ID. ItemzType can not be moved to under current parent Project.");
                }

            }

            originalItemzHierarchyIdString = movingItemzHierarchyRecord!.ItemzHierarchyId!.ToString();
            allDescendentItemzHierarchyRecord = await _context.ItemzHierarchy!
                .Where(ih => ih.ItemzHierarchyId!.IsDescendantOf(movingItemzHierarchyRecord!.ItemzHierarchyId)).ToListAsync();

            // RemoveItemzTypeJoinItemzRecord(movingItemzTypeId);


            var newRootHierarchyRecord = _context.ItemzHierarchy!.AsNoTracking()
                            .Where(ih => ih.Id == targetProjectId);
            var newRootHierarchyRecordLevel = newRootHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!.GetLevel();
            if (newRootHierarchyRecord.Count() != 1)
            {
                throw new ApplicationException($"{newRootHierarchyRecord.Count()} records found for the " +
                    $"New Root Hierarchy Id {targetProjectId} in the system. " +
                    $"Expected 1 record but instead found {newRootHierarchyRecord.Count()}");
            }

            if (newRootHierarchyRecordLevel != 1)
            {
                throw new ApplicationException($"New Root Hierarchy record has to be of type 'Project'");
            }

            // EXPLANATION : We are using SQL Server HierarchyID field type. Now we can use EF Core special
            // methods to query for all Decendents as per below. We are actually finding all Decendents by saying
            // First find the ItemzHierarchy record where ID matches RootItemzType ID. This is expected to be the
            // ItemzType ID itself which is the root OR parent to newly added Itemz.
            // Then we find all desendents of Repository which is nothing but existing Itemz(s). 

            var childItemzHierarchyRecords = await _context.ItemzHierarchy!
                    .AsNoTracking()
                    .Where(ih => ih.ItemzHierarchyId!.GetAncestor(1) == newRootHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!)
                    .OrderBy(ih => ih.ItemzHierarchyId!)
                    .ToListAsync();

            if (childItemzHierarchyRecords.Count() == 0)
            {
                movingItemzHierarchyRecord!.ItemzHierarchyId = newRootHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!
                    .GetDescendant(null, null);
            }
            else
            {
                if (atBottomOfChildNodes)
                {

                    movingItemzHierarchyRecord!.ItemzHierarchyId = newRootHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!
                                        .GetDescendant(childItemzHierarchyRecords.LastOrDefault()!.ItemzHierarchyId
                                                       , null);
                }
                else
                {
                    movingItemzHierarchyRecord!.ItemzHierarchyId = newRootHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!
                    .GetDescendant(null
                                    , childItemzHierarchyRecords.FirstOrDefault()!.ItemzHierarchyId);
                }
            }

            var newItemzHierarchyIdString = movingItemzHierarchyRecord!.ItemzHierarchyId!.ToString();

            foreach (var descendentItemzHierarchyRecord in allDescendentItemzHierarchyRecord)
            {
                Regex oldValueRegEx = new Regex(originalItemzHierarchyIdString);
                descendentItemzHierarchyRecord.ItemzHierarchyId = HierarchyId.Parse(
                    (oldValueRegEx.Replace((descendentItemzHierarchyRecord!
                                            .ItemzHierarchyId!.ToString())
                                            , newItemzHierarchyIdString
                                            , 1)
                    )
                );
            }

            // EXPLANATION :: Now that ItemzType has been moved to another project,
            // we should update ItemzType to Project One to One relationship data as well.

            var found_it = await _context.ItemzTypes!.Where(it => it.Id == movingItemzTypeId).ToListAsync();
            if (found_it.Any())
            {
                foreach (var it in found_it)
                {
                    found_it.FirstOrDefault()!.ProjectId = targetProjectId;
                }
            }
        }

    }
}
