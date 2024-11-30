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
using SeleniumExtras.WaitHelpers;
using System.Globalization;
using EntityLayer.Dto.RequestDto.Product;

namespace BusinessLayer.Managers
{
	public class AmazonManager : IAmazonService
	{
		private readonly IProductService _productService;
		private readonly IMapper _mapper;
		private readonly IEmotinalAnalysis _emotinalAnalyseService;
        private readonly ICommentService _commentService;
        private readonly IAIService _AIService;

        public AmazonManager(IProductService productService, IMapper mapper, IEmotinalAnalysis emotinalAnalyseService, ICommentService commentService, IAIService aIService)
        {
            _productService = productService;
            _mapper = mapper;
            _emotinalAnalyseService = emotinalAnalyseService;
            _commentService = commentService;
            _AIService = aIService;
        }

        public async Task<ScrapingResponseDto> GetProductAndCommentsAsync(GetProductAndCommentsDto request)
		{
			var options = new ChromeOptions();
            //options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("window-size=1920x1080");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");



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
				var ScrapeProduct = driver.FindElements(By.ClassName("sg-col-4-of-24")).ToList();
                var matchedProducts = ScrapeProduct
                   .Where(product =>
                   {
                       try
                       {
                           var productName = product.FindElement(By.CssSelector("span.a-text-normal")).Text;
                           return productName.Contains(request.ProductName, StringComparison.OrdinalIgnoreCase);
                       }
                       catch (NoSuchElementException)
                       {
                           return false;
                       }
                   })
                   .ToList();

                List<CommentDto> comments = new List<CommentDto>();
				foreach (var Sp in matchedProducts)
				{
					Thread.Sleep(1000);
                    var originalWindow = driver.CurrentWindowHandle;

					var ProdName = Sp.FindElement(By.CssSelector("span.a-text-normal")).Text;

                    var isTrueProduct = await _AIService.isTrueProduct(new isTrueProductDto { ProductName = request.ProductName, ProductNamePlatform = ProdName });
                    if (isTrueProduct == false) { continue; }
                    //var isSame = SameControl(request.ProductName, ProdName);
                    //if (isSame == false) { continue; }

                    var prodID = Sp.GetAttribute("data-uuid");
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
