using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class TrendyolController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

		public TrendyolController(ICategoryService categoryService, IProductService productService)
		{
			_categoryService = categoryService;
			_productService = productService;
		}

		public IActionResult Index()
        {
            return View();
        }
	
	}
}
