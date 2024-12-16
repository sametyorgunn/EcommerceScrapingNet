using AutoMapper;
using BusinessLayer.IServices;
using DataAccessLayer.IRepositories;
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
	public class TrendyolManager : ITrendyolService
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryrepository;
		private readonly ICommentService _commentService;
		private readonly IEmotinalAnalysis _emotinalAnalyseService;
        private readonly IAIService _AIService;
		private readonly ILogService _logService;

		public TrendyolManager(IProductService productService, IMapper mapper, ICategoryService categoryService,
			ICategoryRepository categoryrepository,
			ICommentService commentService, IEmotinalAnalysis emotinalAnalyseService, IAIService aIService, ILogService logService)
		{
			_productService = productService;
			_mapper = mapper;
			_categoryService = categoryService;
			_categoryrepository = categoryrepository;
			_commentService = commentService;
			_emotinalAnalyseService = emotinalAnalyseService;
			_AIService = aIService;
			_logService = logService;
		}

		public async Task<ScrapingResponseDto> GetProductAndCommentsAsync(GetProductAndCommentsDto request)
        {
			new DriverManager().SetUpDriver(new ChromeConfig());
			var options = new ChromeOptions();
            //options.AddArgument("--headless");
            //options.AddArgument("--disable-gpu");
            //options.AddArgument("--no-sandbox");
            //options.AddArgument("--disable-dev-shm-usage");
            //options.AddArgument("--profile-directory=Default");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.6778.140 Safari/537.36");
			options.AddArgument("window-size=1920,1080");
			options.AddArgument("--disable-blink-features=AutomationControlled");
			//options.AddArgument("--start-maximized");
			//options.AddArgument("--disable-extensions");
			//options.AddArgument("--disable-infobars");
			//options.AddArgument("--disable-notifications");
			//options.AddArgument("--disable-popup-blocking");

			try
            {
                using (IWebDriver driver = new ChromeDriver(options))
                {
					driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
					WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    var jsExecutor = (IJavaScriptExecutor)driver;

                    driver.Navigate().GoToUrl("https://www.trendyol.com/");
                    Thread.Sleep(1000);
                    var searchInput = driver.FindElement(By.ClassName("V8wbcUhU"));
                    searchInput.SendKeys(request.ProductName);
                    searchInput.SendKeys(Keys.Enter);
                    var ScrapeProduct = driver.FindElements(By.CssSelector("div.p-card-wrppr ")).Take(3).ToList();

                    List<CommentDto> comments = new List<CommentDto>();
                    foreach (var Sp in ScrapeProduct)
                    {
                        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete");
                        var ProdName = Sp.FindElement(By.CssSelector("span.prdct-desc-cntnr-name")).Text;
                        //var isTrueProduct = await _AIService.isTrueProduct(new isTrueProductDto { ProductName = request.ProductName, ProductNamePlatform = ProdName });
                        //if (isTrueProduct == false) { continue; }
                        //var isSame = SameControl(request.ProductName, ProdName);
                        //if (isSame == false) { continue; }
                        var ProdID = Sp.GetAttribute("data-id");
                        var isExistProduct = await
                        _productService.GetProductByMarketPlaceID(new GetProductByMarketPlaceId { ProductId = ProdID });
                        if (isExistProduct == false) { break; }

                        var Link = Sp.FindElement(By.CssSelector("div.p-card-chldrn-cntnr a")).GetAttribute("href");
                        string originalWindow = driver.CurrentWindowHandle;
                        Sp.Click();
                        var windowHandles = driver.WindowHandles;
                        driver.SwitchTo().Window(windowHandles[1]);

                        OverlayControl(driver);
                        IList<IWebElement> Comments = new List<IWebElement>();
                        OverlayControl(driver);
                        var ratingIsExist = driver.FindElements(By.ClassName("rvw-cnt-tx"));
                        if (ratingIsExist.Count > 0)
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

                        #region GetOtherComments
                        int previousCount = 0;
                        while (true)
                        {
                            var Commentss = driver.FindElements(By.ClassName("comment-text"));
                            foreach (var element in Commentss.Skip(previousCount))
                            {
                                comments.Add(new CommentDto
                                {
                                    CommentText = element.Text,
                                    ProductId = (int)request.ProductId,
                                    ProductLink = Link,
                                    ProductPlatformID = Convert.ToString(ProdID)
                                });
                            }
                            if (Commentss.Count == previousCount || comments.Count() >= 200)
                            {
                                break;
                            }
                            previousCount = Commentss.Count;
                            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                            Thread.Sleep(1000);
                        }
                        #endregion

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

				
				try
				{
                    int windowHeight = driver.Manage().Window.Size.Height;
                    int windowWidth = driver.Manage().Window.Size.Width;
                    int centerX = windowWidth / 2;
                    int centerY = windowHeight / 2;
                    Actions actions = new Actions(driver);
                    actions.MoveByOffset(centerX, centerY).Click().Perform();
                }
                catch 
				{
                    int windowHeight = driver.Manage().Window.Size.Height;
                    int windowWidth = driver.Manage().Window.Size.Width;
                    int centerX = windowWidth / 3;
                    int centerY = windowHeight / 3;
                    Actions actions = new Actions(driver);
                    actions.MoveByOffset(centerX, centerY).Click().Perform();
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

        public async Task<bool> TrendyolCategoryUpdate()
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
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var jsExecutor = (IJavaScriptExecutor)driver;
                driver.Navigate().GoToUrl("https://www.trendyol.com/");
                OverlayControl(driver);
                var sideMenuBtn = driver.FindElement(By.CssSelector("div.side-menu-button-wrapper"));
                var sideMenuBtn2 = driver.FindElement(By.CssSelector("div.side-menu-button"));
				OverlayControl(driver);
				sideMenuBtn.Click();
                var MainMenus = driver.FindElements(By.CssSelector("div.left-side-container"));


                List<CategoryMarketPlaceDto> categories = new List<CategoryMarketPlaceDto>();

                foreach (var menu in MainMenus)
                {
                    OverlayControl(driver);
                    List<SubCategoryMarketPlaceDto> subcategoriesList = new List<SubCategoryMarketPlaceDto>();

                    var mainMenuName = menu.FindElement(By.CssSelector("span.category-title")).Text;
                    Actions actions = new Actions(driver);
                    actions.MoveToElement(menu).Perform();
                    var windowHandles = driver.WindowHandles;

                    var subcategories = driver.FindElements(By.CssSelector("li.category-sub-title a"));
                    foreach (var subcategory in subcategories)
                    {
                        var sub = subcategory.Text;
                        subcategoriesList.Add(new SubCategoryMarketPlaceDto { CategoryName = sub, PlatformID = (int)EntityLayer.Enums.Platform.trendyol });
                    }
                    categories.Add(new CategoryMarketPlaceDto { CategoryName = mainMenuName, SubCategories = subcategoriesList, PlatformID = (int)EntityLayer.Enums.Platform.trendyol });
                }
                await _categoryService.UpdateTrendyolCategories(categories);
                return true;
            }
        }
    }
}
