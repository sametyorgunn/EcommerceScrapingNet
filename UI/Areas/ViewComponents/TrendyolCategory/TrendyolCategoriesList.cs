using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace UI.Areas.Admin.ViewComponents.TrendyolCategory
{
	public class TrendyolCategoriesList:ViewComponent
	{
		private readonly HttpClient _httpclient;
		public TrendyolCategoriesList(HttpClient httpclient)
		{
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
				var products = JsonSerializer.Deserialize<List<CategoryDto>>(responseBody, options);
				return View(products);
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
