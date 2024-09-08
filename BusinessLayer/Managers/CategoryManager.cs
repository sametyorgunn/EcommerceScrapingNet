using AutoMapper;
using BusinessLayer.IServices;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories;
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

		public async Task<List<CategoryDto>> GetListByFilterAsync(Expression<Func<Category, bool>> filter)
		{
			var Filter = _mapper.Map<Expression<Func<Category, bool>>>(filter);
			var categories =await _categoryRepository.GetListAllFilterAsync(Filter);
			var payload = _mapper.Map<List<CategoryDto>>(categories);

			return payload;
		}

		public Task TAddAsync(CategoryDto t)
        {
            throw new NotImplementedException();
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
        public async Task<bool> UpdateTrendyolCategories()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.trendyol.com/sapigw/product-categories";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    CategoryResponseDto categoryResponse = JsonConvert.DeserializeObject<CategoryResponseDto>(responseData);
                    var payload = _mapper.Map<List<Category>>(categoryResponse.Categories);
                    var result = _categoryRepository.UpdateTrendyolCategories(payload);
                    return await result;
                }
                else
                {
                    Console.WriteLine($"İstek başarısız oldu: {response.StatusCode}");
                    return false;
                }
            }
        }
    }
}
