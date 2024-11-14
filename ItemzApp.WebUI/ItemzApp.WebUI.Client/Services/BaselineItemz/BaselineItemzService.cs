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

namespace ItemzApp.WebUI.Client.Services.BaselineItemz
{
	public class BaselineItemzService : IBaselineItemzService
	{
		private readonly HttpClient _httpClient;

		public BaselineItemzService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		#region __Single_BaselineItemz_By_GUID_ID__Async
		public async Task<GetBaselineItemzDTO> __Single_BaselineItemz_By_GUID_ID__Async(Guid baselineItemzId)
		{
			return await __Single_BaselineItemz_By_GUID_ID__Async(baselineItemzId, CancellationToken.None);
		}
		public async Task<GetBaselineItemzDTO> __Single_BaselineItemz_By_GUID_ID__Async(Guid baselineItemzId, CancellationToken cancellationToken)
		{
			try
			{

				if (baselineItemzId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(baselineItemzId) + "is required for which value is not provided");
				}
				// TODO :: Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/BaselineItemz/{baselineItemzId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<GetBaselineItemzDTO>($"/api/BaselineItemz/{baselineItemzId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion

		#region __GET_BaselineItemzs_By_Itemz__Async
		public async Task<ICollection<GetBaselineItemzDTO>> __GET_BaselineItemzs_By_Itemz__Async(Guid itemzId)
		{
			return await __GET_BaselineItemzs_By_Itemz__Async(itemzId, CancellationToken.None);
		}
		public async Task<ICollection<GetBaselineItemzDTO>> __GET_BaselineItemzs_By_Itemz__Async(Guid itemzId, CancellationToken cancellationToken)
		{
			try
			{

				if (itemzId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(itemzId) + "is required for which value is not provided");
				}
				// TODO :: Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/BaselineItemz/GetBaselineItemzs/{itemzId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<ICollection<GetBaselineItemzDTO>>($"/api/BaselineItemz/GetBaselineItemzs/{itemzId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion

		#region __GET_BaselineItemz_Count_By_ItemzId__Async
		public async Task<int> __GET_BaselineItemz_Count_By_ItemzId__Async(Guid itemzId)
		{
			return await __GET_BaselineItemz_Count_By_ItemzId__Async(itemzId, CancellationToken.None);
		}

		public async Task<int> __GET_BaselineItemz_Count_By_ItemzId__Async(Guid itemzId, CancellationToken cancellationToken)
		{
			try
			{
				if (itemzId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(itemzId) + "is required for which value is not provided");
				}
				// TODO :: Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/BaselineItemz/GetBaselineItemzCount/{itemzId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>($"/api/BaselineItemz/GetBaselineItemzCount/{itemzId.ToString()}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}
		#endregion

		//#region __GET_BaselineItemz_Collection_By_GUID_IDS__Async
		//public async Task<ICollection<GetBaselineItemzDTO>> __GET_BaselineItemz_Collection_By_GUID_IDS__Async(IEnumerable<Guid> baselineItemzids)
		//{
		//	return await __GET_BaselineItemz_Collection_By_GUID_IDS__Async(baselineItemzids, CancellationToken.None);
		//}
		//public async Task<ICollection<GetBaselineItemzDTO>> __GET_BaselineItemz_Collection_By_GUID_IDS__Async(IEnumerable<Guid> baselineItemzids, CancellationToken cancellationToken)
		//{
		//	try
		//	{
		//		// TODO :: Utilize urlBuilder which is commented below.

		//		if (!baselineItemzids?.Any() ?? true)
		//		{
		//			throw new ArgumentNullException(nameof(baselineItemzids) + "is required for which value is not provided");
		//		}
		//		var urlBuilder_ = new System.Text.StringBuilder();
		//		urlBuilder_.Append("/api/BaselineItemzCollection/(");
		//		// urlBuilder_.Append('(');
		//		for (var i = 0; i < baselineItemzids!.Count(); i++)
		//		{
		//			if (i > 0) urlBuilder_.Append(',');
		//			urlBuilder_.Append((baselineItemzids!.ElementAt(i).ToString()));
		//		}
		//		urlBuilder_.Append(')');


		//		var response = await _httpClient.GetFromJsonAsync<IEnumerable<GetBaselineItemzDTO>>($"{urlBuilder_}", cancellationToken);

		//		return response!.ToList();
		//	}
		//	catch (Exception)
		//	{
		//	}
		//	return default;

		//}

		//#endregion

		#region __PUT_Update_BaselineItemzs_By_GUID_IDs__Async
		public async Task __PUT_Update_BaselineItemzs_By_GUID_IDs__Async(UpdateBaselineItemzDTO updateBaselineItemzDTO)
		{
			await __PUT_Update_BaselineItemzs_By_GUID_IDs__Async(updateBaselineItemzDTO, CancellationToken.None);
		}

		public async Task __PUT_Update_BaselineItemzs_By_GUID_IDs__Async(UpdateBaselineItemzDTO updateBaselineItemzDTO, CancellationToken cancellationToken)
		{
			try
			{
				// TODO::Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/BaselineItemz");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.PutAsJsonAsync($"/api/BaselineItemz", updateBaselineItemzDTO, cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();
				string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

			}
			catch (Exception)
			{
			}
		}
		#endregion

	}
}