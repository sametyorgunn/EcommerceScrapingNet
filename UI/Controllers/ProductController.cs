using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto.Product;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		public async Task<IActionResult> Index()
		{
			var products = await _productService.GetLastTwelveProduct();
			return View(products);
		}
		[HttpGet("ürün-listesi/{id}")]
		public async Task<IActionResult> Products(int id)
		{
			var products = await _productService.GetProductsByCategoryId(new GetProductByFilterDto
			{
				CategoryId = id
			});
			return View(products);
		}
		[HttpGet("ürün-detay/{id}")]
		public async Task<IActionResult> ProductDetail(int id)
		{
			var product = await _productService.GetProductWithCommentAndProperties(new GetProductByFilterDto
			{
				Id = id
			});
			return View(product);
		}
	}
}
