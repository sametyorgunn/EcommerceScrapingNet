using BusinessLayer.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrendyolController : ControllerBase
    {
        private readonly ITrendyolService _trendyolService;

        public TrendyolController(ITrendyolService trendyolService)
        {
            _trendyolService = trendyolService;
        }
        [HttpGet]
        public IActionResult GetTrendyolCategories()
        {
            var categories = _trendyolService.GetTrendyolCategories().Result;
            return Ok(categories);
        }
    }
}
