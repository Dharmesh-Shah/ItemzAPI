using ItemzApp.WebUI.Client.SharedModels;

namespace ItemzApp.WebUI.Client.Services.BaselineItemzTrace
{
    public interface IBaselineItemzTraceService
	{
		public Task<BaselineItemzTraceDTO> __GET_Check_Baseline_Itemz_Trace_Exists__Async(Guid? fromTraceBaselineItemzId, Guid? toTraceBaselineItemzId);

		public Task<ICollection<BaselineItemzTraceDTO>> __GET_Baseline_Itemz_Traces_By_BaselineItemzID__Async(Guid baselineItemzId);

		public Task<BaselineItemzParentAndChildTraceDTO> __GET_All_Parent_and_Child_Baseline_Itemz_Traces_By_BaselineItemzID__Async(Guid baselineItemzId);

		public Task<int> __GET_From_BaselineItemz_Trace_Count_By_BaselineItemzID__Async(Guid baselineItemzId);

		public Task<int> __GET_To_BaselineItemz_Trace_Count_By_BaselineItemzID__Async(Guid baselineItemzId);

		public Task<int> __GET_All_From_and_To_Traces_Count_By_BaselineItemzID__Async(Guid baselineItemzId);

	}
}
