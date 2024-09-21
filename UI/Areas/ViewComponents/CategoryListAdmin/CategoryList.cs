using BusinessLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.ViewComponents.CategoryListAdmin
{
    public class CategoryList:ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryList(ICategoryService categoryService)
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
