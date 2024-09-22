using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace UI.Areas.Admin.ViewComponents.AdminProductList
{
	public class ProductList:ViewComponent
	{
		private readonly IProductService _productService;
		public ProductList(IProductService productService)
		{
			_productService = productService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var result = await _productService.GetListAsync();
			return View(result);
		}
	}
}
