using System.Threading.Tasks;
using System.Collections.Generic;
using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.Hierarchy
{
    public interface IHierarchyService
	{
		public Task<HierarchyIdRecordDetailsDTO> __Get_Hierarchy_Record_Details_By_GUID__Async(Guid recordId);
		
		public Task<HierarchyIdRecordDetailsDTO> __Get_Next_Sibling_Hierarchy_Record_Details_By_GUID__Async(Guid recordId);

		public Task<ICollection<HierarchyIdRecordDetailsDTO>> __Get_Immediate_Children_Hierarchy_By_GUID__Async(Guid recordId);

		public Task<ICollection<NestedHierarchyIdRecordDetailsDTO>> __Get_All_Parents_Hierarchy_By_GUID__Async(Guid recordId);
		
		public Task<ICollection<NestedHierarchyIdRecordDetailsDTO>> __Get_All_Children_Hierarchy_By_GUID__Async(Guid recordId);
	}
}
