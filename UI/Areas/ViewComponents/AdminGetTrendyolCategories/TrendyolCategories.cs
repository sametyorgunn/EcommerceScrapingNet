using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace UI.Areas.Admin.ViewComponents.AdminGetTrendyolCategories
{
	public class TrendyolCategories : ViewComponent
	{
		private readonly ICategoryService _categoryService;
		private readonly HttpClient _httpclient;

		public TrendyolCategories(ICategoryService categoryService, HttpClient httpclient)
		{
			_categoryService = categoryService;
			_httpclient = httpclient;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			try
			{
				HttpResponseMessage response = await _httpclient.GetAsync("https://localhost:7260/api/Category/GetTrendyolCategories");
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				var categories = JsonSerializer.Deserialize<List<CategoryDto>>(responseBody, options);
				return View(categories);
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine("\nException Caught!");
				Console.WriteLine("Message :{0} ", e.Message);

				return View(new List<CategoryDto>());
			}
		}
	}
}
