using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}
		[HttpGet("GetProductsOfTrendyol")]
		public async Task<IActionResult> GetProducts()
		{
			var result = await _productService.GetProductsByPlatformId(new GetProductsByPlatformId { Id = (int)Platform.trendyol });
			return Ok(result);
		}
	}
}
