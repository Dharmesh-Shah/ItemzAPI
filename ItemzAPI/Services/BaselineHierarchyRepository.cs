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
            baselineHierarchyIdRecordDetails.Name = foundBaselineHierarchyRecord.FirstOrDefault()!.Name ?? "";
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

			// EXPLANATION : Here we are getting record where Baseline Hierarchy ID is equal to the Baseline Hierarchy Id of immediate parent. 
			// We find immediate parent by using GetAncestor(1) method on found Baseline hierarchy record.

			var parentBaselineHierarchyRecord = _context.BaselineItemzHierarchy!
				.AsNoTracking()
				.Where(bih => bih.BaselineItemzHierarchyId == foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!.GetAncestor(1))
				.FirstOrDefault();

			if (parentBaselineHierarchyRecord != null)
			{
				baselineHierarchyIdRecordDetails.ParentRecordId = parentBaselineHierarchyRecord.Id;
				baselineHierarchyIdRecordDetails.ParentRecordType = parentBaselineHierarchyRecord.RecordType;
				baselineHierarchyIdRecordDetails.ParentBaselineHierarchyId = parentBaselineHierarchyRecord.BaselineItemzHierarchyId!.ToString();
				baselineHierarchyIdRecordDetails.ParentLevel = parentBaselineHierarchyRecord.BaselineItemzHierarchyId!.GetLevel();
                baselineHierarchyIdRecordDetails.ParentName = parentBaselineHierarchyRecord.Name ?? "";
			}
			return baselineHierarchyIdRecordDetails;
        }


		public async Task<IEnumerable<BaselineHierarchyIdRecordDetailsDTO?>> GetImmediateChildrenOfBaselineItemzHierarchy(Guid recordId)
		{
			if (recordId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(recordId));
			}

			var foundBaselineHierarchyRecord = _context.BaselineItemzHierarchy!.AsNoTracking()
							.Where(ih => ih.Id == recordId);


			if (foundBaselineHierarchyRecord.Count() != 1) 
			{
				//if (!foundBaselineHierarchyRecord.Any())
				//{
				//	return default;
				//}
				return default;
				// EXPLANATION:: WE COMMENTED OUT FOLLOWING EXPECTION THROWING CODE 
				//				 BECAUSE IT'S POSSIBLE THAT PROJECT MIGHT NOT HAVE ANY BASELINE IN IT. 

				//throw new ApplicationException($"Expected 1 Baseline Hierarchy record to be found " +
				//	$"but instead found {foundBaselineHierarchyRecord.Count()} records for ID {recordId} " +
				//	"Please contact your System Administrator.");
			}

			var foundBaselineHierarchyRecordLevel = foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId.GetLevel();


			// EXPLANATION : We are using SQL Server HierarchyID field type. Now we can use EF Core special
			// methods to query for all Decendents as per below. By adding clause to check for GetLevel which is less then
			// CurrentBaselineHierarchyRecordLevel + three, we get Baseline Hierarchy record itself
			// plus two more deeper level of baseline hierarchy records.
			// The 1st Level data of the record itself is ignored and then 2nd level data is the actual child records.
			// While third level data are used for calculating number of children for child records.

			var baselineItemzTypeHierarchyItemzs = await _context.BaselineItemzHierarchy!
					.AsNoTracking()
					.Where(bih => bih.BaselineItemzHierarchyId!.IsDescendantOf(foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!)
						&& bih.BaselineItemzHierarchyId.GetLevel() < (foundBaselineHierarchyRecordLevel + 3))
					.OrderBy(bih => bih.BaselineItemzHierarchyId!)
					.ToListAsync();

			if (baselineItemzTypeHierarchyItemzs.Count() < 2) // Less then 2 because we may have project entry but no baseline below it.
			{
				return default;
			}

			List<BaselineHierarchyIdRecordDetailsDTO> returningRecords = [];
			BaselineHierarchyIdRecordDetailsDTO baselineHierarchyIdRecordDetails = new();
			string? _localTopChildBaselineHierarchyId = null;
			int _localNumerOfChildNodes = 0;


			for (var i = 0; i < baselineItemzTypeHierarchyItemzs.Count(); i++)
			{
				if (baselineItemzTypeHierarchyItemzs[i].BaselineItemzHierarchyId!.GetLevel() == (foundBaselineHierarchyRecordLevel + 1))
				{
					if (i == 1) // Because i = 0 is the baseline hierarchy record for passed in recordId parameter itself. So we check for i == 1 as first child record.
					{
						baselineHierarchyIdRecordDetails.RecordId = baselineItemzTypeHierarchyItemzs[i].Id;
						baselineHierarchyIdRecordDetails.Name = baselineItemzTypeHierarchyItemzs[i].Name ?? "";
						baselineHierarchyIdRecordDetails.BaselineHierarchyId = baselineItemzTypeHierarchyItemzs[i].BaselineItemzHierarchyId!.ToString();
						baselineHierarchyIdRecordDetails.RecordType = baselineItemzTypeHierarchyItemzs[i].RecordType;
						baselineHierarchyIdRecordDetails.Level = baselineItemzTypeHierarchyItemzs[i].BaselineItemzHierarchyId!.GetLevel();
						baselineHierarchyIdRecordDetails.SourceHierarchyId = (baselineItemzTypeHierarchyItemzs[i].SourceItemzHierarchyId != null)
																							? baselineItemzTypeHierarchyItemzs[i].SourceItemzHierarchyId!.ToString()
																							: "";
						baselineHierarchyIdRecordDetails.IsIncluded = baselineItemzTypeHierarchyItemzs[i].isIncluded;

						// EXPLANATION :: Now add Parent Details which is nothing but foundHierarchyRecord
						baselineHierarchyIdRecordDetails.ParentRecordId = foundBaselineHierarchyRecord.FirstOrDefault()!.Id;
						baselineHierarchyIdRecordDetails.ParentRecordType = foundBaselineHierarchyRecord.FirstOrDefault()!.RecordType;
						baselineHierarchyIdRecordDetails.ParentBaselineHierarchyId = foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!.ToString();
						baselineHierarchyIdRecordDetails.ParentLevel = foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!.GetLevel();
						baselineHierarchyIdRecordDetails.ParentName = foundBaselineHierarchyRecord.FirstOrDefault()!.Name ?? "";
					}
					else
					{
						// WE HAVE TO FINISH WORKING ON PREVIOUS RECORD AND START PROCESSING NEXT ONE
						// IF NUMBER OF CHILD RECORDS ARE GREATER THEN ZERO i.e. ANY CHILD RECORDS FOUND THEN 
						// We have to capture BottomChildHierarchyId and NumberOfChildNodes values.
						if (_localNumerOfChildNodes > 0)
						{
							// hierarchyIdRecordDetails.TopChildHierarchyId = _localTopChildHierarchyId;
							baselineHierarchyIdRecordDetails.BottomChildBaselineHierarchyId = baselineItemzTypeHierarchyItemzs[i - 1].BaselineItemzHierarchyId!.ToString();
							baselineHierarchyIdRecordDetails.NumberOfChildNodes = _localNumerOfChildNodes;
							_localNumerOfChildNodes = 0; // RESET 
						}

						returningRecords.Add(baselineHierarchyIdRecordDetails);

						// RESET hierarchyIdRecordDetails AND START CAPTURING DETAILS OF THE NEXT CHILD RECORD

						baselineHierarchyIdRecordDetails = new();
						baselineHierarchyIdRecordDetails.RecordId = baselineItemzTypeHierarchyItemzs[i].Id;
						baselineHierarchyIdRecordDetails.Name = baselineItemzTypeHierarchyItemzs[i].Name ?? "";
						baselineHierarchyIdRecordDetails.BaselineHierarchyId = baselineItemzTypeHierarchyItemzs[i].BaselineItemzHierarchyId!.ToString();
						baselineHierarchyIdRecordDetails.RecordType = baselineItemzTypeHierarchyItemzs[i].RecordType;
						baselineHierarchyIdRecordDetails.Level = baselineItemzTypeHierarchyItemzs[i].BaselineItemzHierarchyId!.GetLevel();
						baselineHierarchyIdRecordDetails.SourceHierarchyId = (baselineItemzTypeHierarchyItemzs[i].SourceItemzHierarchyId != null)
																							? baselineItemzTypeHierarchyItemzs[i].SourceItemzHierarchyId!.ToString()
																							: "";


						baselineHierarchyIdRecordDetails.IsIncluded = baselineItemzTypeHierarchyItemzs[i].isIncluded;


						// EXPLANATION :: Now add Parent Details which is nothing but foundHierarchyRecord
						baselineHierarchyIdRecordDetails.ParentRecordId = foundBaselineHierarchyRecord.FirstOrDefault()!.Id;
						baselineHierarchyIdRecordDetails.ParentRecordType = foundBaselineHierarchyRecord.FirstOrDefault()!.RecordType;
						baselineHierarchyIdRecordDetails.ParentBaselineHierarchyId = foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!.ToString();
						baselineHierarchyIdRecordDetails.ParentLevel = foundBaselineHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!.GetLevel();
						baselineHierarchyIdRecordDetails.ParentName = foundBaselineHierarchyRecord.FirstOrDefault()!.Name ?? "";
					}

				}
				else if (baselineItemzTypeHierarchyItemzs[i].BaselineItemzHierarchyId.GetLevel() == (foundBaselineHierarchyRecordLevel + 2))
				{
					if (_localNumerOfChildNodes == 0)
					{
						baselineHierarchyIdRecordDetails.TopChildBaselineHierarchyId = baselineItemzTypeHierarchyItemzs[i].BaselineItemzHierarchyId!.ToString();
					}
					_localNumerOfChildNodes = _localNumerOfChildNodes + 1;
				}
			}

			// ADD FINAL hierarchyIdRecordDetails TO THE COLLECTION.
			if (_localNumerOfChildNodes > 0)
			{
				baselineHierarchyIdRecordDetails.BottomChildBaselineHierarchyId = baselineItemzTypeHierarchyItemzs[(baselineItemzTypeHierarchyItemzs.Count() - 1)].BaselineItemzHierarchyId!.ToString();
				baselineHierarchyIdRecordDetails.NumberOfChildNodes = _localNumerOfChildNodes;
			}
			returningRecords.Add(baselineHierarchyIdRecordDetails);

			return returningRecords;
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

		public async Task<bool> UpdateBaselineHierarchyRecordNameByID(Guid recordId, string name)
		{
			if (recordId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(recordId));
			}

			var foundHierarchyRecord = _context.BaselineItemzHierarchy!
							.Where(bih => bih.Id == recordId);

			if (foundHierarchyRecord.Count() != 1)
			{
				throw new ApplicationException($"Expected 1 Baseline Hierarchy record to be found " +
					$"but instead found {foundHierarchyRecord.Count()} records for ID {recordId}" +
					"Please contact your System Administrator.");
			}

			if (foundHierarchyRecord.FirstOrDefault()!.RecordType.ToLower() == "repository") // TODO :: Use Constants instead of Text
			{
				throw new ApplicationException($"Can not update name of the Repository Root Baseline Hierarchy Record with ID {recordId}");
			}
			if (foundHierarchyRecord.FirstOrDefault()!.BaselineItemzHierarchyId!.GetLevel() == 0) // TODO :: Use Constants instead of Text
			{
				throw new ApplicationException($"Can not update name of the Repository Root Baseline Hierarchy Record with ID {recordId}");
			}

			foundHierarchyRecord.FirstOrDefault()!.Name = name; // TODO :: Remove special characters from name variable before saving it to DB. Security Reason.

			return (await _context.SaveChangesAsync() >= 0);
		}


	}
}
