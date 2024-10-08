﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
    public class HierarchyRepository : IHierarchyRepository, IDisposable
    {
        private readonly ItemzContext _context;

        public HierarchyRepository(ItemzContext context)
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

        public async Task<HierarchyIdRecordDetailsDTO?> GetHierarchyRecordDetailsByID(Guid recordId)
        {
            if (recordId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(recordId));
            }

            var foundHierarchyRecord = _context.ItemzHierarchy!.AsNoTracking()
                            .Where(ih => ih.Id == recordId);

            if (foundHierarchyRecord.Count() != 1)
            {
                throw new ApplicationException($"Expected 1 Hierarchy record to be found " +
                    $"but instead found {foundHierarchyRecord.Count()} records for ID {recordId}" +
                    "Please contact your System Administrator.");
            }
            
            
            var hierarchyIdRecordDetails = new HierarchyIdRecordDetailsDTO();
            hierarchyIdRecordDetails.RecordId = recordId;
            hierarchyIdRecordDetails.HierarchyId = foundHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!.ToString();
            hierarchyIdRecordDetails.RecordType = foundHierarchyRecord.FirstOrDefault()!.RecordType;
            hierarchyIdRecordDetails.Level = foundHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!.GetLevel();

            // EXPLANATION : We are using SQL Server HierarchyID field type. Now we can use EF Core special
            // methods to query for all Decendents as per below. We are actually finding all Decendents by saying
            // First find the ItemzHierarchy record where ID matches RootItemz ID. This is expected to be the
            // repository ID itself which is the root. then we find all desendents of Repository which is nothing but Project(s). 

            var itemzTypeHierarchyItemz = await _context.ItemzHierarchy!
                    .AsNoTracking()
                    .Where(ih => ih.ItemzHierarchyId!.GetAncestor(1) == foundHierarchyRecord.FirstOrDefault()!.ItemzHierarchyId!)
                    .OrderBy(ih => ih.ItemzHierarchyId!)
                    .ToListAsync();


            if (itemzTypeHierarchyItemz.Count != 0)
            {
                hierarchyIdRecordDetails.TopChildHierarchyId = itemzTypeHierarchyItemz.FirstOrDefault()!.ItemzHierarchyId!.ToString();
                hierarchyIdRecordDetails.BottomChildHierarchyId = itemzTypeHierarchyItemz.LastOrDefault()!.ItemzHierarchyId!.ToString();
                hierarchyIdRecordDetails.NumberOfChildNodes = itemzTypeHierarchyItemz.Count;

            }
            return hierarchyIdRecordDetails;
           
        }
    }
}
