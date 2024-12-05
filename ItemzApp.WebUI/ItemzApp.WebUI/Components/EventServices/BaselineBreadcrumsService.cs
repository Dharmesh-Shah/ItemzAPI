using MudBlazor;

namespace ItemzApp.WebUI.Components.EventServices
{
    public class BaselineBreadcrumsService
	{

		public event Func<bool> OnRequestParentNodeIsIncluded;

        // Method to request parent node IsIncuded status
        public bool RequestParentNodeIsIncluded() 
        {
			return OnRequestParentNodeIsIncluded.Invoke(); 
        }
    }
}



