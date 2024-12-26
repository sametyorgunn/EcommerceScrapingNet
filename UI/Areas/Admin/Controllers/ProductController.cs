using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using UI.Areas.Admin.Attiribute;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
    [LoginControlAttiribute]
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
		[HttpPost]
		public async Task<IActionResult> DeleteCheckedProduct(List<int>ids)
		{
			var result =await _productService.DeleteCheckedProducts(ids);
			return Ok();
        }
        public async Task<IActionResult> MakeProductPassive(int id)
		{
			var product = await _productService.GetProductById(new GetProductById
			{
				Id=id
			});
			product.Status = false;
			await _productService.TUpdateAsync(product);
			return RedirectToAction("Index", "ProductComment");
		}
		public async Task<IActionResult> MakeProductActive(int id)
		{
			var product = await _productService.GetProductById(new GetProductById
			{
				Id = id
			});
			product.Status = true;
			await _productService.TUpdateAsync(product);
			return RedirectToAction("Index", "ProductComment");
		}
		public async Task<IActionResult> EditProduct()
		{
			return Ok();
		}
	}
}
