using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories.Generic;
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
		public async Task<List<Product>> GetListAllByPlatformIdAsync(int platformId)
		{
			using(var c = new AppDbContext())
			{
				var products = c.products.Where(x=>x.PlatformId == platformId).ToList();
				return products;
			};
		}
	}
}
