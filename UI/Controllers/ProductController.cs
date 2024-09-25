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

		public async Task<IActionResult> Index(int id)
		{
			var products =await _productService.GetProductsByCategoryId(new GetProductByFilterDto
			{
				CategoryId = id
			});
			return View(products);
		}
	}
}
