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
using System;
using System.Net;
using OpenQA.Selenium.Firefox;

namespace BusinessLayer.Managers
{
	public class N11Manager : IN11Service
    {
        private readonly IProductService _productService;
		private readonly IMapper _mapper;
		private readonly IEmotinalAnalysis _emotinalAnalyseService;
		private readonly IAIService _AIService;
		private readonly ICategoryService _categoryService;
		private readonly ILogService _logService;
		public N11Manager(IProductService productService, IMapper mapper, IEmotinalAnalysis emotinalAnalyseService, IAIService aIService, ICategoryService categoryService, ILogService logService)
		{
			_productService = productService;
			_mapper = mapper;
			_emotinalAnalyseService = emotinalAnalyseService;
			_AIService = aIService;
			_categoryService = categoryService;
			_logService = logService;
		}

		public async Task<ScrapingResponseDto> GetProductAndCommentsAsync(GetProductAndCommentsDto request)
		{
			new DriverManager().SetUpDriver(new ChromeConfig());
			var options = new ChromeOptions();
			//options.AddArgument("--headless");
			//options.AddArgument("--disable-gpu");
			//options.AddArgument("--no-sandbox");
			//options.AddArgument("--profile-directory=Default");
			//options.AddArgument("--disable-dev-shm-usage");
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
					//driver.Navigate().Refresh();					
					driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
					var jsExecutor = (IJavaScriptExecutor)driver;
					driver.Navigate().GoToUrl("https://www.n11.com/");
					var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

					var searchInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("searchData")));
					//var searchInput =  driver.FindElement(By.Id("searchData"));
					searchInput.Clear();
					searchInput.SendKeys(request.ProductName);
					searchInput.SendKeys(Keys.Enter);
					Thread.Sleep(1000);

					var ProductId = "";
					var ProductName = "";
					var ProductPrice = "";
					var ProductImage = "";
					var ProductLink = "";
					ProductDto productDto = new ProductDto();

					OverlayControl(driver);
					var ScrapeProductIsExist = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("li.column")));
					var ScrapeProduct = driver.FindElements(By.CssSelector("li.column")).Take(3).ToList();

					List<CommentDto> comments = new List<CommentDto>();
					foreach (var Sp in ScrapeProduct)
					{
						Thread.Sleep(1000);
						string originalWindow = driver.CurrentWindowHandle;
						var ProdName = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("productName"))).Text;
						//var ProdName = Sp.FindElement(By.ClassName("productName")).Text;

						var isTrueProduct = await _AIService.isTrueProduct(new isTrueProductDto { ProductName = request.ProductName, ProductNamePlatform = ProdName });
						if (isTrueProduct == false) { continue; }

						//var isSame = SameControl(request.ProductName, ProdName); 
						//if (isSame == false){continue;}

						ProductId = Sp.FindElement(By.ClassName("plink")).GetAttribute("data-id");
						var isExistProduct = await
							_productService.GetProductByMarketPlaceID(new GetProductByMarketPlaceId
							{ ProductId = ProductId });

						if (isExistProduct == false) { break; }

						ProductName = Sp.FindElement(By.CssSelector("h3.productName")).Text;
						ProductPrice = Sp.FindElement(By.CssSelector("div.priceContainer ins")).Text;
						ProductImage = Sp.FindElement(By.CssSelector("img.cardImage")).GetAttribute("src");
						ProductLink = Sp.FindElement(By.CssSelector("div.pro a")).GetAttribute("href");

						productDto.ProductId = ProductId;
						productDto.CategoryId = request.CategoryId;
						productDto.PlatformId = (int)EntityLayer.Enums.Platform.n11;
						productDto.ProductBrand = "";
						productDto.ProductImage = ProductImage;
						productDto.ProductPrice = ProductPrice;
						productDto.ProductName = ProductName;
						productDto.ProductRating = "4";
						productDto.ProductProperty = null;
						productDto.Status = true;
						productDto.ProductLink = ProductLink;
						productDto.Comment = new List<CommentDto>();

						var ProdID = Sp.FindElement(By.ClassName("plink")).GetAttribute("data-id");
						var Link = Sp.FindElement(By.CssSelector("div.pro a")).GetAttribute("href");

						OverlayControl(driver);
						Actions newTabAction = new Actions(driver);
						newTabAction.KeyDown(Keys.Control).Click(Sp.FindElement(By.CssSelector("div.pro a"))).KeyUp(Keys.Control).Perform();
						var windowHandles = driver.WindowHandles;
						OverlayControl(driver);
						wait.Until(d => d.WindowHandles.Count > 1);
						driver.SwitchTo().Window(windowHandles[1]);
						Thread.Sleep(1000);

						IList<IWebElement> Comments = driver.FindElements(By.ClassName("comment"));

						foreach (var comment in Comments)
						{
							var a = comment.FindElement(By.CssSelector("p")).Text;
							comments.Add(new CommentDto { CommentText = a, ProductId = productDto.Id, ProductLink = Link, ProductPlatformID = ProdID.ToString() });
						}

						#region GetOtherComment
						var nextPageIsExist = driver.FindElements(By.CssSelector("a.next.navigation")).ToList();
						if (nextPageIsExist.Count > 0)
						{
							IWebElement nextpageBtn2 = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("a.next.navigation")));
							while (nextpageBtn2 != null)
							{
								bool isElementPresent = driver.FindElements(By.CssSelector("a.next.navigation")).Count > 0;
								if (!isElementPresent) { nextpageBtn2 = null; break; }
								IWebElement nextpageBtn = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("a.next.navigation")));
								((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", nextpageBtn);
								Thread.Sleep(1000);
								((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextpageBtn);
								Thread.Sleep(1000);
								IList<IWebElement> CommentsNext = driver.FindElements(By.ClassName("comment"));
								foreach (var comment in CommentsNext)
								{
									var a = comment.FindElement(By.CssSelector("p")).Text;
									comments.Add(new CommentDto { CommentText = a, ProductId = productDto.Id, ProductLink = Link, ProductPlatformID = ProdID.ToString() });
								}
								if (comments.Count() >= 200) { break; }
							}
						}
						#endregion

						driver.Close();
						driver.SwitchTo().Window(originalWindow);
					}
					var analyse = await _emotinalAnalyseService.GetEmotionalAnalysis(comments);
					var res = _mapper.Map<List<CommentDto>>(analyse);
					productDto.Comment = res;
					productDto.CategoryId = request.CategoryId;
					var result = await _productService.CreateProduct(productDto);

					return new ScrapingResponseDto { Description = "Başarılı", ProductId = result.Id, Status = "True" };
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
			Thread.Sleep(1000);
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
		public bool SameControl(string RequestProductName,string ScrapeProductName)
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
        public static double ComputeSimilarity(string source, string target)
        {
            var engProdName = source.ToUpper(new CultureInfo("en-US", false));
            var engSplitProdName = target.ToUpper(new CultureInfo("en-US", false));

            var sourceSet = source.Split(' ').ToHashSet();
            var targetSet = target.Split(' ').ToHashSet();

            var intersection = sourceSet.Intersect(targetSet).Count();
            var union = sourceSet.Union(targetSet).Count();

            return (double)intersection / union;
        }

		public async Task<bool> N11CategoryUpdate()
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
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
				var jsExecutor = (IJavaScriptExecutor)driver;
				driver.Navigate().GoToUrl("https://www.n11.com/");
				var CateTree = driver.FindElements(By.CssSelector("li.catMenuItem"));
				List<CategoryMarketPlaceDto> categories = new List<CategoryMarketPlaceDto>();

				foreach (var cat in CateTree)
				{
					var catree = cat.FindElement(By.ClassName("catMenuTree"));
					jsExecutor.ExecuteScript("arguments[0].className = 'catMenuTree active';", catree);

					string originalWindow = driver.CurrentWindowHandle;
					var maincategory = cat.FindElement(By.CssSelector("a.itemContainer"));
					var maincatname = maincategory.GetAttribute("title");
					var subcategoriess = cat.FindElements(By.CssSelector("a.subCatMenuItem"));
                    List<SubCategoryMarketPlaceDto> subcategories = new List<SubCategoryMarketPlaceDto>();
					var subName = "";
                    foreach (var sub in subcategoriess)
					{
						Thread.Sleep(1000);
                        Actions actions = new Actions(driver);
						OverlayControl(driver);
						actions.MoveToElement(maincategory).Perform();
						actions.MoveToElement(sub).KeyDown(Keys.Control).Click().KeyUp(Keys.Control).Perform();

						subName = sub.FindElement(By.CssSelector("span")).Text;
						var windowHandles = driver.WindowHandles;

						wait.Until(d => d.WindowHandles.Count > 1);
						Thread.Sleep(1000);
						driver.SwitchTo().Window(windowHandles[1]);

						var section = driver.FindElement(By.CssSelector("section.filterCategory"));
						var subcats = section.FindElements(By.CssSelector("li.parent ul.filterList li.filterItem a"));
						foreach (var subcat in subcats)
						{
							var a = subcat.GetAttribute("title");
							subcategories.Add(new SubCategoryMarketPlaceDto { CategoryName = a,PlatformID = (int)EntityLayer.Enums.Platform.n11 });
						}
                        driver.Close();
						driver.SwitchTo().Window(originalWindow);
					}
					 categories.Add(new CategoryMarketPlaceDto { CategoryName = subName, SubCategories = subcategories, PlatformID = (int)EntityLayer.Enums.Platform.n11 });
					await _categoryService.UpdateN11Categories(categories);
				}
				return true;
			}
		}
	}
}
