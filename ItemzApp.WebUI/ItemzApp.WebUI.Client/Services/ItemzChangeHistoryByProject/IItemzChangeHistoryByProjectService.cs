using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.ItemzChangeHistoryByProjectService
{
    public interface IItemzChangeHistoryByProjectService
	{
		public Task<int> __DELETE_Itemz_Change_History_By_Project_GUID_ID__Async(DeleteChangeHistoryDTO body);

		public Task<int> __GET_Number_of_ItemzChangeHistory_By_Project_Upto_DateTime__Async(GetNumberOfChangeHistoryDTO body);

		public Task<int> __GET_Number_of_ItemzChangeHistory_By_Project__Async(Guid projectId);

	}
}
