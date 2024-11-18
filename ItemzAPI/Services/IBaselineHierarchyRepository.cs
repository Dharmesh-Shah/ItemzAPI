﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;
using ItemzApp.API.Models;
using ItemzApp.API.Models.BetweenControllerAndRepository;

namespace ItemzApp.API.Services
{
    public interface IBaselineHierarchyRepository
    {
        Task<bool> CheckIfPartOfSingleBaselineHierarchyBreakdownStructureAsync(Guid parentId, Guid childId);

        public Task<BaselineHierarchyIdRecordDetailsDTO?> GetBaselineHierarchyRecordDetailsByID(Guid recordId);

        public Task<IEnumerable<BaselineHierarchyIdRecordDetailsDTO?>> GetImmediateChildrenOfBaselineItemzHierarchy(Guid recordId);

        public Task<RecordCountAndEnumerable<NestedBaselineHierarchyIdRecordDetailsDTO>> GetAllChildrenOfBaselineItemzHierarchy(Guid recordId);

        // public Task<IEnumerable<NestedBaselineHierarchyIdRecordDetailsDTO?>> GetAllChildrenOfBaselineItemzHierarchy(Guid recordId);

        //public Task<IEnumerable<NestedBaselineHierarchyIdRecordDetailsDTO?>> GetAllParentsOfBaselineItemzHierarchy(Guid recordId);

        public Task<RecordCountAndEnumerable<NestedBaselineHierarchyIdRecordDetailsDTO>> GetAllParentsOfBaselineItemzHierarchy(Guid recordId);

		public Task<bool> UpdateBaselineHierarchyRecordNameByID(Guid recordId, string name);

	}
}
