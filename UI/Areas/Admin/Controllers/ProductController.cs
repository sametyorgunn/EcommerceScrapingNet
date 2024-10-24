using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = _productService.GetProductById(new GetProductById
			{
				Id = id
			}).Result;
			await _productService.TDeleteAsync(product);
			return RedirectToAction("Index", "ProductComment");
		}
	}
}
