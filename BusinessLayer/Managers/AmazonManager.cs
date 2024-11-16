﻿using AutoMapper;
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

namespace BusinessLayer.Managers
{
	public class AmazonManager : IAmazonService
	{
		private readonly IProductService _productService;
		private readonly IMapper _mapper;
		private readonly IEmotinalAnalysis _emotinalAnalyseService;
        private readonly ICommentService _commentService;

        public AmazonManager(IProductService productService, IMapper mapper, IEmotinalAnalysis emotinalAnalyseService, ICommentService commentService)
        {
            _productService = productService;
            _mapper = mapper;
            _emotinalAnalyseService = emotinalAnalyseService;
            _commentService = commentService;
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
				driver.Navigate().GoToUrl("https://www.amazon.com.tr/");
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				var searchInput = driver.FindElement(By.Id("twotabsearchtextbox"));
				searchInput.Clear();
				searchInput.SendKeys(request.ProductName);
				searchInput.SendKeys(Keys.Enter);
				Thread.Sleep(1000);

				//OverlayControl(driver);
				var ScrapeProduct = driver.FindElements(By.ClassName("puis-card-container")).Take(1).ToList();

				var ProductId = driver.FindElement(By.CssSelector("#search > div.s-desktop-width-max.s-desktop-content.s-opposite-dir.s-wide-grid-style.sg-row > div.sg-col-20-of-24.s-matching-dir.sg-col-16-of-20.sg-col.sg-col-8-of-12.sg-col-12-of-16 > div > span.rush-component.s-latency-cf-section > div.s-main-slot.s-result-list.s-search-results.sg-row > div:nth-child(11)")).GetAttribute("data-uuid");
				var ProductName = driver.FindElement(By.CssSelector("a.a-link-normal span")).Text;
				//var ProductRating = driver.FindElement(By.CssSelector("strong.ratingScore")).Text;
				var ProductPrice = driver.FindElement(By.CssSelector("span.a-price-whole")).Text;
				var ProductImage = driver.FindElement(By.CssSelector("img.s-image")).GetAttribute("src");
				var ProductLink = driver.FindElement(By.CssSelector("a.a-link-normal")).GetAttribute("href");

				ProductDto dto = new ProductDto
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
					var Link = "";
                    var originalWindow = driver.CurrentWindowHandle;

                    var button = wait.Until(ExpectedConditions.ElementToBeClickable(Sp.FindElement(By.CssSelector("h2.a-size-mini a.a-link-normal"))));
                    button.Click();

                    var ratings = wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.Id("acrCustomerReviewLink"))));
                    ratings.Click();

                    IList<IWebElement> Comments = driver.FindElements(By.ClassName("review-text-content"));
                    foreach (var comment in Comments)
                    {
                        var a = comment.FindElement(By.CssSelector("span")).Text;
                        comments.Add(new CommentDto { CommentText = a, ProductId = (int)request.ProductId, ProductLink = Link });
                    }

                    driver.Close();
                    //driver.SwitchTo().Window(originalWindow);
                }

                var analyse = await _emotinalAnalyseService.GetEmotionalAnalysis(comments);
				var res = _mapper.Map<List<CommentDto>>(analyse);
                var result = await _commentService.TAddRangeAsync(res);

                return new ScrapingResponseDto { Description = "Başarılı", ProductId = (int)request.ProductId, Status = "True" };
			}
		}
	}
}
