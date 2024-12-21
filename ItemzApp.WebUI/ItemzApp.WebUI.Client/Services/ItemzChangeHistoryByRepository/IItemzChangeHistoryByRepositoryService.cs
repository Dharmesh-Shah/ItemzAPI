using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.ItemzChangeHistoryByRepositoryService
{
    public interface IItemzChangeHistoryByRepositoryService
	{
		public Task<int> __GET_Number_of_ItemzChangeHistory_By_Repository__Async();

		public Task<int> __GET_Number_of_ItemzChangeHistory_By_Repository_Upto_DateTime__Async(GetNumberOfChangeHistoryByRepositoryDTO body);

	}
}
