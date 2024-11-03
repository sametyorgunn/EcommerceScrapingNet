using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories.Generic;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.RequestDto.Product;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

		public async Task<Product> CreateProduct(Product product)
		{
			 _appDbContext.products.Add(product);
		     _appDbContext.SaveChanges();
			 return product;
		}

		public async Task<List<Product>> GetLastFiveProducts()
		{
			
			var products = _appDbContext.products.OrderByDescending(x => x.Id).Take(5).ToList();
			return products;
			
		}

		public async Task<List<Product>> GetLastTwelveProduct()
		{
			var products = _appDbContext.products.OrderByDescending(x => x.Id).Take(12).ToList();
			return products;
		}

		public async Task<List<Product>> GetListAllByPlatformIdAsync(int platformId)
		{
			var products = _appDbContext.products.Where(x=>x.PlatformId == platformId).ToList();
			return products;
		}

		//public async Task<Product> GetProductByProductId(GetProductByProductId request)
		//{
		//	var product = _appDbContext.products.Where(x => x.ProductId == request.ProductId).FirstOrDefault();
		//	return product;
		//}

		public async Task<List<Product>> GetProductsByCategoryId(GetProductByFilterDto request)
		{
			var products = _appDbContext.products.
				Where(x=>x.CategoryId == request.CategoryId &&
				x.Status == true)
				.ToList();
			return products;
		}

		public async Task<Product> GetProductWithCommentAndProperties(GetProductByFilterDto request)
		{
			var product = _appDbContext.products.Where(x=>x.Id == request.Id)
				.Include(x => x.Category)
				.Include(x => x.Comment)
				.Include(x => x.ProductProperty)
				.FirstOrDefault();
			return product;
		}
	}
}
