namespace ItemzApp.WebUI.Components.EventServices
{
    public class TreeNodeItemzSelectionService
    {
        public event Action<Guid> OnTreeNodeItemzSelected;
        public event Action<Guid, string> OnTreeNodeItemzNameUpdated;
		public event Action<Guid> OnScrollToTreeViewNode;

		public void SelectTreeNodeItemz(Guid recordId)
        {
            OnTreeNodeItemzSelected?.Invoke(recordId);
        }

        public void UpdateTreeNodeItemzName(Guid recordId, string newName)
        {
            OnTreeNodeItemzNameUpdated?.Invoke(recordId, newName);
        }

		public void ScrollToTreeViewNode(Guid recordId)
		{
			OnScrollToTreeViewNode?.Invoke(recordId);
		}
	}
}



