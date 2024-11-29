namespace ItemzApp.WebUI.Components.EventServices
{
    public class TreeNodeItemzSelectionService
    {
        public event Action<Guid> OnTreeNodeItemzSelected;

        public void SelectTreeNodeItemz(Guid recordId)
        {
            OnTreeNodeItemzSelected?.Invoke(recordId);
        }
    }
}

