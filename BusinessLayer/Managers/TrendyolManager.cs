using AutoMapper;
using BusinessLayer.IServices;
using DataAccessLayer.IRepositories;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
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
		public TrendyolManager(IProductService productService, IMapper mapper, ICategoryService categoryService, ICategoryRepository categoryrepository)
		{
			_productService = productService;
			_mapper = mapper;
			_categoryService = categoryService;
			_categoryrepository = categoryrepository;
		}

		public async Task<bool> GetProductAndCommentsAsync(GetProductAndCommentsDto request)
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
                Thread.Sleep(1000);
                var catName = request.CategoryName;
                string[] splitCatName = catName.Split(" ");
				wait.Until(driver => driver.FindElement(By.CssSelector("div.fltrs div.fltr-item-text")).Displayed);

				var elementKategori = driver.FindElements(By.CssSelector("div.fltrs div.fltr-item-text"));

				if (elementKategori.Count() != 1)
                {
					var SelectedCategory = elementKategori.FirstOrDefault(element => element.Text.Contains(catName, StringComparison.OrdinalIgnoreCase));
					if (SelectedCategory?.Text == catName)
					{
						SelectedCategory.Click();
					}
					else
					{
                        var catResult = AnalyseBestCagory.bestMatch(elementKategori, catName);
						catResult.Click();
					}
				}
			
				Thread.Sleep(1000);
                var ScrapeProduct = driver.FindElements(By.CssSelector("div.p-card-wrppr ")).Take(5).ToList();
                List<Product> ProductList = new List<Product>();
                var count = 0;
                foreach (var Sp in ScrapeProduct)
                {
                    var ProductId = Sp.GetAttribute("data-id");
                    var ProdId = Convert.ToInt32(ProductId);
					var productControl = await _productService.GetProductByProductId(new GetProductByProductId { ProductId = ProdId });
					if (productControl != null)
					{
						continue;
					}
					string originalWindow = driver.CurrentWindowHandle;
                    try
                    {
						Sp.Click();
                        var windowHandles = driver.WindowHandles;
						driver.SwitchTo().Window(windowHandles[1]);
					}
                    catch (Exception ex)
                    {
                        Sp.Click();
						var windowHandles = driver.WindowHandles;
						driver.SwitchTo().Window(windowHandles[1]);
					}
                   
                    try
                    {
                        string Brand = null;
                        string ProductName;
						var ProductBrand = driver.FindElements(By.CssSelector("a.product-brand-name-with-link"));
                        if(ProductBrand.Count() == 0) 
                        { 
                            ProductBrand = driver.FindElements(By.CssSelector("span.product-brand-name-without-link"));
                            Brand = ProductBrand.FirstOrDefault().Text;
							ProductName = driver.FindElement(By.CssSelector("h1.pr-new-br span:nth-of-type(2)")).Text;
						}
						else
                        {
                            Brand = ProductBrand.FirstOrDefault().Text;
						    ProductName = driver.FindElement(By.CssSelector("h1.pr-new-br span")).Text;
						}
						var ProductRating = driver.FindElement(By.CssSelector("div.rating-line-count")).Text;
                        var ProductPrice = driver.FindElement(By.CssSelector("span.prc-dsc")).Text;
                        var ProductImage = driver.FindElement(By.CssSelector("div.base-product-image img")).GetAttribute("src");
                        var ProductProperties = driver.FindElements(By.ClassName("attribute-item"));
                        Dictionary<string, string> properties = new Dictionary<string, string>();

                        foreach (var item in ProductProperties)
                        {
                            var attributeNameElement = item.FindElement(By.CssSelector(".attribute-label.attr-name"));
                            string attributeName = attributeNameElement.Text;
                            var attributeValueElement = item.FindElement(By.CssSelector(".attribute-value .attr-name.attr-name-w"));
                            string attributeValue = attributeValueElement.Text;
                            properties.Add(attributeName, attributeValue);
                        }
						List<ProductProperty> propertiesList = properties.Select(kv => new ProductProperty
                        {
                            PropertyTitle = kv.Key,
                            PropertyText = kv.Value,
                            ProductId = ProdId
                        }).ToList();
						IList<IWebElement> Comments = new List<IWebElement>();
						try
						{
							OverlayControl(driver);
                            IWebElement ratings = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("rvw-cnt-tx")));
                            ratings.Click();
							Comments = driver.FindElements(By.ClassName("comment-text"));
                        }
                        catch(Exception ex)
                        {
							Console.WriteLine(ex.Message);
						}

						Product product = new Product
                        {
                            ProductId = ProdId,
                            ProductBrand = Brand,
                            ProductName = ProductName,
                            ProductImage = ProductImage,
                            ProductPrice = ProductPrice,
                            ProductProperty = propertiesList,
                            ProductRating = ProductRating,
                            PlatformId = (int)EntityLayer.Enums.Platform.trendyol,
                            Comment = new List<Comment>()
                        };
                        foreach (var element in Comments)
                        {
                            product.Comment.Add(new Comment { CommentText = element.Text });
                        }
                       
                        ProductList.Add(product);
                        count++;
                        
                    }
                    catch (Exception ex)
                    {
						Console.WriteLine(ex.Message);
					}

					driver.Close();
                    driver.SwitchTo().Window(originalWindow);
                }
               
                var payload = _mapper.Map<List<ProductDto>>(ProductList);
                var result = await _productService.TAddRangeAsync(payload);
                if(result == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<string> GetTrendyolCategoriesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.trendyol.com/sapigw/product-categories";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return responseData;
                }
                else
                {
                    Console.WriteLine($"İstek başarısız oldu: {response.StatusCode}");
                    return "başarısız";
                }
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

				// Her iki stringdeki kelimeleri kümelere dönüştür
				var set1 = new HashSet<string>(words1);
				var set2 = new HashSet<string>(words2);

				// Ortak kelimeleri say
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

				//Actions actions = new Actions(driver);
				//actions.MoveByOffset(10, 100).Click().Perform();
				int windowHeight = driver.Manage().Window.Size.Height;
				int windowWidth = driver.Manage().Window.Size.Width;

				int centerX = windowWidth / 2;
				int centerY = windowHeight / 2;
				Actions actions = new Actions(driver);
				actions.MoveByOffset(centerX, centerY).Click().Perform();
			}
		}

		public async Task<string> ScrapeTrendyolCategoriesAsync()
		{
            var options = new ChromeOptions();
			//options.AddArgument("--headless");

			using (IWebDriver driver = new ChromeDriver())
			{
				WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				var jsExecutor = (IJavaScriptExecutor)driver;

				driver.Navigate().GoToUrl("https://www.trendyol.com/");
				IWebElement cat = wait.Until(ExpectedConditions.ElementExists(By.ClassName("category-header")));

				var categories = driver.FindElements(By.ClassName("category-header"));
				var elektronik = categories.FirstOrDefault(x => x.Text == "Elektronik");
				IWebElement elkt = wait.Until(ExpectedConditions.ElementToBeClickable(elektronik));
				List<CategoryDto> listCategory = new List<CategoryDto>();

				try
				{
					elkt.Click();
					var items = driver.FindElements(By.ClassName("item"));
					foreach (var item in items)
					{
						CategoryDto dt = new CategoryDto();
						dt.Name = item.Text;
						dt.PlatformId = 0;
						listCategory.Add(dt);
					}
				}
				catch
				{
					Actions actions = new Actions(driver);
					actions.MoveByOffset(10, 100).Click().Perform();
					Thread.Sleep(1000);
					elkt.Click();
					var items = driver.FindElements(By.ClassName("item"));
					foreach (var item in items)
					{
						CategoryDto dt = new CategoryDto();
						dt.Name = item.Text;
						dt.PlatformId = 0;
						listCategory.Add(dt);
					}
					var payload = _mapper.Map<List<Category>>(listCategory);
					await _categoryrepository.UpdateTrendyolCategories(payload);
				}
				return "a";
			}
		}
	}
}
