using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace UI.Areas.Admin.ViewComponents.TrendyolCategory
{
	public class TrendyolCategoriesList:ViewComponent
	{
		private readonly ITrendyolService _trendyolService;
        public TrendyolCategoriesList(ITrendyolService trendyolService)
        {
            _trendyolService = trendyolService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
		{
			var result = await _trendyolService.getTrendyolCategories();
			return View(result);
		}
	}
}
