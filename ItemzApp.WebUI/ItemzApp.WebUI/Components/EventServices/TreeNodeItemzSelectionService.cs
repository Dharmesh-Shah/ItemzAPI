namespace ItemzApp.WebUI.Components.EventServices
{
    public class TreeNodeItemzSelectionService
    {
        public event Action<Guid> OnTreeNodeItemzSelected;
        public event Action<Guid, string> OnTreeNodeItemzNameUpdated;

        public void SelectTreeNodeItemz(Guid recordId)
        {
            OnTreeNodeItemzSelected?.Invoke(recordId);
        }

        public void UpdateTreeNodeItemzName(Guid recordId, string newName)
        {
            OnTreeNodeItemzNameUpdated?.Invoke(recordId, newName);
        }
    }
}



