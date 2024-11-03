using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto.Product;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
		private readonly ICommentService _commentService;

		public ProductController(IProductService productService, ICommentService commentService)
		{
			_productService = productService;
			_commentService = commentService;
		}
		public async Task<IActionResult> Index()
		{
			var products = await _productService.GetLastTwelveProduct();
			return View(products);
		}
		[HttpGet("ürün-listesi/{id}")]
		public async Task<IActionResult> Products(int id)
		{
			var products = await _productService.GetProductsByCategoryId(new GetProductByFilterDto
			{
				CategoryId = id
			});
			return View(products);
		}
		[HttpGet("ürün-detay/{id}")]
		public async Task<IActionResult> ProductDetail(int id)
		{
			var product = await _productService.GetProductWithCommentAndProperties(new GetProductByFilterDto
			{
				Id = id
			});
			var comments = _commentService.GetCommentByPrediction(new EntityLayer.Dto.ResponseDto.CommentDto { ProductId = id });
			product.GroupsComment = comments.Result;
			return View(product);
		}
	}
}
