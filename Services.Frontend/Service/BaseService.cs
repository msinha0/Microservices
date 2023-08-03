﻿using Newtonsoft.Json;
using Services.Frontend.Models;
using Services.Frontend.Service.IService;
using System.Net;
using System.Text;
using static Services.Frontend.Utility.SD;

namespace Services.Frontend.Service
{
	public class BaseService : IBaseService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public BaseService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
		{
			HttpClient client = _httpClientFactory.CreateClient("EcomAPI");
			HttpRequestMessage message = new();

			message.Headers.Add("Accept", "application/json");
			message.RequestUri = new Uri(requestDto.Url);

			if (requestDto.Data != null)
			{
				message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
			}

			HttpResponseMessage? apiResponse = null;

			switch (requestDto.ApiType)
			{
				case ApiType.POST:
					message.Method = HttpMethod.Post;
					break;
				case ApiType.DELETE:
					message.Method = HttpMethod.Delete;
					break;
				case ApiType.PUT:
					message.Method = HttpMethod.Put;
					break;
				default:
					message.Method = HttpMethod.Get;
					break;
			}

			apiResponse = await client.SendAsync(message);

			try
			{
				switch (apiResponse.StatusCode)
				{
					case HttpStatusCode.NotFound:
						return new() { IsSuccess = false, Message = "Not Found" };
					case HttpStatusCode.Forbidden:
						return new() { IsSuccess = false, Message = "Access Denied" };
					case HttpStatusCode.Unauthorized:
						return new() { IsSuccess = false, Message = "Unauthorized" };
					case HttpStatusCode.InternalServerError:
						return new() { IsSuccess = false, Message = "Internal Server Error" };
					default:
						var apiContent = await apiResponse.Content.ReadAsStringAsync();
						var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
						return apiResponseDto;
				}
			}
			catch (Exception ex)
			{
				var dto = new ResponseDto
				{
					Message = ex.Message.ToString(),
					IsSuccess = false
				};
				return dto;
			}

		}
	}
}
