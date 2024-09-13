using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly HttpClient _httpclient;

		public ProductController(HttpClient httpclient)
		{
			_httpclient = httpclient;
		}

		public async Task<IActionResult> DeleteProduct(int id)
		{
			try
			{
				string url = $"https://localhost:7260/api/Product/DeleteProduct/{id}";
				HttpResponseMessage response = await _httpclient.DeleteAsync(url);
				return RedirectToAction("Index", "Trendyol");
			}
			catch (HttpRequestException e)
			{
				Console.WriteLine($"İstek yapılırken bir hata oluştu: {e.Message}");
				return RedirectToAction("Index", "Trendyol");
			}

		}
	}
}
