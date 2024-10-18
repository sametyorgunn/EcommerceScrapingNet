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

namespace BusinessLayer.Managers
{
    public class HBManager : IHbService
    {
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

                driver.Navigate().GoToUrl("https://www.hepsiburada.com/");
                var searchInput = driver.FindElement(By.ClassName("initialComponent-z0s572PM2ZR4NUXqD_iB"));
                searchInput.SendKeys(request.ProductName);
                searchInput.SendKeys(Keys.Enter);
                Thread.Sleep(1000);
                var catName = request.TrendyolCategoryName;
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
                    var ProductLink = Sp.FindElement(By.CssSelector("div.p-card-chldrn-cntnr a")).GetAttribute("href");
                    var ProdId = Convert.ToInt32(ProductId);
                    //var productControl = await _productService.GetProductByProductId(new GetProductByProductId { ProductId = ProdId });
                    if ("" != null)
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
                        if (ProductBrand.Count() == 0)
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
                            //OverlayControl(driver);
                            IWebElement ratings = wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("rvw-cnt-tx")));
                            ratings.Click();
                            Comments = driver.FindElements(By.ClassName("comment-text"));
                        }
                        catch (Exception ex)
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
                            ProductLink = ProductLink,
                            CategoryId = request.CategoryId,
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

                //var payload = _mapper.Map<List<ProductDto>>(ProductList);
                //var result = await _productService.TAddRangeAsync(payload);
                //if (result == true)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return true;
            }
        }
    }
}
