using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace UI.Areas.Admin.ViewComponents.AdminLastFiveProducts
{
	public class GetLastFiveProducts:ViewComponent
	{
		private readonly IProductService _productService;

        public GetLastFiveProducts(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
		{
			var result = await _productService.GetLastFiveProducts();
			return View(result);
		}
	}
}
