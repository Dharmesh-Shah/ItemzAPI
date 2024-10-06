using ItemzApp.WebUI.Client.Services.Hierarchy;
using ItemzApp.WebUI.Client.SharedModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ItemzApp.WebUI.Client.Services.Hierarchy
{
    public class HierarchyService : IHierarchyService
	{
        private readonly HttpClient _httpClient;

        public HierarchyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

		#region __Get_Hierarchy_Record_Details_By_GUID__Async

		public async Task<HierarchyIdRecordDetailsDTO> __Get_Hierarchy_Record_Details_By_GUID__Async(Guid recordId)
        {
            return await __Get_Hierarchy_Record_Details_By_GUID__Async(recordId, CancellationToken.None);
		}
		public async Task<HierarchyIdRecordDetailsDTO> __Get_Hierarchy_Record_Details_By_GUID__Async(Guid recordId, CancellationToken cancellationToken)
		{
			try
			{
				//TODO::Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Hierarchy/{recordId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.GetFromJsonAsync<HierarchyIdRecordDetailsDTO>($"/api/Hierarchy/{recordId.ToString()}", cancellationToken);
				return httpResponseMessage!;
			}
			catch (Exception)
			{
			}
			return default;
		}

		#endregion


	}
}
