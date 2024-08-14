using AutoMapper;
using BusinessLayer.IServices;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using EntityLayer.Enums;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace BusinessLayer.Managers
{
    public class TrendyolManager : ITrendyolService
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        public TrendyolManager(IProductService productService, IMapper mapper, ICategoryService categoryService)
        {
            _productService = productService;
            _mapper = mapper;
            _categoryService = categoryService;
        }

        public async Task<bool> GetProductAndCommentsAsync(GetProductAndCommentsDto request)
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless");
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl("https://www.trendyol.com/");
                Thread.Sleep(1000);
                var searchInput = driver.FindElement(By.ClassName("V8wbcUhU"));
                searchInput.SendKeys(request.ProductName);
                searchInput.SendKeys(Keys.Enter);
                Thread.Sleep(1000);

                var catName = request.CategoryName;
				var elementKategori = driver.FindElements(By.CssSelector("div.fltr-item-text"));
				var SelectedCategory = elementKategori.FirstOrDefault(element => element.Text.Contains("iPhone IOS Cep Telefonları", StringComparison.OrdinalIgnoreCase));

				//var SelectedCategory = driver.FindElements(By.CssSelector("div.fltr-item-text")).Contains(catName).FirstOrDefault();
				SelectedCategory.Click();
                Thread.Sleep(1000);
                var ScrapeProduct = driver.FindElements(By.CssSelector("div.p-card-wrppr ")).Take(5).ToList();
                List<Product> ProductList = new List<Product>();
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var count = 0;
                foreach (var Sp in ScrapeProduct)
                {
                    var ProductId = Sp.GetAttribute("data-id");
                    var ProdId = Convert.ToInt32(ProductId);
                    var productControl = await _productService.GetProductById(new GetProductById { Id = ProdId});
                    if (productControl != null)
                    {
                        continue;
                    }
                    string originalWindow = driver.CurrentWindowHandle;
                    try
                    {
                        Sp.Click();
                    }
                    catch (Exception ex)
                    {
                        Actions actions = new Actions(driver);
                        actions.MoveByOffset(10, 100).Click().Perform();
                        Sp.Click();
                    }
                    var windowHandles = driver.WindowHandles;
                    driver.SwitchTo().Window(windowHandles[1]);
                    try
                    {
                        Thread.Sleep(1000);
                        var ProductBrand = driver.FindElement(By.CssSelector("a.product-brand-name-with-link")).Text;
                        var ProductName = driver.FindElement(By.CssSelector("h1.pr-new-br span")).Text;
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
                        IWebElement rating = driver.FindElement(By.ClassName("rvw-cnt-tx"));
                        Thread.Sleep(1000);
                        rating.Click();
                        List<ProductProperty> propertiesList = properties.Select(kv => new ProductProperty
                        {
                            PropertyTitle = kv.Key,
                            PropertyText = kv.Value,
                            ProductId = ProdId
                        }).ToList();
                        var elements = driver.FindElements(By.ClassName("comment-text"));
                        Product product = new Product
                        {
                            ProductId = ProdId,
                            ProductBrand = ProductBrand,
                            ProductName = ProductName,
                            ProductImage = ProductImage,
                            ProductPrice = ProductPrice,
                            ProductProperty = propertiesList,
                            ProductRating = ProductRating,
                            PlatformId = (int)EntityLayer.Enums.Platform.trendyol,
                            Comment = new List<Comment>()
                        };
                        foreach (var element in elements)
                        {
                            product.Comment.Add(new Comment { CommentText = element.Text });
                        }
                       
                        ProductList.Add(product);
                        count++;
                        
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }

                    driver.Close();
                    driver.SwitchTo().Window(originalWindow);
                }
               
                var payload = _mapper.Map<List<ProductDto>>(ProductList);
                var result = await _productService.TAddRangeAsync(payload);
                return result;
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

       
    }
}
