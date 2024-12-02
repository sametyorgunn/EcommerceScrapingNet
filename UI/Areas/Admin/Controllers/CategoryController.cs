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
        private readonly IN11Service _n11Service;
        private readonly ITrendyolService _trendyolService;

		public CategoryController(ICategoryService categoryService, IN11Service n11Service, ITrendyolService trendyolService)
		{
			_categoryService = categoryService;
			_n11Service = n11Service;
			_trendyolService = trendyolService;
		}
		public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int categoryID)
        {
            var categories = await _categoryService.GetSubCategories(new GetCategoriesByFilterDto
            {
                Id = categoryID
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
        [HttpPost]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.TGetByIdAsync(id);
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryDto categorydto)
        {
            await _categoryService.TUpdateAsync(categorydto);
            return Ok();
        }
        [HttpGet]
        public async Task <IActionResult> N11CategoryUpdate()
        {
            await _n11Service.N11CategoryUpdate();
            return Ok();
        }
		[HttpGet]
		public async Task<IActionResult> TrendyolCategoryUpdate()
		{
			await _trendyolService.TrendyolCategoryUpdate();
			return Ok();
		}
	}
}
