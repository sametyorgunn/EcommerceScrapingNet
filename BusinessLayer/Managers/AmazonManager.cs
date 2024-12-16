using AutoMapper;
using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.RequestDto.Product;
using EntityLayer.Dto.ResponseDto;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Globalization;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;

namespace BusinessLayer.Managers
{
	public class AmazonManager : IAmazonService
	{
		private readonly IProductService _productService;
		private readonly IMapper _mapper;
		private readonly IEmotinalAnalysis _emotinalAnalyseService;
        private readonly ICommentService _commentService;
        private readonly IAIService _AIService;
		private readonly ILogService _logService;

		public AmazonManager(IProductService productService, IMapper mapper, IEmotinalAnalysis emotinalAnalyseService, ICommentService commentService, IAIService aIService, ILogService logService)
		{
			_productService = productService;
			_mapper = mapper;
			_emotinalAnalyseService = emotinalAnalyseService;
			_commentService = commentService;
			_AIService = aIService;
			_logService = logService;
		}

		public async Task<ScrapingResponseDto> GetProductAndCommentsAsync(GetProductAndCommentsDto request)
		{
			new DriverManager().SetUpDriver(new ChromeConfig());
			var options = new ChromeOptions();
			options.AddArgument("--headless");
			options.AddArgument("--disable-gpu");
			options.AddArgument("--no-sandbox");
			options.AddArgument("--disable-dev-shm-usage");
			options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36");
			options.AddArgument("window-size=1920,1080");
			options.AddArgument("--disable-blink-features=AutomationControlled");
			options.AddArgument("--profile-directory=Default");

			try
			{
				using (IWebDriver driver = new ChromeDriver(options))
				{
					var jsExecutor = (IJavaScriptExecutor)driver;
					driver.Navigate().GoToUrl("https://www.amazon.com.tr/");
					var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
					var searchInput = driver.FindElement(By.Id("twotabsearchtextbox"));
					searchInput.Clear();
					searchInput.SendKeys(request.ProductName);
					searchInput.SendKeys(Keys.Enter);
					Thread.Sleep(1000);

					OverlayControl(driver);
					var ScrapeProduct = driver.FindElements(By.ClassName("sg-col-4-of-24")).Take(10).ToList();
					List<CommentDto> comments = new List<CommentDto>();
					foreach (var Sp in ScrapeProduct)
					{
						Thread.Sleep(1000);
						var originalWindow = driver.CurrentWindowHandle;
						//var ProdName = Sp.FindElement(By.CssSelector("span.a-text-normal")).Text;
						var ProdName = Sp.FindElement(By.CssSelector("a.a-link-normal h2 span")).Text;

						var isTrueProduct = await _AIService.isTrueProduct(new isTrueProductDto { ProductName = request.ProductName, ProductNamePlatform = ProdName });
						if (isTrueProduct == false) { continue; }

						//var isSame = SameControl(request.ProductName, ProdName);
						//if (isSame == false) { continue; }

						var prodID = Sp.GetAttribute("data-uuid");
						var isExistProduct = await
						_productService.GetProductByMarketPlaceID(new GetProductByMarketPlaceId { ProductId = prodID });
						if (isExistProduct == false) { break; }
						var Link = Sp.FindElement(By.CssSelector("a.a-link-normal")).GetAttribute("href");
						Actions newTabAction = new Actions(driver);
						newTabAction.KeyDown(Keys.Control).Click(Sp.FindElement(By.CssSelector("a.a-link-normal"))).KeyUp(Keys.Control).Perform();
						Thread.Sleep(1000);
						var windowHandles = driver.WindowHandles;
						OverlayControl(driver);
						wait.Until(d => d.WindowHandles.Count > 1);
						driver.SwitchTo().Window(windowHandles[1]);
						Thread.Sleep(1000);
						OverlayControl(driver);
						var ratingIsExist = driver.FindElements(By.Id("acrCustomerReviewLink"));
						if (ratingIsExist.Count > 0)
						{
							var ratings = wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Id("acrCustomerReviewLink"))));
							ratings.Click();
						}
						else
						{
							driver.Close();
							driver.SwitchTo().Window(originalWindow);
							continue;
						}

						IList<IWebElement> Comments = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("review-text-content")));
						foreach (var comment in Comments)
						{
							var a = comment.FindElement(By.CssSelector("span")).Text;
							comments.Add(new CommentDto { CommentText = a, ProductId = (int)request.ProductId, ProductLink = Link, ProductPlatformID = prodID });
						}
						driver.Close();
						driver.SwitchTo().Window(originalWindow);
					}

					var analyse = await _emotinalAnalyseService.GetEmotionalAnalysis(comments);
					var res = _mapper.Map<List<CommentDto>>(analyse);
					var result = await _commentService.TAddRangeAsync(res);

					return new ScrapingResponseDto { Description = "Başarılı", ProductId = (int)request.ProductId, Status = "True" };
				}
			}
			catch (Exception ex)
			{
				LogDto dto = new LogDto
				{
					CreatedDate = DateTime.Now,
					Message = ex.Message,
				};
				await _logService.AddLog(dto);
				return new ScrapingResponseDto { Description = "Başarısız", ProductId = 0, Status = "False" };
			}
		}
        public async void OverlayControl(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var overlay = driver.FindElements(By.Id("sp-cc"));

            if (overlay.Count > 0)
            {
                try
                {
                    var close = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("sp-cc-rejectall-link")));
                    close.Click();
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Kapatma butonu zamanında tıklanabilir olmadı.");
                }
            }
        }
        public bool SameControl(string RequestProductName, string ScrapeProductName)
        {
            List<string> sameOf = new List<string>();
            var splitProdName = RequestProductName.Split(" ");
            int index = 0;
            foreach (var prodName in splitProdName)
            {
                var engProdName = ScrapeProductName.ToUpper(new CultureInfo("en-US", false));
                var engSplitProdName = prodName.ToUpper(new CultureInfo("en-US", false));
                if (engProdName.Contains(engSplitProdName))
                {
                    sameOf.Add(prodName);
                }
                index++;
            }
            if (sameOf.Count > 1)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
    }
}
