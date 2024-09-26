using BusinessLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewComponents.Menu
{
	public class MainPageMenu:ViewComponent
	{
		private readonly ICategoryService _categoryService;

		public MainPageMenu(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories =await _categoryService.GetListAsync();
			return View(categories);
		}
	}
}
