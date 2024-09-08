using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace UI.Controllers
{
    public class TrendyolController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
		private readonly HttpClient _httpclient;

		public TrendyolController(ICategoryService categoryService, IProductService productService, HttpClient httpclient)
		{
			_categoryService = categoryService;
			_productService = productService;
			_httpclient = httpclient;
		}

		public IActionResult Index()
        {
			return View();
        }
		[HttpPost]
		public async Task<IActionResult> ScrapeProduct(GetProductAndCommentsDto request)
		{
			using var httpClient = new HttpClient();

			var json = JsonSerializer.Serialize(request);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7260/api/Trendyol/ScrapeProductTrendyol", content);

			response.EnsureSuccessStatusCode();

			string responseBody = await response.Content.ReadAsStringAsync();
			if(responseBody == "true")
			{
                return Ok(true);
			}
			else
			{
                return Ok(false);
            }
        }
		public async Task<IActionResult> Categories(int page = 1, int pageSize = 10)
		{
			return View();
		}
		public async Task<IActionResult> CategoriesUpdate()
		{
			using var httpClient = new HttpClient();

			HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7260/api/Trendyol/UpdateTrendyolCategories");

			response.EnsureSuccessStatusCode();

			string responseBody = await response.Content.ReadAsStringAsync();
			if (responseBody == "true")
			{
				return Ok(true);
			}
			else
			{
				return Ok(false);
			}
		}
    }
}
