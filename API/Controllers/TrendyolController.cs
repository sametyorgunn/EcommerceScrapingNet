using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrendyolController : ControllerBase
    {
        private readonly ITrendyolService _trendyolService;
        private readonly ICategoryService _categoryService;

        public TrendyolController(ITrendyolService trendyolService, ICategoryService categoryService)
        {
            _trendyolService = trendyolService;
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTrendyolCategories()
        {
            var categories = await _trendyolService.GetTrendyolCategoriesAsync();
            return Ok(categories);
        }
        [HttpPost("GetProductsAndComments")]
        public async Task<IActionResult> GetProductAndComments(GetProductAndCommentsDto request)
        {
            var productAndComments = await _trendyolService.GetProductAndCommentsAsync(request);
            return Ok(productAndComments);
        }
        [HttpGet("UpdateTrendyolCategories")]
        public async Task<IActionResult> UpdateTrendyolCategories()
        {
            var result = await _categoryService.UpdateTrendyolCategories();
            return Ok(result);
        }
    }
}
