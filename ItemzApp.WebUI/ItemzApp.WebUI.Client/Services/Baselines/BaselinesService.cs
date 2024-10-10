﻿using ItemzApp.WebUI.Client.SharedModels;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace ItemzApp.WebUI.Client.Services.BaselinesService
{
	public class BaselinesService : IBaselinesService
	{
		private readonly HttpClient _httpClient;

		public BaselinesService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		#region __Single_Baseline_By_GUID_ID__Async
		public async Task<GetBaselineDTO> __Single_Baseline_By_GUID_ID__Async(Guid baselineId)
		{
			return await __Single_Baseline_By_GUID_ID__Async(baselineId, CancellationToken.None);
		}

		public async Task<GetBaselineDTO> __Single_Baseline_By_GUID_ID__Async(Guid baselineId, CancellationToken cancellationToken)
		{

			try
			{
				if (baselineId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(baselineId) + "is required for which value is not provided");
				}

				// TODO::Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/{baselineId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<GetBaselineDTO>($"/api/Baselines/{baselineId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion

		#region __GET_Baselines_Collection__Async
		public async Task<ICollection<GetBaselineDTO>> __GET_Baselines_Collection__Async()
		{
			return await __GET_Baselines_Collection__Async(CancellationToken.None);
		}
		public async Task<ICollection<GetBaselineDTO>> __GET_Baselines_Collection__Async(CancellationToken cancellationToken)
		{
			try
			{
				// TODO::Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<ICollection<GetBaselineDTO>>($"/api/Baselines", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion

		#region __POST_Create_Baseline__Async
		public async Task<GetBaselineDTO> __POST_Create_Baseline__Async(CreateBaselineDTO createBaselineDTO)
		{
			return await __POST_Create_Baseline__Async(createBaselineDTO, CancellationToken.None);
		}
		public async Task<GetBaselineDTO> __POST_Create_Baseline__Async(CreateBaselineDTO createBaselineDTO, CancellationToken cancellationToken)
		{
			try
			{
				// TODO :: Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append("/api/Baselines");
				//urlBuilder_.Append('?');
				//if (createBaselineDTO == null)
				//{
				//	throw new ArgumentNullException(nameof(createBaselineDTO) + "is required for which value is not provided");
				//}

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.PostAsJsonAsync($"/api/Baselines", createBaselineDTO, cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();

				//string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
				string responseContent = httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).Result;

				// EXPLANATION :: HERE WE ARE SERIALIZING JSON RESPONSE INTO DESIRED CLASS / OBJECT FORMAT FOR RETURNING
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				};
				var response = JsonSerializer.Deserialize<GetBaselineDTO>(responseContent, options);
				return (response ?? default);
			}
			catch (Exception)
			{
			}
			return default;
		}

		#endregion

		#region __POST_Clone_Baseline__Async
		public async Task<GetBaselineDTO> __POST_Clone_Baseline__Async(CloneBaselineDTO cloneBaselineDTO)
		{
			return await __POST_Clone_Baseline__Async(cloneBaselineDTO, CancellationToken.None);
		}

		public async Task<GetBaselineDTO> __POST_Clone_Baseline__Async(CloneBaselineDTO cloneBaselineDTO, CancellationToken cancellationToken)
		{
			try
			{
				// TODO :: Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/CloneBaseline");
				//urlBuilder_.Append('?');
				//if (cloneBaselineDTO == null)
				//{
				//	throw new ArgumentNullException(nameof(cloneBaselineDTO) + "is required for which value is not provided");
				//}

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.PostAsJsonAsync($"/api/Baselines/CloneBaseline", cloneBaselineDTO, cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();

				//string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
				string responseContent = httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).Result;

				// EXPLANATION :: HERE WE ARE SERIALIZING JSON RESPONSE INTO DESIRED CLASS / OBJECT FORMAT FOR RETURNING
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				};
				var response = JsonSerializer.Deserialize<GetBaselineDTO>(responseContent, options);
				return (response ?? default);
			}
			catch (Exception)
			{
			}
			return default;
		}

		#endregion

		#region __PUT_Update_Baseline_By_GUID_ID__Async
		public async Task __PUT_Update_Baseline_By_GUID_ID__Async(Guid baselineId, UpdateBaselineDTO updateBaselineDTO)
		{
			await __PUT_Update_Baseline_By_GUID_ID__Async(baselineId, updateBaselineDTO, CancellationToken.None);
		}

		public async Task __PUT_Update_Baseline_By_GUID_ID__Async(Guid baselineId, UpdateBaselineDTO updateBaselineDTO, CancellationToken cancellationToken)
		{
			try
			{
				if (baselineId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(baselineId) + "is required for which value is not provided");
				}

				// TODO::Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/{baselineId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.PutAsJsonAsync($"/api/Baselines/{baselineId.ToString()}", updateBaselineDTO, cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();
				string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

			}
			catch (Exception)
			{
			}
		}

		#endregion


		#region __DELETE_Baseline_By_GUID_ID__Async
		public async Task __DELETE_Baseline_By_GUID_ID__Async(Guid baselineId)
		{
			await __DELETE_Baseline_By_GUID_ID__Async(baselineId, CancellationToken.None);
		}
		public async Task __DELETE_Baseline_By_GUID_ID__Async(Guid baselineId, CancellationToken cancellationToken)
		{
			try
			{
				if (baselineId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(baselineId) + "is required for which value is not provided");
				}

				// TODO::Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/{baselineId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.DeleteAsync($"/api/Baselines/{baselineId.ToString()}", cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();
				string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

			}
			catch (Exception)
			{
			}
		}


		#endregion

		#region __GET_BaselineItemz_Count_By_Baseline__Async
		public async Task<int> __GET_BaselineItemz_Count_By_Baseline__Async(Guid baselineId)
		{
			return await __GET_BaselineItemz_Count_By_Baseline__Async(baselineId, CancellationToken.None);
		}
		public async Task<int> __GET_BaselineItemz_Count_By_Baseline__Async(Guid baselineId, CancellationToken cancellationToken)
		{
			try
			{
				if (baselineId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(baselineId) + "is required for which value is not provided");
				}

				// TODO :: Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/GetBaselineItemzCount/{baselineId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>($"/api/Baselines/GetBaselineItemzCount/{baselineId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}

		#endregion

		#region __GET_BaselineItemz_Trace_Count_By_Baseline__Async
		public async Task<int> __GET_BaselineItemz_Trace_Count_By_Baseline__Async(Guid baselineId)
		{
			return await __GET_BaselineItemz_Trace_Count_By_Baseline__Async(baselineId, CancellationToken.None);
		}

		public async Task<int> __GET_BaselineItemz_Trace_Count_By_Baseline__Async(Guid baselineId, CancellationToken cancellationToken)
		{
			try
			{
				if (baselineId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(baselineId) + "is required for which value is not provided");
				}

				// TODO :: Utilize urlBuilder which is commented below.
				var urlBuilder_ = new System.Text.StringBuilder();
				urlBuilder_.Append($"/api/Baselines/GetBaselineItemzTraceCount/{baselineId.ToString()}");
				urlBuilder_.Append('?');

				urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>($"/api/Baselines/GetBaselineItemzTraceCount/{baselineId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}

		#endregion

		#region __GET_Included_BaselineItemz_Count_By_Baseline__Async

		public async Task<int> __GET_Included_BaselineItemz_Count_By_Baseline__Async(Guid baselineId)
		{
			return await __GET_Included_BaselineItemz_Count_By_Baseline__Async(baselineId, CancellationToken.None);
		}
		public async Task<int> __GET_Included_BaselineItemz_Count_By_Baseline__Async(Guid baselineId, CancellationToken cancellationToken)
		{
			try
			{
				if (baselineId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(baselineId) + "is required for which value is not provided");
				}

				// TODO :: Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/GetIncludedBaselineItemzCount/{baselineId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>($"/api/Baselines/GetIncludedBaselineItemzCount/{baselineId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}

		#endregion

		#region __GET_Baseline_Count_By_Project__Async
		public async Task<int> __GET_Baseline_Count_By_Project__Async(Guid projectId)
		{
			return await __GET_Baseline_Count_By_Project__Async(projectId, CancellationToken.None);
		}

		public async Task<int> __GET_Baseline_Count_By_Project__Async(Guid projectId, CancellationToken cancellationToken)
		{
			try
			{
				if (projectId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(projectId) + "is required for which value is not provided");
				}

				// TODO :: Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/GetBaselineCount/{projectId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>($"/api/Baselines/GetBaselineCount/{projectId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}

		#endregion

		#region __GET_Baselines_By_Project_Id__Async
		public async Task<int> __GET_Baselines_By_Project_Id__Async(Guid projectId)
		{
			return await __GET_Baselines_By_Project_Id__Async(projectId, CancellationToken.None);
		}

		public async Task<int> __GET_Baselines_By_Project_Id__Async(Guid projectId,CancellationToken cancellationToken)
		{
			try
			{
				if (projectId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(projectId) + "is required for which value is not provided");
				}

				// TODO :: Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/GetBaselines/{projectId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>($"/api/Baselines/GetBaselines/{projectId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion

		#region __GET_Excluded_BaselineItemz_Count_By_Baseline__Async
		public async Task<int> __GET_Excluded_BaselineItemz_Count_By_Baseline__Async(Guid baselineId)
		{
			return await __GET_Excluded_BaselineItemz_Count_By_Baseline__Async(baselineId, CancellationToken.None);
		}

		public async Task<int> __GET_Excluded_BaselineItemz_Count_By_Baseline__Async(Guid baselineId, CancellationToken cancellationToken)
		{
			try
			{
				if (baselineId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(baselineId) + "is required for which value is not provided");
				}

				// TODO :: Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/GetExcludedBaselineItemzCount/{baselineId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>($"/api/Baselines/GetExcludedBaselineItemzCount/{baselineId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion

		#region __GET_Orphaned_BaselineItemz_Count__Async
		public async Task<int> __GET_Orphaned_BaselineItemz_Count__Async()
		{
			return await __GET_Orphaned_BaselineItemz_Count__Async(CancellationToken.None);
		}
		public async Task<int> __GET_Orphaned_BaselineItemz_Count__Async(CancellationToken cancellationToken)
		{
			try
			{
				// TODO :: Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/GetOrphanedBaselineItemzCount");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>($"/api/Baselines/GetOrphanedBaselineItemzCount", cancellationToken);
				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion

		#region __GET_Total_BaselineItemz_Count__Async
		public async Task<int> __GET_Total_BaselineItemz_Count__Async()
		{
			return await __GET_Total_BaselineItemz_Count__Async(CancellationToken.None);
		}

		public async Task<int> __GET_Total_BaselineItemz_Count__Async(CancellationToken cancellationToken)
		{
			try
			{
				// TODO :: Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/GetTotalBaselineItemzCount");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>($"/api/Baselines/GetTotalBaselineItemzCount", cancellationToken);
				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion

		#region __GET_BaselineItemzTypes_By_Baseline__Async
		public async Task<ICollection<GetBaselineItemzTypeDTO>> __GET_BaselineItemzTypes_By_Baseline__Async(Guid baselineId)
		{
			return await __GET_BaselineItemzTypes_By_Baseline__Async(baselineId, CancellationToken.None);
		}
		public async Task<ICollection<GetBaselineItemzTypeDTO>> __GET_BaselineItemzTypes_By_Baseline__Async(Guid baselineId, CancellationToken cancellationToken)
		{
			try
			{
				if (baselineId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(baselineId) + "is required for which value is not provided");
				}

				// TODO :: Utilize urlBuilder which is commented below.
				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Baselines/GetBaselineItemzTypes/{baselineId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<ICollection<GetBaselineItemzTypeDTO>>($"/api/Baselines/GetBaselineItemzTypes/{baselineId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion
	}
}