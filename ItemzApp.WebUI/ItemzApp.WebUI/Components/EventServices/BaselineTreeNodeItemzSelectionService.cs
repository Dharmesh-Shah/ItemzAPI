namespace ItemzApp.WebUI.Components.EventServices
{
    public class BaselineTreeNodeItemzSelectionService
	{
        public event Action<Guid> OnBaselineTreeNodeItemzSelected;
        public event Action<Guid, string> OnBaselineTreeNodeItemzNameUpdated;
		public event Action<Guid, bool> OnLoadingOfBaselineItemzTreeViewComponent;
        public event Action<Guid> OnSingleBaselineItemzIsIncludedChanged;
        public event Action<Guid> OnExcludeAllChildrenBaselineItemzTreeNodes;
		public event Action<Guid> OnIncludeAllChildrenBaselineItemzTreeNodes;

		public void SelectBaselineTreeNodeItemz(Guid recordId)
        {
            OnBaselineTreeNodeItemzSelected?.Invoke(recordId);
        }

        public void UpdateBaselineTreeNodeItemzName(Guid recordId, string newName)
        {
            OnBaselineTreeNodeItemzNameUpdated?.Invoke(recordId, newName);
        }

		public void LoadingOfBaselineItemzTreeViewComponent (Guid recordId, bool isIncluded)
		{
            OnLoadingOfBaselineItemzTreeViewComponent?.Invoke(recordId, isIncluded);

        }
        public void SingleBaselineItemzIsIncludedChanged(Guid recordId)
        {
			OnSingleBaselineItemzIsIncludedChanged?.Invoke(recordId);
		}

        public void ExcludeAllChildrenBaselineItemzTreeNodes(Guid recordId)
        {
			OnExcludeAllChildrenBaselineItemzTreeNodes?.Invoke(recordId);

		}
		public void IncludeAllChildrenBaselineItemzTreeNodes(Guid recordId)
		{
			OnIncludeAllChildrenBaselineItemzTreeNodes?.Invoke(recordId);

		}

		// TODO :: In Baseline, we update it's IsIncluded property
		// which will have impact on the BaselineItemzTreeView as we will
		// expect it to change color of the BaselineItemzNode to Red. 
		// This will impact not only a single BaselineItemzNode but all it's child nodes as well.
		// So we will have to come-up with another EventService method that can communicate 
		// change in IsIncluded Property back to BaselineItemzTreeNode to process it accordingly.

	}
}



