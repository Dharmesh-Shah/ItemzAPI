using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.BaselineItemzCollection
{
    public interface IBaselineItemzCollectionService
	{
		public Task<ICollection<GetBaselineItemzDTO>> __GET_BaselineItemz_Collection_By_GUID_IDS__Async(IEnumerable<System.Guid> baselineItemzids);
	}
}
