using ItemzApp.WebUI.Client.SharedModels;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace ItemzApp.WebUI.Client.Services.Itemz
{
	public class ItemzService : IItemzService
	{
		private readonly HttpClient _httpClient;

		public ItemzService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}


		#region __Single_Itemz_By_GUID_ID__Async
		public async Task<GetItemzDTO?> __Single_Itemz_By_GUID_ID__Async(System.Guid itemzId)
		{
			return await __Single_Itemz_By_GUID_ID__Async(itemzId, CancellationToken.None);
		}


		public async Task<GetItemzDTO?> __Single_Itemz_By_GUID_ID__Async(Guid itemzId, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _httpClient.GetFromJsonAsync<GetItemzDTO>($"/api/Itemzs/{itemzId}", cancellationToken);

				return response!;
			}
			catch (Exception)
			{
			}
			return default;
		}

		#endregion

		#region __POST_Create_Itemz__Async
		public async Task<GetItemzDTO?> __POST_Create_Itemz__Async(Guid? parentId, bool? atBottomOfChildNodes, CreateItemzDTO createItemzDTO)
		{
			return await __POST_Create_Itemz__Async(parentId, atBottomOfChildNodes, createItemzDTO, CancellationToken.None);
		}

		public async Task<GetItemzDTO?> __POST_Create_Itemz__Async(Guid? parentId, bool? atBottomOfChildNodes, CreateItemzDTO createItemzDTO, CancellationToken cancellationToken)
		{
			try
			{
				// TODO :: Because parentId and atBottomOfChildNodes are optional parameters we should introduce condition to check for null values in them
				// and accordingly forward request over to API endpoint.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append("/api/Itemzs");
				//urlBuilder_.Append('?');
				//if (parentId != null)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("parentId")).Append('=').Append(System.Uri.EscapeDataString(parentId.ToString())).Append('&');
				//}

				//if ((bool)atBottomOfChildNodes!)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("atBottomOfChildNodes")).Append('=').Append(System.Uri.EscapeDataString("true")).Append('&');
				//}
				//else
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("atBottomOfChildNodes")).Append('=').Append(System.Uri.EscapeDataString("false")).Append('&');
				//}

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.PostAsJsonAsync($"/api/Itemzs?parentId={parentId}&AtBottomOfChildNodes={atBottomOfChildNodes}", createItemzDTO, cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();

				string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
				// TODO :: Send back updated content for GetProjectDTO
				return default;
			}
			catch (Exception)
			{
			}
			return default;
		}
		#endregion


		#region __GET_Orphan_Itemzs_Collection__Async

		public async Task<ICollection<GetItemzDTO>> __GET_Orphan_Itemzs_Collection__Async(int? pageNumber, int? pageSize, string orderBy)
		{
			return await __GET_Orphan_Itemzs_Collection__Async(pageNumber, pageSize, orderBy, CancellationToken.None);
		}
		public async Task<ICollection<GetItemzDTO>> __GET_Orphan_Itemzs_Collection__Async(int? pageNumber, int? pageSize, string orderBy, CancellationToken cancellationToken)
		{
			try
			{

				// TODO :: Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append("/api/Itemzs/GetOrphan");
				//urlBuilder_.Append('?');
				//if (pageNumber != null)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("pageNumber")).Append('=').Append(System.Uri.EscapeDataString(pageNumber.ToString()!)).Append('&');
				//}
				//if (pageSize != null)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("pageSize")).Append('=').Append(System.Uri.EscapeDataString(pageSize.ToString()!)).Append('&');
				//}

				//if (orderBy != null)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("orderBy")).Append('=').Append(System.Uri.EscapeDataString(orderBy)).Append('&');
				//}

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<ICollection<GetItemzDTO>>("/api/Itemzs/GetOrphan", cancellationToken);

				return response!;
			}
			catch (Exception)
			{

			}
			return default;
		}

		#endregion

		#region __GET_Orphan_Itemzs_Count__Async

		public async Task<int> __GET_Orphan_Itemzs_Count__Async()
		{
			return await __GET_Orphan_Itemzs_Count__Async(CancellationToken.None);
		}

		public async Task<int> __GET_Orphan_Itemzs_Count__Async(CancellationToken cancellationToken)
		{
			try
			{

				// TODO :: Utilize urlBuilder_ which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append("/api/Itemzs/GetOrphanCount");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var response = await _httpClient.GetFromJsonAsync<int>("/api/Itemzs/GetOrphanCount", cancellationToken);

				return response!;
			}
			catch (Exception)
			{
			}
			return default;
		}

		#endregion

		#region __POST_Create_Itemz_Between_Existing_Itemz__Async
		public async Task<GetItemzDTO> __POST_Create_Itemz_Between_Existing_Itemz__Async(Guid? firstItemzId, Guid? secondItemzId, CreateItemzDTO createItemzDTO)
		{
			return await __POST_Create_Itemz_Between_Existing_Itemz__Async(firstItemzId, secondItemzId, createItemzDTO, CancellationToken.None);
		}

		public async Task<GetItemzDTO> __POST_Create_Itemz_Between_Existing_Itemz__Async(Guid? firstItemzId, Guid? secondItemzId, CreateItemzDTO createItemzDTO, CancellationToken cancellationToken)
		{

			try
			{
				// TODO :: Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append("/api/Itemzs/CreateItemzBetweenExistingItemz");
				//urlBuilder_.Append('?');
				//if (firstItemzId != Guid.Empty)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("firstItemzId")).Append('=').Append(System.Uri.EscapeDataString(firstItemzId.ToString()!)).Append('&');
				//}
				//if (secondItemzId != Guid.Empty)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("secondItemzId")).Append('=').Append(System.Uri.EscapeDataString(secondItemzId.ToString()!)).Append('&');
				//}

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.PostAsJsonAsync($"/api/Itemzs/CreateItemzBetweenExistingItemz?firstItemzId={firstItemzId.ToString()}&secondItemzId={secondItemzId.ToString()}"
					, createItemzDTO, cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();
				string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

				// EXPLANATION :: HERE WE ARE SERIALIZING JSON RESPONSE INTO DESIRED CLASS / OBJECT FORMAT FOR RETURNING
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				};
				var response = JsonSerializer.Deserialize<GetItemzDTO>(responseContent, options);

				return response!;

			}
			catch (Exception)
			{
			}
			return default;

		}
		#endregion

		#region __POST_Move_Itemz_Between_Existing_Itemz__Async

		public async Task __POST_Move_Itemz_Between_Existing_Itemz__Async(Guid? movingItemzId, Guid? firstItemzId, Guid? secondItemzId)
		{
			await __POST_Move_Itemz_Between_Existing_Itemz__Async(movingItemzId, firstItemzId, secondItemzId, CancellationToken.None);

		}

		public async Task __POST_Move_Itemz_Between_Existing_Itemz__Async(Guid? movingItemzId, Guid? firstItemzId, Guid? secondItemzId, CancellationToken cancellationToken)
		{
			try
			{
				// TODO :: Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append("/api/Itemzs/MoveItemzBetweenExistingItemz");
				//urlBuilder_.Append('?');

				//if(movingItemzId != Guid.Empty)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("movingItemzId")).Append('=').Append(System.Uri.EscapeDataString(movingItemzId.ToString()!)).Append('&');
				//}
				//else
				//{
				//	throw new ArgumentNullException(nameof(movingItemzId) + "is required for which value is not provided");
				//}

				//if (firstItemzId != Guid.Empty)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("firstItemzId")).Append('=').Append(System.Uri.EscapeDataString(firstItemzId.ToString()!)).Append('&');
				//}
				//else
				//{
				//	throw new ArgumentNullException(nameof(firstItemzId) + "is required for which value is not provided");
				//}

				//if (secondItemzId != Guid.Empty)
				//{
				//	urlBuilder_.Append(System.Uri.EscapeDataString("secondItemzId")).Append('=').Append(System.Uri.EscapeDataString(secondItemzId.ToString()!)).Append('&');
				//}
				//else
				//{
				//	throw new ArgumentNullException(nameof(secondItemzId) + "is required for which value is not provided");
				//}

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.PostAsJsonAsync($"/api/Itemzs/MoveItemzBetweenExistingItemz?movingItemzId={movingItemzId.ToString()}&firstItemzId={firstItemzId.ToString()}&secondItemzId={secondItemzId.ToString()}  ", cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();
				string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
				// TODO :: Send back updated content for GetProjectDTO
			}
			catch (Exception)
			{
			}
		}

		#endregion

		#region __PUT_Update_Itemz_By_GUID_ID__Async
		public async Task __PUT_Update_Itemz_By_GUID_ID__Async(Guid itemzId, UpdateItemzDTO updateItemzDTO)
		{
			await __PUT_Update_Itemz_By_GUID_ID__Async(itemzId,updateItemzDTO, CancellationToken.None);
		}

		public async Task __PUT_Update_Itemz_By_GUID_ID__Async(Guid itemzId, UpdateItemzDTO updateItemzDTO, CancellationToken cancellationToken)
		{
			try
			{
				// TODO::Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Itemzs/{itemzId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.PutAsJsonAsync($"/api/Itemzs/{itemzId.ToString()}", updateItemzDTO,  cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();
				string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

			}
			catch (Exception)
			{
			}
		}

		#endregion

		#region __DELETE_Itemz_By_GUID_ID__Async

		public async Task __DELETE_Itemz_By_GUID_ID__Async(Guid itemzId)
		{
			await __DELETE_Itemz_By_GUID_ID__Async(itemzId, CancellationToken.None);
		}

		public async Task __DELETE_Itemz_By_GUID_ID__Async(Guid itemzId, CancellationToken cancellationToken)
		{
			try
			{
				if (itemzId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(itemzId) + "is required for which value is not provided");
				}

				// TODO::Utilize urlBuilder which is commented below.

				//var urlBuilder_ = new System.Text.StringBuilder();
				//urlBuilder_.Append($"/api/Itemzs/{itemzId.ToString()}");
				//urlBuilder_.Append('?');

				//urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.DeleteAsync($"/api/Itemzs/{itemzId.ToString()}", cancellationToken);
				httpResponseMessage.EnsureSuccessStatusCode();
				string responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

			}
			catch (Exception)
			{
			}

		}
		#endregion


		#region __POST_Move_Itemz__Async

		public async Task __POST_Move_Itemz__Async(Guid movingItemzId, Guid? targetId, bool? atBottomOfChildNodes)
		{
			await __POST_Move_Itemz__Async(movingItemzId, targetId, atBottomOfChildNodes, CancellationToken.None);
		}

		public async Task __POST_Move_Itemz__Async(Guid movingItemzId, Guid? targetId, bool? atBottomOfChildNodes, CancellationToken cancellationToken)
		{
			try
			{
				// TODO::Utilize urlBuilder which is commented below.

				var urlBuilder_ = new System.Text.StringBuilder();

				if (movingItemzId == Guid.Empty)
				{
					throw new ArgumentNullException(nameof(movingItemzId) + "is required for which value is not provided");
				}

				urlBuilder_.Append($"/api/Itemzs/{movingItemzId.ToString()}");
				urlBuilder_.Append('?');

				if (targetId != Guid.Empty)
				{
					urlBuilder_.Append(System.Uri.EscapeDataString("targetId")).Append('=').Append(System.Uri.EscapeDataString(targetId.ToString()!)).Append('&');
				}
				else
				{
					throw new ArgumentNullException(nameof(targetId) + "is required for which value is not provided");
				}

				if ((bool)atBottomOfChildNodes!)
				{
					urlBuilder_.Append(System.Uri.EscapeDataString("atBottomOfChildNodes")).Append('=').Append(System.Uri.EscapeDataString("true")).Append('&');
				}
				else
				{
					urlBuilder_.Append(System.Uri.EscapeDataString("atBottomOfChildNodes")).Append('=').Append(System.Uri.EscapeDataString("false")).Append('&');
				}

				urlBuilder_.Length--;

				var httpResponseMessage = await _httpClient.PostAsJsonAsync($"/api/Itemzs/{movingItemzId.ToString()}?targetId={targetId.ToString()}&atBottomOfChildNodes={atBottomOfChildNodes}", cancellationToken);
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