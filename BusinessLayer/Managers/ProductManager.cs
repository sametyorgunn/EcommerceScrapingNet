using AutoMapper;
using BusinessLayer.IServices;
using BusinessLayer.IServices.IGeneric;
using DataAccessLayer.IRepositories;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.RequestDto.Product;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Microsoft.VisualBasic;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
    public class ProductManager : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

		public ProductManager(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<ProductDto> CreateProduct(ProductDto dto)
		{
			var payload = _mapper.Map<Product>(dto);
			var result = await _productRepository.CreateProduct(payload);

			var response = _mapper.Map<ProductDto>(result);
			return response;
		}

		public async Task<List<ProductDto>> GetLastFiveProducts()
		{
			var products = await _productRepository.GetLastFiveProducts();
			var payload = _mapper.Map<List<ProductDto>>(products);
			return payload;
		}

		public async Task<List<ProductDto>> GetLastTwelveProduct()
		{
			var products = await _productRepository.GetLastTwelveProduct();
			var payload = _mapper.Map<List<ProductDto>>(products);
			return payload;
		}

		public async Task<List<ProductDto>> GetListAsync()
        {
            var products =  await _productRepository.GetListAllAsync();
            var payload = _mapper.Map<List<ProductDto>>(products);
            return payload;
		}

		//public async Task<List<ProductDto>> GetListByFilterAsync(Expression<Func<ProductDto, bool>> filter)
		//{
		//	var productFilter = _mapper.Map<Expression<Func<Product, bool>>>(filter);
		//	var products = await _productRepository.GetListAllFilterAsync(productFilter);
		//	var payload = _mapper.Map<List<ProductDto>>(products);
		//	return payload;
		//}

		public async Task<ProductDto> GetProductById(GetProductById request)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);
			var payload = _mapper.Map<ProductDto>(product);
			return payload;
		}

		//public async Task<ProductDto> GetProductByProductId(GetProductByProductId request)
		//{
		//	var product = await _productRepository.GetProductByProductId(request);
		//	var payload = _mapper.Map<ProductDto>(product);
		//	return payload;
		//}

		public async Task<List<ProductDto>> GetProductsByCategoryId(GetProductByFilterDto request)
		{
			var products = await _productRepository.GetProductsByCategoryId(request);
			var payload = _mapper.Map<List<ProductDto>>(products);
			return payload;
		}

		public async Task<List<ProductDto>> GetProductsByPlatformId(GetProductsByPlatformId request)
		{
			var products = await _productRepository.GetListAllByPlatformIdAsync(request.Id);
			var payload = _mapper.Map<List<ProductDto>>(products);
			return payload;
		}

		public async Task<ProductDto> GetProductWithCommentAndProperties(GetProductByFilterDto request)
		{
			var product = await _productRepository.GetProductWithCommentAndProperties(request);
			var payload = _mapper.Map<ProductDto>(product);
			return payload;
		}

		public async Task TAddAsync(ProductDto t)
        {
			var product = _mapper.Map<Product>(t);
			await _productRepository.InsertAsync(product);
        }

        public async Task<bool> TAddRangeAsync(List<ProductDto> t)
        {
			var payload = _mapper.Map<List<Product>>(t);
		    var result = await _productRepository.InsertManyAsync(payload);
            return result;
        }

        public async Task TDeleteAsync(ProductDto t)
        {
			var payload = _mapper.Map<Product>(t);
			await _productRepository.DeleteAsync(payload);
        }

        public async Task<ProductDto> TGetByIdAsync(int id)
        {
			var result = await _productRepository.GetByIdAsync(id);
			var payload = _mapper.Map<ProductDto>(result);
            return payload; 

		}

		public async Task TUpdateAsync(ProductDto t)
        {
			var payload = _mapper.Map<Product>(t);
			await _productRepository.UpdateAsync(payload);
		}

        public Task<bool> TUpdateRangeAsync(List<ProductDto> t)
        {
            throw new NotImplementedException();
        }
    }
}
