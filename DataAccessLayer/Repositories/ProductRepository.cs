using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Migrations;
using DataAccessLayer.Repositories.Generic;
using EntityLayer.Dto.RequestDto;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		public async Task<List<Product>> GetLastFiveProducts()
		{
			using (var c = new AppDbContext())
			{
				var products = c.products.OrderByDescending(x => x.Id).Take(5).ToList();
				return products;
			};
		}

		public async Task<List<Product>> GetListAllByPlatformIdAsync(int platformId)
		{
			using(var c = new AppDbContext())
			{
				var products = c.products.Where(x=>x.PlatformId == platformId).ToList();
				return products;
			};
		}

		public async Task<Product> GetProductByProductId(GetProductByProductId request)
		{
			using (var c = new AppDbContext())
			{
				var product = c.products.Where(x => x.ProductId == request.ProductId).FirstOrDefault();
				return product;
			};
		}
	}
}
