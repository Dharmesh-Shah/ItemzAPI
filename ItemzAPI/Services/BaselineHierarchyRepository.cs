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
using System.Linq.Dynamic.Core;
using ItemzApp.API.Models;

namespace ItemzApp.API.Services
{
    public class BaselineHierarchyRepository : IBaselineHierarchyRepository, IDisposable
    {
        private readonly ItemzContext _context;

        public BaselineHierarchyRepository(ItemzContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

        public async Task<BaselineHierarchyIdRecordDetailsDTO?> GetBaselineHierarchyRecordDetailsByID(Guid recordId)
        {
            if (recordId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(recordId));
            }

            var foundBaselineHierarchyRecord = _context.BaselineItemzHierarchy!.AsNoTracking()
                            .Where(bih => bih.Id == recordId);

            if (foundBaselineHierarchyRecord.Count() != 1)
            {
                throw new ApplicationException($"Expected 1 Baseline Hierarchy record to be found " +
                    $"but instead found {foundBaselineHierarchyRecord.Count()} records for ID {recordId} " +
                    "Please contact your System Administrator.");
            }
            
            var baselineHierarchyIdRecordDetails = new BaselineHierarchyIdRecordDetailsDTO();
            baselineHierarchyIdRecordDetails.RecordId = recordId;
            baselineHierarchyIdRecordDetails.BaselineHierarchyId = foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!.ToString();
            baselineHierarchyIdRecordDetails.SourceHierarchyId = (foundBaselineHierarchyRecord.FirstOrDefault()!.SourceItemzHierarchyId != null) 
                                                                    ? foundBaselineHierarchyRecord.FirstOrDefault()!.SourceItemzHierarchyId!.ToString() 
                                                                    : "";
            baselineHierarchyIdRecordDetails.RecordType = foundBaselineHierarchyRecord.FirstOrDefault()!.RecordType;
            baselineHierarchyIdRecordDetails.Level = foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!.GetLevel();
            baselineHierarchyIdRecordDetails.IsIncluded = foundBaselineHierarchyRecord.FirstOrDefault()!.isIncluded;

            // EXPLANATION : We are using SQL Server HierarchyID field type. Now we can use EF Core special
            // methods to query for all Decendents as per below. We are actually finding all Decendents by saying
            // First find the BaselineItemzHierarchy record where ID matches RootItemz ID. This is expected to be the
            // repository ID itself which is the root. then we find all desendents of Repository which is nothing but Project(s). 

            var parentBaselineItemzHierarchyRecord = await _context.BaselineItemzHierarchy!
                    .AsNoTracking()
                    .Where(bih => bih.BaselineItemzHierarchyId!.GetAncestor(1) == foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!)
                    .OrderBy(bih => bih.BaselineItemzHierarchyId!)
                    .ToListAsync();

            if (parentBaselineItemzHierarchyRecord.Count != 0)
            {
                baselineHierarchyIdRecordDetails.TopChildBaselineHierarchyId = parentBaselineItemzHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!.ToString();
                baselineHierarchyIdRecordDetails.BottomChildBaselineHierarchyId = parentBaselineItemzHierarchyRecord.LastOrDefault()!.BaselineItemzHierarchyId!.ToString();
                baselineHierarchyIdRecordDetails.NumberOfChildNodes = parentBaselineItemzHierarchyRecord.Count;

            }
            return baselineHierarchyIdRecordDetails;
        }

        public async Task<bool> CheckIfPartOfSingleBaselineHierarchyBreakdownStructureAsync(Guid parentId, Guid childId) 
        {
            var foundParentId = await _context.BaselineItemzHierarchy!.AsNoTracking()
                .Where(bih => bih.Id == parentId)
                .Where(bih => bih.BaselineItemzHierarchyId!.GetLevel() > 1 ) 
                .FirstOrDefaultAsync();

            if (foundParentId != null)
            {
                var foundChildId =  await _context.BaselineItemzHierarchy!.AsNoTracking()
                    .Where(bih => bih.Id == childId)
                    .Where(bih => bih.BaselineItemzHierarchyId!.IsDescendantOf( foundParentId!.BaselineItemzHierarchyId))
                    .ToListAsync();
                
                if (foundChildId.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
