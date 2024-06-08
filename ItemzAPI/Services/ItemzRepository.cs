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
using Microsoft.Data.SqlClient;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ItemzApp.API.Services
{

    public class ItemzRepository : IItemzRepository, IDisposable
    {
        private readonly ItemzContext _context;
        private readonly IPropertyMappingService _propertyMappingService;
        public ItemzRepository(ItemzContext context,
            IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public async Task<Itemz?> GetItemzAsync(Guid ItemzId)
        {

            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }

            return await _context.Itemzs!
                .Where(c => c.Id == ItemzId).AsNoTracking().FirstOrDefaultAsync();
            
            // EXPLAINATION: It is possible to return Itemz data with details
            // about FromItemzJoinItemzTrace + ToItemzJoinItemzTrace + ItemzTypeJoinItemz together 
            // as per below option.
            
            //return await _context.Itemzs!
            //    .Include(i => i.FromItemzJoinItemzTrace)
            //    .Include(i => i.ToItemzJoinItemzTrace)
            //    .Include(i => i.ItemzTypeJoinItemz)
            //    .Where(c => c.Id == ItemzId).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Itemz?> GetItemzForUpdatingAsync(Guid ItemzId)
        {

            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }

            return await _context.Itemzs!
                .Where(c => c.Id == ItemzId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Itemz>> GetItemzsAsync(IEnumerable<Guid> itemzIds)
        {
            if (itemzIds == null)
            {
                throw new ArgumentNullException(nameof(itemzIds));
            }

            return await _context.Itemzs.AsNoTracking().Where(a => itemzIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToListAsync();
        }
        public PagedList<Itemz>? GetItemzs(ItemzResourceParameter itemzResourceParameter)
        {
            // TODO: Should we check for itemzResourceParameter being null?
            // There are chances that we just want to get all the itemz and
            // consumer of the API might now pass in necessary values for pagging.

            if (itemzResourceParameter == null)
            {
                throw new ArgumentNullException(nameof(itemzResourceParameter));
            }
            try
            {
                if (_context.Itemzs!.Count<Itemz>() > 0) //TODO: await and use CountAsync
                {
                    var itemzCollection = _context.Itemzs!.AsQueryable<Itemz>(); // as IQueryable<Itemz>;

                    if (!string.IsNullOrWhiteSpace(itemzResourceParameter.OrderBy))
                    {
                        var itemzPropertyMappingDictionary =
                                               _propertyMappingService.GetPropertyMapping<Models.GetItemzDTO, Itemz>();

                        itemzCollection = itemzCollection.ApplySort(itemzResourceParameter.OrderBy,
                            itemzPropertyMappingDictionary).AsNoTracking();
                    }

                    // EXPLANATION: Pagging feature should be implemented at the end 
                    // just before calling ToList. This will make sure that any filtering,
                    // sorting, grouping, etc. that we implement on the data are 
                    // put in place before calling ToList. 

                    return PagedList<Itemz>.Create(itemzCollection,
                        itemzResourceParameter.PageNumber,
                        itemzResourceParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                // TODO: It's not good that we capture Generic Exception and then 
                // return null here. Basically, I wanted to check if we have 
                // itemzs returned from the DB and if it does not then
                // it should simply return null back to the calling function.
                // One has to learn how to do this gracefully as part of Entity Framework 
                return null;
            }
        }

        public PagedList<Itemz>? GetOrphanItemzs(ItemzResourceParameter itemzResourceParameter)
        {
            // TODO: Should we check for itemzResourceParameter being null?
            // There are chances that we just want to get all the itemz and
            // consumer of the API might now pass in necessary values for pagging.

            if (itemzResourceParameter == null)
            {
                throw new ArgumentNullException(nameof(itemzResourceParameter));
            }
            try
            {
                if (_context.Itemzs!.Count<Itemz>() > 0) //TODO: await and use CountAsync
                {
                    var itemzCollection = _context.Itemzs
                        .Include(i => i.ItemzTypeJoinItemz)
                        .Where (i => i.ItemzTypeJoinItemz!.Count() == 0)
                        .AsQueryable<Itemz>(); // as IQueryable<Itemz>;

                    if (!string.IsNullOrWhiteSpace(itemzResourceParameter.OrderBy))
                    {
                        var itemzPropertyMappingDictionary =
                                               _propertyMappingService.GetPropertyMapping<Models.GetItemzDTO, Itemz>();

                        itemzCollection = itemzCollection.ApplySort(itemzResourceParameter.OrderBy,
                            itemzPropertyMappingDictionary).AsNoTracking();
                    }

                    // EXPLANATION: Pagging feature should be implemented at the end 
                    // just before calling ToList. This will make sure that any filtering,
                    // sorting, grouping, etc. that we implement on the data are 
                    // put in place before calling ToList. 

                    return PagedList<Itemz>.Create(itemzCollection,
                        itemzResourceParameter.PageNumber,
                        itemzResourceParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                // TODO: It's not good that we capture Generic Exception and then 
                // return null here. Basically, I wanted to check if we have 
                // itemzs returned from the DB and if it does not then
                // it should simply return null back to the calling function.
                // One has to learn how to do this gracefully as part of Entity Framework 
                return null;
            }
        }

        public async Task<int> GetOrphanItemzsCount()
        {
            var foundOrphanItemzsCount = -1;
            foundOrphanItemzsCount = await _context.Itemzs
                        .Include(i => i.ItemzTypeJoinItemz)
                        .Where(i => i.ItemzTypeJoinItemz!.Count() == 0)
                        .CountAsync();
            return foundOrphanItemzsCount > 0 ? foundOrphanItemzsCount : -1;
        }

        public async Task<int> GetItemzsCountByItemzType(Guid itemzTypeId)
        {
            return await _context.Itemzs
                      .Include(i => i.ItemzTypeJoinItemz)
                      .Where(i => i.ItemzTypeJoinItemz!.Any(itji => itji.ItemzTypeId == itemzTypeId)).CountAsync();
        }

        public PagedList<Itemz>? GetItemzsByItemzType(Guid itemzTypeId, ItemzResourceParameter itemzResourceParameter)
        {
            // TODO: Should we check for itemzResourceParameter being null?
            // There are chances that we just want to get all the itemz and
            // consumer of the API might now pass in necessary values for pagging.

            if (itemzResourceParameter == null)
            {
                throw new ArgumentNullException(nameof(itemzResourceParameter));
            }

            if (itemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTypeId));
            }
            try
            {
                if (_context.Itemzs!.Count<Itemz>() > 0)
                {
                    var itemzCollection = _context.Itemzs
                        .Include(i => i.ItemzTypeJoinItemz)
                        //                        .ThenInclude(PjI => PjI.ItemzType)
                        .Where(i => i.ItemzTypeJoinItemz!.Any(itji => itji.ItemzTypeId == itemzTypeId));

                    //     .Where(i => i.  . AsQueryable<Itemz>(); // as IQueryable<Itemz>;

                    if (!string.IsNullOrWhiteSpace(itemzResourceParameter.OrderBy))
                    {
                        var itemzPropertyMappingDictionary =
                                               _propertyMappingService.GetPropertyMapping<Models.GetItemzDTO, Itemz>();

                        itemzCollection = itemzCollection.ApplySort(itemzResourceParameter.OrderBy,
                            itemzPropertyMappingDictionary).AsNoTracking();
                    }

                    // EXPLANATION: Pagging feature should be implemented at the end 
                    // just before calling ToList. This will make sure that any filtering,
                    // sorting, grouping, etc. that we implement on the data are 
                    // put in place before calling ToList. 

                    return PagedList<Itemz>.Create(itemzCollection,
                        itemzResourceParameter.PageNumber,
                        itemzResourceParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                // TODO: It's not good that we capture Generic Exception and then 
                // return null here. Basically, I wanted to check if we have 
                // itemzs returned from the DB and if it does not then
                // it should simply return null back to the calling function.
                // One has to learn how to do this gracefully as part of Entity Framework 
                return null;
            }
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

        public void AddItemz(Itemz itemz)
        {
            if (itemz == null)
            {
                throw new ArgumentNullException(nameof(itemz));
            }
            _context.Itemzs!.Add(itemz);
        }

        /// <summary>
        /// Purpose of this method is to add new Itemz under parent ItemzID which is passed in as parameter
        /// It adds new Itemz at the end of the existing list of child Itemz under supplied parent ItemzId
        /// </summary>
        /// <param name="parentItemzId"></param>
        /// <param name="newlyAddedItemzId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>

        public async Task AddNewItemzHierarchyAsync(Guid parentItemzId, Guid newlyAddedItemzId)
        {
            if (parentItemzId == Guid.Empty )
            {
                throw new ArgumentNullException(nameof(parentItemzId));
            }

            if ( newlyAddedItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(newlyAddedItemzId));
            }

            var rootItemz = _context.ItemzHierarchy!.AsNoTracking()
                            .Where(ih => ih.Id == parentItemzId);

            if (rootItemz.Count() != 1)
            {
                throw new ApplicationException("Either no Parent Itemz Hierarchy record was " +
                    "found OR multiple Parent Itemz Hierarchy records were found in the system");
            }

            // EXPLANATION : We are using SQL Server HierarchyID field type. Now we can use EF Core special
            // methods to query for all Decendents as per below. We are actually finding all Decendents by saying
            // First find the ItemzHierarchy record where ID matches Parent Itemz ID. This is expected to be the
            // Parent Itemz ID itself which is the root OR parent to newly added Itemz.
            // Then we find all desendents of Parent Itemz which is nothing but existing Itemz(s). 

            var parentItemzHierarchyChildRecords = await _context.ItemzHierarchy!
                    .AsNoTracking()
                    .Where(ih => ih.ItemzHierarchyId!.GetAncestor(1) == rootItemz.FirstOrDefault()!.ItemzHierarchyId!)
                    .OrderByDescending(ih => ih.ItemzHierarchyId!)
                    .ToListAsync();

            var tempItemzHierarchy = new Entities.ItemzHierarchy
            {
                Id = newlyAddedItemzId,
                RecordType = "Itemz",
                ItemzHierarchyId = rootItemz.FirstOrDefault()!.ItemzHierarchyId!
                                    .GetDescendant(parentItemzHierarchyChildRecords.Count() > 0
                                                        ? parentItemzHierarchyChildRecords.FirstOrDefault()!.ItemzHierarchyId
                                                        : null
                                                   , null),
            };
            _context.ItemzHierarchy!.Add(tempItemzHierarchy);
        }

        public async Task AddNewItemzBetweenTwoHierarchyRecordsAsync(Guid between1stItemzId, Guid between2ndItemzId,  Guid newlyAddedItemzId)
        {
            if (between1stItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(between1stItemzId));
            }
            if (between2ndItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(between2ndItemzId));
            }

            if (newlyAddedItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(newlyAddedItemzId));
            }


            var tempFirstItemz = _context.ItemzHierarchy!.AsNoTracking()
                                        .Where(ih => ih.Id == between1stItemzId);
            if ((tempFirstItemz).Count() != 1)
            {
                throw new ApplicationException("For Between 1st Itemz, Either no hierarchy record was " +
                            "found OR more then one hierarchy record were found in the system");
            }

            if ((tempFirstItemz.FirstOrDefault()!.RecordType != "Itemz" ))
            {
                throw new ApplicationException("Incorrect Record Type for First Itemz. " +
                    "Instead of 'Itemz' it is '" + tempFirstItemz.FirstOrDefault()!.RecordType + "'");
            }

            var tempSecondItemz = _context.ItemzHierarchy!.AsNoTracking()
                                        .Where(ih => ih.Id == between2ndItemzId);
            if ((tempSecondItemz).Count() != 1)
            {
                throw new ApplicationException("For Between 1st Itemz, Either no hierarchy record was " +
                            "found OR more then one hierarchy record were found in the system");
            }

            if ((tempSecondItemz.FirstOrDefault()!.RecordType != "Itemz"))
            {
                throw new ApplicationException("Incorrect Record Type for Second Itemz. " +
                    "Instead of 'Itemz' it is '" + tempSecondItemz.FirstOrDefault()!.RecordType + "'");
            }

            if (tempSecondItemz.FirstOrDefault()!.ItemzHierarchyId < tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId)
            {
                throw new ApplicationException($"1st Itemz HierarchyID Level is '{tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId!.ToString()}' " +
                    $"which is greater then 2nd Itemz Hirarchy ID Level as '{tempSecondItemz.FirstOrDefault()!.ItemzHierarchyId!.ToString()}'. " +
                                   $"Provided 1st Itemz ID is '{tempFirstItemz.FirstOrDefault()!.Id}' and 2nd Itemz ID is '{tempSecondItemz.FirstOrDefault()!.Id}'   ");
            }

            if(!(tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId!.GetAncestor(1) ==
                    tempSecondItemz.FirstOrDefault()!.ItemzHierarchyId!.GetAncestor(1)))
            {
                throw new ApplicationException("Between Itemz do not belong to the same Parent. FirstItemz " +
                    "belongs to Hierarchy ID '" + tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId!.GetAncestor(1)!.ToString() 
                    + "' and SecondItemz belongs to HierarchyID '" + tempSecondItemz.FirstOrDefault()!.ItemzHierarchyId!.GetAncestor(1)!.ToString() + "'!"
                    );
            }

            var gapBetweenLowerAndUpper = _context.ItemzHierarchy!.AsNoTracking()
                .Where(ih => ih.ItemzHierarchyId >= tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId 
                && ih.ItemzHierarchyId <= tempSecondItemz.FirstOrDefault()!.ItemzHierarchyId 
                && ih.ItemzHierarchyId!.GetLevel() == tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId!.GetLevel());

            if ((gapBetweenLowerAndUpper).Count() > 2)
            {
                throw new ApplicationException("1st and 2nd Itemz are not next to each other. " +
                    "Please consider adding new Itemz between two Itemz which are next to each other. " +
                    "Total Itemz found in between 1st and 2nd Itemz are '" + (gapBetweenLowerAndUpper).Count() + "'");
            }

                var tempItemzHierarchy = new Entities.ItemzHierarchy
            {
                Id = newlyAddedItemzId,
                RecordType = "Itemz",
                ItemzHierarchyId = tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId!.GetAncestor(1)!
                    .GetDescendant(tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId
                                        , tempSecondItemz.FirstOrDefault()!.ItemzHierarchyId == tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId
                                         ? HierarchyId.Parse(
                                              localHelperGetMeNextHierarchyIDNumber(tempFirstItemz.FirstOrDefault()!.ItemzHierarchyId!.ToString())
                                           )
                                        // ? HierarchyId.Parse("/3/2/1/1.0.1.2/")
                                        : tempSecondItemz.FirstOrDefault()!.ItemzHierarchyId),
                //ItemzHierarchyId = rootItemz.FirstOrDefault()!.ItemzHierarchyId!
                //                    .GetDescendant(parentItemzHierarchyChildRecords.Count() > 0
                //                                        ? parentItemzHierarchyChildRecords.FirstOrDefault()!.ItemzHierarchyId
                //                                        : null
                //                                   , null),
            };
            await _context.ItemzHierarchy!.AddAsync(tempItemzHierarchy);
        }

        private string? localHelperGetMeNextHierarchyIDNumber(string lowerBoundHierarchyId)
        {
            var lastSlashPosition = lowerBoundHierarchyId.LastIndexOf("/");
            var convertedlowerBoundHierarchyId = lowerBoundHierarchyId.Remove(lastSlashPosition, 1).Insert(lastSlashPosition, ".2/");
            return convertedlowerBoundHierarchyId;
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void AddItemzByItemzType(Itemz itemz, Guid itemzTypeId)
        {
            if (itemz == null)
            {
                throw new ArgumentNullException(nameof(itemz));
            }

            if (itemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTypeId));
            }

            var tempitemzType = _context.ItemzTypes!.Find(itemzTypeId);
            _context.Itemzs!.Add(itemz);
            var itji = new ItemzTypeJoinItemz { Itemz = itemz, ItemzType = tempitemzType };
            _context.ItemzTypeJoinItemz!.Add(itji);

            // TODO: This is where we have to make sure that we add
            //       ITEMZ and ITEMZTYPE hierarchy record in _context.ItemzHierarchy type.















        }














        public async Task AddNewItemzHierarchyAsync(Itemz itemz, Guid itemzTypeId)
        {
            if (itemz == null)
            {
                throw new ArgumentNullException(nameof(itemz));
            }

            if (itemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTypeId));
            }

            var rootItemzTypeHierarchyRecord = _context.ItemzHierarchy!.AsNoTracking()
                            .Where(ih => ih.Id == itemzTypeId);

            if (rootItemzTypeHierarchyRecord.Count() != 1)
            {
                // TODO: Following error can be improved by providing expected Vs actual found records.
                throw new ApplicationException("Either no Root Itemz Type Hierarchy record " +
                    "found OR multiple Root Itemz Type Hierarchy records found in the system");
            }

            // EXPLANATION : We are using SQL Server HierarchyID field type. Now we can use EF Core special
            // methods to query for all Decendents as per below. We are actually finding all Decendents by saying
            // First find the ItemzHierarchy record where ID matches RootItemzType ID. This is expected to be the
            // ItemzType ID itself which is the root OR parent to newly added Itemz.
            // Then we find all desendents of Repository which is nothing but existing Itemz(s). 

            var itemzHierarchyRecords = await _context.ItemzHierarchy!
                    .AsNoTracking()
                    .Where(ih => ih.ItemzHierarchyId!.GetAncestor(1) == rootItemzTypeHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!)
                    .OrderByDescending(ih => ih.ItemzHierarchyId!)
                    .ToListAsync();

            var tempItemzHierarchy = new Entities.ItemzHierarchy
            {
                Id = itemz.Id,
                RecordType = "Itemz",
                ItemzHierarchyId = rootItemzTypeHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!
                                    .GetDescendant(itemzHierarchyRecords.Count() > 0
                                                        ? itemzHierarchyRecords.FirstOrDefault()!.ItemzHierarchyId
                                                        : null
                                                   , null),
            };

            _context.ItemzHierarchy!.Add(tempItemzHierarchy);
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

        public async Task<bool> ItemzTypeExistsAsync(Guid itemzTypeId)
        {
            if (itemzTypeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzTypeId));
            }

            // EXPLANATION: Using ".Any()" instead of ".Find" as explained in method
            // public bool ItemzExists(Guid itemzId)

            return await _context.ItemzTypes.AsNoTracking().AnyAsync(it => it.Id == itemzTypeId);
        }

        public async Task<bool> ItemzTypeItemzExistsAsync(ItemzTypeItemzDTO itemzTypeItemzDTO)
        {
            if (itemzTypeItemzDTO == null)
            {
                throw new ArgumentNullException(nameof(itemzTypeItemzDTO));
            }

            // EXPLANATION: Using ".Any()" instead of ".Find" as explained in method
            // public bool ItemzExists(Guid itemzId)

            return await _context.ItemzTypeJoinItemz.AsNoTracking().AnyAsync(itji => itji.ItemzId == itemzTypeItemzDTO.ItemzId
                                                                && itji.ItemzTypeId == itemzTypeItemzDTO.ItemzTypeId);
        }

        public async Task<bool> IsOrphanedItemzAsync(Guid ItemzId)
        {
            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }

            // EXPLANATION: Using ".Any()" instead of ".Find" as explained in method
            // public bool ItemzExists(Guid itemzId)
            var isItemzFoundInItemzTypeJoinItemzAssociation = await _context.ItemzTypeJoinItemz.AsNoTracking()
                .AnyAsync(itji => itji.ItemzId == ItemzId);

            if (isItemzFoundInItemzTypeJoinItemzAssociation)
            {
                return false;
            }
            return true;

//            return await _context.ItemzTypeJoinItemz.AsNoTracking().AnyAsync(itji => itji.ItemzId == ItemzId);
        }

        public void UpdateItemz(Itemz itemz)
        {
            // Due to Repository Pattern implementation, 
            // there is no code in this implementation.  
        }

        public async Task DeleteItemzAsync(Guid itemzId)
        {
            var sqlParameters = new[]
{
                new SqlParameter
                {
                    ParameterName = "ItemzId",
                    Value = itemzId,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                }
            };

            // Instead of using Itemzs.Remove we are now using Stored
            // procedure because we need to perform some cleanup of 
            // "non-cascade delete" data due to Entity Framework
            // SQL Server limitations when it comes to many-to-many 
            // relationship. 
            // _context.Itemzs!.Remove(itemz);

            var OUTPUT_Success = new SqlParameter
            {
                ParameterName = "OUTPUT_Success",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Bit,
            };

            sqlParameters = sqlParameters.Append(OUTPUT_Success).ToArray();

            var _ = await _context.Database.ExecuteSqlRawAsync(sql: "EXEC userProcDeleteSingleItemzByItemzID @ItemzId, @OUTPUT_Success = @OUTPUT_Success OUT", parameters: sqlParameters);
        }


        public void RemoveItemzFromItemzType(ItemzTypeItemzDTO itemzTypeItemzDTO)
        {
            var itji = _context.ItemzTypeJoinItemz!.Find(itemzTypeItemzDTO.ItemzTypeId, itemzTypeItemzDTO.ItemzId);
            if (itji != null)
            {
                _context.ItemzTypeJoinItemz.Remove(itji);
            }
        }

        public void AssociateItemzToItemzType(ItemzTypeItemzDTO itemzTypeItemzDTO)
        {
            var itji = _context.ItemzTypeJoinItemz!.Find(itemzTypeItemzDTO.ItemzTypeId, itemzTypeItemzDTO.ItemzId);
            if (itji == null)
            {
                var temp_itji = new ItemzTypeJoinItemz
                {
                    ItemzId = itemzTypeItemzDTO.ItemzId,
                    ItemzTypeId = itemzTypeItemzDTO.ItemzTypeId
                };
                _context.ItemzTypeJoinItemz.Add(temp_itji);
            }
        }

        public void MoveItemzFromOneItemzTypeToAnother(ItemzTypeItemzDTO sourceItemzTypeItemzDTO, ItemzTypeItemzDTO targetItemzTypeItemzDTO)
        {
            // EXPLANATION: Fow now best thing to do would be to remove unwanted itemz and itemzType association 
            // and then find target  association and if not found then simply add it. 
            // This method should be used for moving one itemz at a time. If one would like to move
            // multiple items (i.e. 100s of them in bulk) then this method of updating one record at a time
            // is not very efficient. We will have to come-up with alternative option for 
            // Bulk updating multiple itemz and itemzType association. 

            RemoveItemzFromItemzType(sourceItemzTypeItemzDTO);
            AssociateItemzToItemzType(targetItemzTypeItemzDTO);
        }

    }
}
