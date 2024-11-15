﻿using System.Threading.Tasks;
using System.Collections.Generic;
using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.BaselineHierarchy
{
    public interface IBaselineHierarchyService
	{ 
		public Task<BaselineHierarchyIdRecordDetailsDTO> __Get_BaselineHierarchy_Record_Details_By_GUID__Async(Guid recordId);

		public Task<ICollection<BaselineHierarchyIdRecordDetailsDTO>> __Get_Immediate_Children_Baseline_Hierarchy_By_GUID__Async(Guid recordId);

		public Task<bool> __Get_VerifyParentChild_BreakdownStructure__Async(Guid? parentId, Guid? childId);

		public Task<ICollection<NestedBaselineHierarchyIdRecordDetailsDTO>> __Get_All_Children_Baseline_Hierarchy_By_GUID__Async(Guid recordId);

    }
}
