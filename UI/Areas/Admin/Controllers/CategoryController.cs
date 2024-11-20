using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto.Category;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Attiribute;
namespace UI.Areas.Admin.Controllers
{
	[LoginControlAttiribute]
	[Area("Admin")]
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
        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryDto dto)
        {
            var result =await _categoryService.AddCategory(new CategoryDto
            {
                Name = dto.CategoryName,
                ParentId = dto.CategoryId
            });
            return Ok(result);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.TGetByIdAsync(id);
            var result =  _categoryService.TDeleteAsync(category);
            return RedirectToAction("Index","Category");
        }
    }
}
