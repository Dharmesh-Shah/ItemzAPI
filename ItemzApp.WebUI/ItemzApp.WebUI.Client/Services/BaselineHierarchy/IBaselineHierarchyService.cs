using System.Threading.Tasks;
using System.Collections.Generic;
using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.BaselineHierarchyService
{
    public interface IBaselineHierarchyService
	{ 
		public Task<BaselineHierarchyIdRecordDetailsDTO> __Get_BaselineHierarchy_Record_Details_By_GUID__Async(Guid recordId);

		public Task<bool> __Get_VerifyParentChild_BreakdownStructure__Async(Guid? parentId, Guid? childId);

	}
}
