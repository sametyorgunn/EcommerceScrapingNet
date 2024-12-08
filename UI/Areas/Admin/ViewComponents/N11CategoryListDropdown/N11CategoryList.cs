using BusinessLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.ViewComponents.N11CategoryListDropdown
{
    public class N11CategoryList:ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public N11CategoryList(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var n11categories = await _categoryService.GetAllN11Categories();
            return View(n11categories);
        }
    }
}
