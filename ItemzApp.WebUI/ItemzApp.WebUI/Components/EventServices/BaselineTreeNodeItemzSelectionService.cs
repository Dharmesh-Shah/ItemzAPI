namespace ItemzApp.WebUI.Components.EventServices
{
    public class BaselineTreeNodeItemzSelectionService
	{
        public event Action<Guid> OnBaselineTreeNodeItemzSelected;
        public event Action<Guid, string> OnBaselineTreeNodeItemzNameUpdated;

        public void SelectBaselineTreeNodeItemz(Guid recordId)
        {
            OnBaselineTreeNodeItemzSelected?.Invoke(recordId);
        }

        public void UpdateBaselineTreeNodeItemzName(Guid recordId, string newName)
        {
            OnBaselineTreeNodeItemzNameUpdated?.Invoke(recordId, newName);
        }

        // TODO :: In Baseline, we update it's IsIncluded property
        // which will have impact on the BaselineItemzTreeView as we will
        // expect it to change color of the BaselineItemzNode to Red. 
        // This will impact not only a single BaselineItemzNode but all it's child nodes as well.
        // So we will have to come-up with another EventService method that can communicate 
        // change in IsIncluded Property back to BaselineItemzTreeNode to process it accordingly.

    }
}



