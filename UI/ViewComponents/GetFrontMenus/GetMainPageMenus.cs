using BusinessLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewComponents.GetFrontMenus
{
	public class GetMainPageMenus:ViewComponent
	{
		private readonly ICategoryService _categoryService;

		public GetMainPageMenus(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public async Task <IViewComponentResult> InvokeAsync()
		{
			var categories =await _categoryService.GetListAsync();
			return View(categories);
		}
	}
}
