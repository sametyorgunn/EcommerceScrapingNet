using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class TrendyolController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ITrendyolService _trendyolservice;
        private readonly IHbService _hbService;
        private readonly HttpClient _httpclient;

        public TrendyolController(ICategoryService categoryService, IProductService productService, HttpClient httpclient, ITrendyolService trendyolservice, IHbService hbService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _httpclient = httpclient;
            _trendyolservice = trendyolservice;
            _hbService = hbService;
        }

        public IActionResult Index()
        {
			return View();
        }
		[HttpPost]
		public async Task<IActionResult> ScrapeProduct(GetProductAndCommentsDto request)
		{
            var resulthb = await _hbService.GetProductAndCommentsAsync(request);

            var result = await _trendyolservice.GetProductAndCommentsAsync(request);
			return Ok(result);
        }
		public async Task<IActionResult> Categories(int page = 1, int pageSize = 10)
		{
			return View();
		}
		public async Task<IActionResult> CategoriesUpdate()
		{
			var result =await _trendyolservice.UpdateTrendyolCategories();
			if (result)
			{
                return Ok(true);
            }
			else
			{
				return Ok(false);
			}
			
		}

    }
}
