using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.ItemzChangeHistory
{
    public interface IItemzChangeHistoryService
	{
		public Task<ICollection<GetItemzChangeHistoryDTO>> __GET_ItemzChangeHistory_By_GUID_ItemzID__Async(Guid itemzId);

		public Task<int> __DELETE_ItemzChangeHistory_By_GUID_ID__Async(DeleteChangeHistoryDTO body);

	}
}
