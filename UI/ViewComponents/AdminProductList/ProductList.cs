using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace UI.ViewComponents.AdminProductList
{
	public class ProductList:ViewComponent
	{
		private readonly IProductService _productService;
		private readonly HttpClient _httpclient;

		public ProductList(IProductService productService, HttpClient httpclient)
		{
			_productService = productService;
			_httpclient = httpclient;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			try
			{
				HttpResponseMessage response = await _httpclient.GetAsync("https://localhost:7260/api/Product/GetProductsOfTrendyol");
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				var products = JsonSerializer.Deserialize<List<ProductDto>>(responseBody, options);
				return View(products);
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine("\nException Caught!");
				Console.WriteLine("Message :{0} ", e.Message);

				return View(new List<ProductDto>());
			}
		}
	}
}
