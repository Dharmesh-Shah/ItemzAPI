using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.BaselineItemzService
{
    public interface IBaselineItemzService
	{
		public Task<GetBaselineItemzDTO> __Single_BaselineItemz_By_GUID_ID__Async(Guid baselineItemzId);

		public Task<ICollection<GetBaselineItemzDTO>> __GET_BaselineItemzs_By_Itemz__Async(Guid itemzId);

		public Task<int> __GET_BaselineItemz_Count_By_ItemzId__Async(Guid itemzId);

		public Task<ICollection<GetBaselineItemzDTO>> __GET_BaselineItemz_Collection_By_GUID_IDS__Async(IEnumerable<System.Guid> baselineItemzids);

		public Task __PUT_Update_BaselineItemzs_By_GUID_IDs__Async(UpdateBaselineItemzDTO body);
	}
}
