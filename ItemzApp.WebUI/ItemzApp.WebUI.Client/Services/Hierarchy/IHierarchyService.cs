using System.Threading.Tasks;
using System.Collections.Generic;
using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.Hierarchy
{
    public interface IHierarchyService
	{
		public Task<HierarchyIdRecordDetailsDTO> __Get_Hierarchy_Record_Details_By_GUID__Async(Guid recordId);
	}
}
