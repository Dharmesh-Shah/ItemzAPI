using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.Itemz
{
    public interface IItemzService
    {
		public Task<GetItemzDTO?> __Single_Itemz_By_GUID_ID__Async(Guid itemzId);

		public Task<GetItemzDTO> __POST_Create_Itemz__Async(Guid? parentId, bool? atBottomOfChildNodes, CreateItemzDTO body);

		public Task<ICollection<GetItemzDTO>> __GET_Orphan_Itemzs_Collection__Async(int? pageNumber, int? pageSize, string orderBy);

		public Task<int> __GET_Orphan_Itemzs_Count__Async();

		public Task<GetItemzDTO> __POST_Create_Itemz_Between_Existing_Itemz__Async(Guid? firstItemzId, Guid? secondItemzId, CreateItemzDTO body);

		public Task __POST_Move_Itemz_Between_Existing_Itemz__Async(Guid? movingItemzId, Guid? firstItemzId, Guid? secondItemzId);

		public Task __PUT_Update_Itemz_By_GUID_ID__Async(Guid itemzId, UpdateItemzDTO updateItemzDTO);

		public Task __DELETE_Itemz_By_GUID_ID__Async(Guid itemzId);

		public Task __POST_Move_Itemz__Async(Guid movingItemzId, Guid? targetId, bool? atBottomOfChildNodes);

	}
}
