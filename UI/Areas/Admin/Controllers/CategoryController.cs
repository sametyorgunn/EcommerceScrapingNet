using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int parentId)
        {
            var categories = await _categoryService.GetSubCategories(new GetCategoriesByFilterDto
            {
                ParentId = parentId
            });
            return Ok(categories);
        }
    }
}
