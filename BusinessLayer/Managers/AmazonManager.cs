using AutoMapper;
using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
	public class AmazonManager : IAmazonService
	{
		private readonly IProductService _productService;
		private readonly IMapper _mapper;
		private readonly IEmotinalAnalysis _emotinalAnalyseService;

		public AmazonManager(IProductService productService, IMapper mapper, IEmotinalAnalysis emotinalAnalyseService)
		{
			_productService = productService;
			_mapper = mapper;
			_emotinalAnalyseService = emotinalAnalyseService;
		}

		public async Task<ScrapingResponseDto> GetProductAndCommentsAsync(GetProductAndCommentsDto request)
		{
			var options = new ChromeOptions();
			//options.AddArgument("--headless");
			//options.AddArgument("--disable-gpu");
			//options.AddArgument("--window-size=1920,1080");
			//options.AddArgument("--no-sandbox");
			//options.AddArgument("--disable-dev-shm-usage");


			using (IWebDriver driver = new ChromeDriver(options))
			{
				var jsExecutor = (IJavaScriptExecutor)driver;
				driver.Navigate().GoToUrl("https://www.amazon.com/");
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				var searchInput = driver.FindElement(By.Id("twotabsearchtextbox"));
				searchInput.Clear();
				searchInput.SendKeys(request.ProductName);
				searchInput.SendKeys(Keys.Enter);
				Thread.Sleep(1000);

				//OverlayControl(driver);
				var ScrapeProduct = driver.FindElements(By.ClassName("sg-col-inner")).Take(1).ToList();

				var ProductId = driver.FindElement(By.ClassName("btnBasket")).GetAttribute("data-group-id");
				var ProductName = driver.FindElement(By.CssSelector("a.a-link-normal")).Text;
				//var ProductRating = driver.FindElement(By.CssSelector("strong.ratingScore")).Text;
				var ProductPrice = driver.FindElement(By.CssSelector("span.a-offscreen")).Text;
				var ProductImage = driver.FindElement(By.CssSelector("img.cardImage")).GetAttribute("src");
				var ProductLink = driver.FindElement(By.CssSelector("a.a-link-normal")).GetAttribute("href");

				ProductDto dto = new ProductDto
				{
					ProductId = ProductId,
					CategoryId = request.CategoryId,
					PlatformId = (int)EntityLayer.Enums.Platform.n11,
					ProductBrand = "",
					ProductImage = ProductImage,
					ProductPrice = ProductPrice,
					ProductName = ProductName,
					ProductRating = "4",
					ProductProperty = null,
					Status = true,
					ProductLink = ProductLink,
					Comment = new List<CommentDto>()
				};
				List<CommentDto> comments = new List<CommentDto>();
				foreach (var Sp in ScrapeProduct)
				{
					var Link = Sp.FindElement(By.CssSelector("a.a-link-normal")).GetAttribute("href");

					//OverlayControl(driver);
					string originalWindow = driver.CurrentWindowHandle;

					Actions newTabAction = new Actions(driver);
					newTabAction.KeyDown(Keys.Control).Click(Sp.FindElement(By.ClassName("acrCustomerReviewLink"))).KeyUp(Keys.Control).Perform();
					var windowHandles = driver.WindowHandles;
					wait.Until(d => d.WindowHandles.Count > 1);
					driver.SwitchTo().Window(windowHandles[1]);
					Thread.Sleep(1000);
					IList<IWebElement> Comments = driver.FindElements(By.ClassName("comment"));
					foreach (var comment in Comments)
					{
						var a = comment.FindElement(By.CssSelector("p")).Text;
						comments.Add(new CommentDto { CommentText = a, ProductId = dto.Id, ProductLink = Link });
					}
					driver.Close();
					driver.SwitchTo().Window(originalWindow);
				}
				var analyse = await _emotinalAnalyseService.GetEmotionalAnalysis(comments);

				var res = _mapper.Map<List<CommentDto>>(analyse);
				dto.Comment = res;
				var result = await _productService.CreateProduct(dto);

				return new ScrapingResponseDto { Description = "Başarılı", ProductId = result.Id, Status = "True" };
			}
		}
	}
}
