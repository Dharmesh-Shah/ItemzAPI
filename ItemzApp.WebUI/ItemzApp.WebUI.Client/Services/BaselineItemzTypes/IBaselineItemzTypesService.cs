using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.BaselineItemzTypesService
{
    public interface IBaselineItemzTypesService
	{

		public Task<GetBaselineItemzTypeDTO> __Single_BaselineItemzType_By_GUID_ID__Async(Guid baselineItemzTypeId);

		public Task<ICollection<GetBaselineItemzTypeDTO>> __GET_BaselineItemzTypes_Collection__Async();


		public Task<int> __GET_BaselineItemz_Count_By_BaselineItemzType__Async(Guid baselineItemzTypeId);

		public Task<ICollection<GetBaselineItemzDTO>> __GET_BaselineItemzs_By_BaselineItemzType__Async(Guid baselineItemzTypeId, int? pageNumber, int? pageSize, string orderBy);





	}
}
