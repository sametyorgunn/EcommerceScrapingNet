using BusinessLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.ViewComponents.CategoryListPage
{
	public class CategoryListAll:ViewComponent
	{
		private readonly ICategoryService _categoryService;

		public CategoryListAll(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = await _categoryService.GetListAsync();
			return View(categories);
		}
	}
}
