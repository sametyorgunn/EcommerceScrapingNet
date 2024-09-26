using BusinessLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.ViewComponents.CategoryListAdmin
{
    public class CategoryListForDropdown : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryListForDropdown(ICategoryService categoryService)
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
