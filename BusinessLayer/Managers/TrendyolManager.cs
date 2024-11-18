using AutoMapper;
using BusinessLayer.IServices;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using PuppeteerSharp;
using SeleniumExtras.WaitHelpers;


namespace BusinessLayer.Managers
{
	public class TrendyolManager : ITrendyolService
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryrepository;
		private readonly ICommentService _commentService;
		private readonly IEmotinalAnalysis _emotinalAnalyseService;
		public TrendyolManager(IProductService productService, IMapper mapper, ICategoryService categoryService,
			ICategoryRepository categoryrepository,
			ICommentService commentService, IEmotinalAnalysis emotinalAnalyseService)
		{
			_productService = productService;
			_mapper = mapper;
			_categoryService = categoryService;
			_categoryrepository = categoryrepository;
			_commentService = commentService;
			_emotinalAnalyseService = emotinalAnalyseService;
		}

		public async Task<ScrapingResponseDto> GetProductAndCommentsAsync(GetProductAndCommentsDto request)
        {

			var options = new ChromeOptions();
			//options.AddArgument("--headless");
			options.AddArgument("--disable-gpu"); 
			options.AddArgument("--window-size=1920,1080");
			options.AddArgument("--no-sandbox");
			options.AddArgument("--disable-dev-shm-usage");

			using (IWebDriver driver = new ChromeDriver(options))
			{
				WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				var jsExecutor = (IJavaScriptExecutor)driver;

				driver.Navigate().GoToUrl("https://www.trendyol.com/");
                var searchInput = driver.FindElement(By.ClassName("V8wbcUhU"));
                searchInput.SendKeys(request.ProductName);
                searchInput.SendKeys(Keys.Enter);
                var ScrapeProduct = driver.FindElements(By.CssSelector("div.p-card-wrppr ")).Take(1).ToList();
                //var ProductId = driver.FindElement(By.ClassName("p-card-wrppr")).GetAttribute("data-id");
    //            var ProductLink = driver.FindElement(By.CssSelector("div.p-card-chldrn-cntnr a")).GetAttribute("href");
				//var ProductBrand = driver.FindElement(By.CssSelector("span.prdct-desc-cntnr-ttl")).Text;
				//var ProductName = driver.FindElement(By.CssSelector("span.prdct-desc-cntnr-name")).Text;

				////var ProductRating = driver.FindElement(By.CssSelector("div.rating-line-count")).Text;
				//var ProductPrice = driver.FindElement(By.CssSelector("div.prc-box-dscntd")).Text;
				//var ProductImage = driver.FindElement(By.CssSelector("img.p-card-img")).GetAttribute("src");
				////var ProductProperties = driver.FindElements(By.ClassName("attribute-item"));
				
				//ProductDto product = new ProductDto
				//{
				//	ProductId = ProductId,
				//	CategoryId = request.CategoryId,
				//	PlatformId = 1,
				//	ProductBrand = "",
				//	ProductImage = ProductImage,
				//	ProductPrice = ProductPrice,
				//	ProductName = ProductName,
				//	ProductRating = "4",
				//	ProductProperty = null,
				//	Status = false,
				//	ProductLink = ProductLink,
				//	Comment = new List<CommentDto>()
				//};
				List<CommentDto> comments = new List<CommentDto>();	
				foreach (var Sp in ScrapeProduct)
				{
					var ProdID = Sp.GetAttribute("data-id");
					var Link = Sp.FindElement(By.CssSelector("div.p-card-chldrn-cntnr a")).GetAttribute("href");
					string originalWindow = driver.CurrentWindowHandle;
					Sp.Click();
                    var windowHandles = driver.WindowHandles;
					driver.SwitchTo().Window(windowHandles[1]);

					OverlayControl(driver);
					IList<IWebElement> Comments = new List<IWebElement>();
					OverlayControl(driver);
					var ratingIsExist = driver.FindElements(By.ClassName("rvw-cnt-tx"));
					if(ratingIsExist.Count > 0)
					{
						IWebElement ratings = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("rvw-cnt-tx")));
						ratings.Click();
					}
					else
					{
						driver.Close();
						driver.SwitchTo().Window(originalWindow);
						continue;
					}
					Comments = driver.FindElements(By.ClassName("comment-text"));
                    foreach (var element in Comments)
                    {
						comments.Add(new CommentDto { CommentText = element.Text, ProductId = (int)request.ProductId, ProductLink = Link, ProductPlatformID = Convert.ToString(ProdID) });
                    }                    
					driver.Close();
                    driver.SwitchTo().Window(originalWindow);
                }
				 var analyse =await _emotinalAnalyseService.GetEmotionalAnalysis(comments);
				 var res = _mapper.Map<List<CommentDto>>(analyse);

				 var result = await _commentService.TAddRangeAsync(res);
				 return new ScrapingResponseDto { Description = "Başarılı", ProductId = (int)request.ProductId, Status = "True" };

			}
		}

        public static class AnalyseBestCagory
        {
            public static IWebElement bestMatch(IList<IWebElement>elementKategori,string catName)
            {
				string bestMatch = null;
				int highestMatchCount = 0;
				IWebElement cat = null;
				foreach (var item in elementKategori)
				{
					int matchCount = GetMatchCount(catName, item.Text);
					if (matchCount > highestMatchCount)
					{
						highestMatchCount = matchCount;
						bestMatch = item.Text;
						cat = item;
					}
				}
                return cat;
			}
			static int GetMatchCount(string str1, string str2)
			{
				var words1 = str1.Split(' ');
				var words2 = str2.Split(' ');

				var set1 = new HashSet<string>(words1);
				var set2 = new HashSet<string>(words2);

				set1.IntersectWith(set2);
				return set1.Count;
			}
		}
        public async void OverlayControl(IWebDriver driver)
        {
            var overlay = driver.FindElements(By.ClassName("dark-overlay"));
			var overlay2 = driver.FindElements(By.ClassName("overlay"));
            if (overlay.Count() > 0 || overlay2.Count() > 0)
			{

				int windowHeight = driver.Manage().Window.Size.Height;
				int windowWidth = driver.Manage().Window.Size.Width;
				int centerX = windowWidth / 2;
				int centerY = windowHeight / 2;
				Actions actions = new Actions(driver);
				actions.MoveByOffset(centerX, centerY).Click().Perform();
			}
		}
    }
}
