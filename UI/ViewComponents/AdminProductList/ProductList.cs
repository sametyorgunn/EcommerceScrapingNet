using BusinessLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewComponents.AdminProductList
{
	public class ProductList:ViewComponent
	{
		private readonly IProductService _productService;

		public ProductList(IProductService productService)
		{
			_productService = productService;
		}

		public IViewComponentResult Invoke()
		{
			var product = _productService.GetListAsync().Result;
			return View(product);
		}
	}
}
