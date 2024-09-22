﻿using BusinessLayer.IServices.IGeneric;
using EntityLayer.Dto.RequestDto.Category;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices
{
    public interface ICategoryService:IGenericService<CategoryDto>
    {
        public Task<List<CategoryDto>> GetTrendyolCategoriesByPlatform(GetCategoriesByFilterDto request);
        public Task<List<CategoryDto>> GetMainCategories();
        public Task<List<CategoryDto>> GetSubCategories(GetCategoriesByFilterDto request);
    }
}
