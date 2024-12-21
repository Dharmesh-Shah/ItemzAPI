namespace ItemzApp.WebUI.Components.FindServices
{
    public interface IFindProjectAndBaselineIdsByBaselineItemzIdService
    {
        Task<(Guid ProjectId, Guid BaselineId)> GetProjectAndBaselineId(Guid baselineItemzId);
    }
}
