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

		public async Task<CategoryDto> AddCategory(CategoryDto categoryDto)
		{
			var payload = _mapper.Map<Category>(categoryDto);
            var result = await _categoryRepository.AddCategory(payload);
            var model = _mapper.Map<CategoryDto>(result);
            return model;

        }

        public async Task<List<CategoryMarketPlaceDto>> GetAllN11Categories()
        {
            var n11categories = await _categoryRepository.GetAllN11Categories();
            var result = _mapper.Map<List<CategoryMarketPlaceDto>>(n11categories);
            return result;
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
            var payload = _mapper.Map<Category>(t);
            _categoryRepository.DeleteAsync(payload);
            return Task.CompletedTask;
        }

        public async Task<CategoryDto> TGetByIdAsync(int id)
        {
            var category =await _categoryRepository.GetByIdAsync(id);
            var result = _mapper.Map<CategoryDto>(category);
            return result;
        }

        public async Task TUpdateAsync(CategoryDto t)
        {
            var payload = _mapper.Map<Category>(t);
            await _categoryRepository.UpdateAsync(payload);
        }

        public Task<bool> TUpdateRangeAsync(List<CategoryDto> t)
        {
            throw new NotImplementedException();
        }

		public async Task<bool> UpdateN11Categories(List<CategoryMarketPlaceDto> categoryDto)
		{
            var payload = _mapper.Map<List<CategoryMarketPlace>>(categoryDto);
            var result = await _categoryRepository.UpdateN11Categories(payload);
            return result;
		}

        public async Task<bool> UpdateTrendyolCategories(List<CategoryMarketPlaceDto> TrendyolcategoryDto)
        {
            var payload = _mapper.Map<List<CategoryMarketPlace>>(TrendyolcategoryDto);
            var result = await _categoryRepository.UpdateTrendyolCategories(payload);
            return result;
        }
    }
}
