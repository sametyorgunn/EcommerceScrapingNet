using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessLayer.Managers.TrendyolManager;
using System.Reflection.Metadata;
using OpenQA.Selenium.Interactions;
using AutoMapper;
using System.Xml.Linq;

namespace BusinessLayer.Managers
{
    public class N11Manager : IN11Service
    {
        private readonly IProductService _productService;
		private readonly IMapper _mapper;
		private readonly IEmotinalAnalysis _emotinalAnalyseService;

		public N11Manager(IProductService productService, IMapper mapper, IEmotinalAnalysis emotinalAnalyseService)
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
                driver.Navigate().GoToUrl("https://www.n11.com/");
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				var searchInput = driver.FindElement(By.Id("searchData"));
                searchInput.Clear();
                searchInput.SendKeys(request.ProductName);
                searchInput.SendKeys(Keys.Enter);
				Thread.Sleep(1000);

				OverlayControl(driver);
				var ScrapeProduct = driver.FindElements(By.CssSelector("li.column")).Take(1).ToList();

				var ProductId = driver.FindElement(By.ClassName("plink")).GetAttribute("data-id");
				var ProductName = driver.FindElement(By.CssSelector("h3.productName")).Text;
				//var ProductRating = driver.FindElement(By.CssSelector("strong.ratingScore")).Text;
				var ProductPrice = driver.FindElement(By.CssSelector("div.priceContainer ins")).Text;
				var ProductImage = driver.FindElement(By.CssSelector("img.cardImage")).GetAttribute("src");
				var ProductLink = driver.FindElement(By.CssSelector("div.pro a")).GetAttribute("href");
				//GetCategories(driver, request.ProductName);
                ProductDto productDto = new ProductDto
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
					var ProdID = Sp.FindElement(By.ClassName("plink")).GetAttribute("data-id");
					var Link = Sp.FindElement(By.CssSelector("div.pro a")).GetAttribute("href");

					OverlayControl(driver);
					string originalWindow = driver.CurrentWindowHandle;

					Actions newTabAction = new Actions(driver);
					newTabAction.KeyDown(Keys.Control).Click(Sp.FindElement(By.CssSelector("div.pro a"))).KeyUp(Keys.Control).Perform();
					var windowHandles = driver.WindowHandles;
                    OverlayControl(driver);
                    wait.Until(d => d.WindowHandles.Count > 1);
					Thread.Sleep(1000);
					driver.SwitchTo().Window(windowHandles[1]);
					Thread.Sleep(1000);
					IList<IWebElement> Comments = driver.FindElements(By.ClassName("comment"));
					foreach (var comment in Comments)
					{
						var a = comment.FindElement(By.CssSelector("p")).Text;
						comments.Add(new CommentDto { CommentText = a, ProductId = productDto.Id, ProductLink = Link,ProductPlatformID = ProdID.ToString() });
					}
					driver.Close();
					driver.SwitchTo().Window(originalWindow);
				}
				var analyse = await _emotinalAnalyseService.GetEmotionalAnalysis(comments);
				var res = _mapper.Map<List<CommentDto>>(analyse);
                productDto.Comment = res;
				var result = await _productService.CreateProduct(productDto);
				
				return new ScrapingResponseDto {Description = "Başarılı", ProductId = result.Id,Status = "True" };
            }
        }
		public async void OverlayControl(IWebDriver driver)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
			var overlay = driver.FindElements(By.Id("dengage-push-perm-slide"));

			if (overlay.Count > 0)
			{
				try
				{
					var close = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("dn-slide-deny-btn")));
					Thread.Sleep(1000);
					close.Click();
				}
				catch (WebDriverTimeoutException)
				{
					Console.WriteLine("Kapatma butonu zamanında tıklanabilir olmadı.");
				}
			}
			var layout = driver.FindElements(By.TagName("efilli-layout-dynamic"));
			if (layout.Count() > 0)
			{
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript(@"
                var element = document.querySelector('efilli-layout-dynamic');
                if (element) {
                    element.remove();
                }
            ");
            }
			
		}
		public async void GetCategories(IWebDriver driver,string ProdName)
		{
			var categoriesFilter = driver.FindElements(By.ClassName("filter"))
				.Where(x => x.GetAttribute("data-tag-name") == "Model").ToList();
			var FilterList = categoriesFilter.Select(x => x.FindElement(By.ClassName("filterList")));

			List<IWebElement> catNames = FilterList
				.SelectMany(x => x.FindElements(By.CssSelector("div.filterItem"))).ToList();
			

			foreach(var category in catNames)
			{
				var catName = category.FindElement(By.ClassName("label")).Text.ToString().ToLower();
				if (catName.Contains(ProdName.ToLower()))
				{
					var checkbox = category.FindElement(By.CssSelector("input.brand-checkbox"));
					checkbox.Click();
				}
			}
			
		}
	}
}
