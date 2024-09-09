using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto.Category;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}
		[HttpGet("GetTrendyolCategories")]
		public IActionResult GetTrendyolCategories()
		{
			GetCategoriesByFilterDto request = new GetCategoriesByFilterDto
			{
				PlatformId = 0
			};
			var result = _categoryService.GetTrendyolCategoriesByPlatform(request).Result;
			return Ok(result);
		}
	}
}
