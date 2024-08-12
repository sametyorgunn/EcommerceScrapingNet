using BusinessLayer.IServices;
using BusinessLayer.IServices.IGeneric;
using DataAccessLayer.IRepositories;
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

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await _productRepository.GetListAllAsync();
        }

        public async Task<List<Product>> GetListByFilterAsync(Expression<Func<Product, bool>> filter)
        {
           var result = await _productRepository.GetListAllAsync(filter);
           return result;
        }

        public async Task TAddAsync(Product t)
        {
            await _productRepository.InsertAsync(t);
        }

        public async Task<bool> TAddRangeAsync(List<Product> t)
        {
           var result = await _productRepository.InsertManyAsync(t);
           return result;
        }

        public async Task TDeleteAsync(Product t)
        {
            await _productRepository.DeleteAsync(t);
        }

        public async Task<Product> TGetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task TUpdateAsync(Product t)
        {
            await _productRepository.UpdateAsync(t);
        }

        public Task<bool> TUpdateRangeAsync(List<Product> t)
        {
            throw new NotImplementedException();
        }
    }
}
