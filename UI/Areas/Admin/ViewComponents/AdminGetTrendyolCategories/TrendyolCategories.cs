using BusinessLayer.IServices;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace UI.Areas.Admin.ViewComponents.AdminGetTrendyolCategories
{
	public class TrendyolCategories : ViewComponent
	{
		private readonly ITrendyolService _trendyolservice;

        public TrendyolCategories(ITrendyolService trendyolservice)
        {
            _trendyolservice = trendyolservice;
        }

        public async Task<IViewComponentResult> InvokeAsync()
		{
			var result = await _trendyolservice.getTrendyolCategories();
			return View(result);
		}
	}
}
