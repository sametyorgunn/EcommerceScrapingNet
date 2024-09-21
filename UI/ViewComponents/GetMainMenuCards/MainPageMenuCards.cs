using BusinessLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace UI.ViewComponents.GetMainMenuCards
{
    public class MainPageMenuCards:ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public MainPageMenuCards(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories =await _categoryService.GetMainCategories();
            return View(categories);
        }
    }
}
