using BusinessLayer.IServices;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewComponents.AdminGetTrendyolCategories
{
	public class TrendyolCategories:ViewComponent
	{
		private readonly ICategoryService _categoryService;

		public TrendyolCategories(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public IViewComponentResult Invoke()
		{
			var result = _categoryService.GetListByFilterAsync(x => x.Platform == (int)Platform.trendyol).Result;
			return View(result);
		}
	}
}
