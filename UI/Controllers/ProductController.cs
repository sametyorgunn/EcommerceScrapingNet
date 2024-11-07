using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto.Product;
using EntityLayer.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using static OpenQA.Selenium.PrintOptions;

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
		public async Task<IActionResult> ProductDetail(int id, int pageNumber = 1, int pageSize = 5)
		{
			var product = await _productService.GetProductWithCommentAndProperties(new GetProductByFilterDto
			{
				Id = id
			});
			var comments = _commentService.GetCommentByPrediction(new EntityLayer.Dto.ResponseDto.CommentDto { ProductId = id });
			product.GroupsComment = comments.Result;
		
			return View(product);
		}
		//[HttpGet("yorum/{id}")]
		//public async Task<IActionResult> CommentPagination(int id, int pageNumber = 1, int pageSize = 5)
		//{
		//	var product = await _productService.GetProductWithCommentAndProperties(new GetProductByFilterDto
		//	{
		//		Id = id
		//	});
		//	var comments = _commentService.GetCommentByPrediction(new EntityLayer.Dto.ResponseDto.CommentDto { ProductId = id });
		//	product.GroupsComment = comments.Result;

		//	return Json(new { product, totalCount = comments.Result.Count });
		//}
	}
}
