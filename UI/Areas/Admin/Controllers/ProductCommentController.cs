using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using UI.Areas.Admin.Attiribute;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
    [LoginControlAttiribute]
	public class ProductCommentController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ITrendyolService _trendyolservice;
        private readonly IN11Service _n11Service;
        private readonly IAmazonService _amazonService;
		private readonly HttpClient _httpclient;

		public ProductCommentController(ICategoryService categoryService, IProductService productService, HttpClient httpclient,
			ITrendyolService trendyolservice, IN11Service n11Service, IAmazonService amazonService)
		{
			_categoryService = categoryService;
			_productService = productService;
			_httpclient = httpclient;
			_trendyolservice = trendyolservice;
			_n11Service = n11Service;
			_amazonService = amazonService;
		}
		public IActionResult Index()
        {
			return View();
        }
		[HttpPost]
		public async Task<IActionResult> ScrapeProduct(GetProductAndCommentsDto request)
		{
			var splitProductNames = request.ProductName.Split(',');
			foreach(var req in splitProductNames)
			{
				request.ProductName = req;
				var resultN11 = await _n11Service.GetProductAndCommentsAsync(request);
				request.ProductId = resultN11.ProductId;
				var amazon = _amazonService.GetProductAndCommentsAsync(request);
				var result = await _trendyolservice.GetProductAndCommentsAsync(request);
			}
			return Ok();
		}
		public async Task<IActionResult> Categories(int page = 1, int pageSize = 10)
		{
			return View();
		}
    }
}
