using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.ItemzChangeHistoryByItemzTypeService
{
    public interface IItemzChangeHistoryByItemzTypeService
	{
		public Task<int> __DELETE_Itemz_Change_History_By_ItemzType_GUID_ID__Async(DeleteChangeHistoryDTO body);

		public Task<int> __GET_Number_of_ItemzChangeHistory_By_ItemzType_Upto_DateTime__Async(GetNumberOfChangeHistoryDTO body);

		public Task<int> __GET_Number_of_ItemzChangeHistory_By_ItemzType__Async(Guid itemzTypeId);

	}
}
