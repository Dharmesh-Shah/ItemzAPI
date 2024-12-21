using MudBlazor;

namespace ItemzApp.WebUI.Components.EventServices
{
    public class BreadcrumsService
	{

		public event Func<bool> OnRequestIsOrphanStatus;

        // Method to request for Orphan status
        public bool RequestIsOrphanStatus() 
        {
			return OnRequestIsOrphanStatus.Invoke(); 
        }
    }
}



