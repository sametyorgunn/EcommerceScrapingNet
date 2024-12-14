using AutoMapper;
using BusinessLayer.IServices;
using DataAccessLayer.IRepositories;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.RequestDto.Product;
using EntityLayer.Dto.ResponseDto;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;

namespace BusinessLayer.Managers
{
    public class HepsiBuradaManager : IHepsiBuradaService
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryrepository;
        private readonly ICommentService _commentService;
        private readonly IEmotinalAnalysis _emotinalAnalyseService;
        private readonly IAIService _AIService;

        public HepsiBuradaManager(IProductService productService,
            IMapper mapper, ICategoryService categoryService,
            ICategoryRepository categoryrepository, 
            ICommentService commentService,
            IEmotinalAnalysis emotinalAnalyseService, 
            IAIService aIService)
        {
            _productService = productService;
            _mapper = mapper;
            _categoryService = categoryService;
            _categoryrepository = categoryrepository;
            _commentService = commentService;
            _emotinalAnalyseService = emotinalAnalyseService;
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
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var jsExecutor = (IJavaScriptExecutor)driver;

                driver.Navigate().GoToUrl("https://www.hepsiburada.com/");
                var searchInput = driver.FindElement(By.CssSelector("div.initialComponent-hk7c_9tvgJ8ELzRuGJwC initialComponent-z0s572PM2ZR4NUXqD_iB"));
                searchInput.SendKeys(request.ProductName);
                searchInput.SendKeys(Keys.Enter);
                var ScrapeProduct = driver.FindElements(By.CssSelector("li.productListContent-zAP0Y5msy8OHn5z7T_K_")).Take(5).ToList();

                List<CommentDto> comments = new List<CommentDto>();
                foreach (var Sp in ScrapeProduct)
                {
                    wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete");
                    var ProdName = Sp.FindElement(By.CssSelector("h3.moria-ProductCard-iymOAa span")).Text;
                    var isTrueProduct = await _AIService.isTrueProduct(new isTrueProductDto { ProductName = request.ProductName, ProductNamePlatform = ProdName });
                    if (isTrueProduct == false) { continue; }
                    //var isSame = SameControl(request.ProductName, ProdName);
                    //if (isSame == false) { continue; }
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
                        if (Commentss.Count == previousCount || comments.Count() >= 100)
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

    }
}
