using AutoMapper;
using BusinessLayer.IServices;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories;
using EntityLayer.Dto.RequestDto.Category;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryManager(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

		public async Task<List<CategoryDto>> GetListAsync()
        {
			var categories = await _categoryRepository.GetListAllAsync();
			var payload = _mapper.Map<List<CategoryDto>>(categories);
			return payload;
		}

		//public Task<List<CategoryDto>> GetListByFilterAsync(Expression<Func<CategoryDto, bool>> filter)
		//{
		//	throw new NotImplementedException();
		//}

		public async Task<List<CategoryDto>> GetMainCategories()
		{
			var categories =await _categoryRepository.GetMainCategories();
            var payload = _mapper.Map<List<CategoryDto>>(categories);
            return payload;
		}

        public async Task<List<CategoryDto>> GetSubCategories(GetCategoriesByFilterDto request)
        {
            var categories = await _categoryRepository.GetSubCategories(request);
            var payload = _mapper.Map<List<CategoryDto>>(categories);
            return payload;
        }

        public async Task<List<CategoryDto>> GetTrendyolCategoriesByPlatform(GetCategoriesByFilterDto request)
		{
			var categories = await _categoryRepository.GetTrendyolCategoriesByPlatform(request);
			var payload = _mapper.Map<List<CategoryDto>>(categories);
			return payload;
		}

		public async Task TAddAsync(CategoryDto t)
        {
			var payload = _mapper.Map<Category>(t);
            await _categoryRepository.InsertAsync(payload);
        }

        public Task<bool> TAddRangeAsync(List<CategoryDto> t)
        {
            throw new NotImplementedException();
        }

        public Task TDeleteAsync(CategoryDto t)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> TGetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task TUpdateAsync(CategoryDto t)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TUpdateRangeAsync(List<CategoryDto> t)
        {
            throw new NotImplementedException();
        }
      
    }
}
