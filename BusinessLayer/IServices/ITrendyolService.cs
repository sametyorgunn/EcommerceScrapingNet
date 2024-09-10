﻿using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface ITrendyolService
    {
        public Task<bool> GetProductAndCommentsAsync(GetProductAndCommentsDto request);
        public Task<string> GetTrendyolCategoriesAsync();
        public Task<string> ScrapeTrendyolCategoriesAsync();

	}
}
