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
			var products = _appDbContext.products.Include(x=>x.Category)
				.Where(x=>x.Status == true)
				.OrderByDescending(x => x.Id).Take(12).ToList();
			return products;
		}

		public async Task<List<Product>> GetListAllByPlatformIdAsync(int platformId)
		{
			var products = _appDbContext.products.Where(x=>x.PlatformId == platformId).ToList();
			return products;
		}

		public async Task<List<Product>> GetProductsByCategoryId(GetProductByFilterDto request)
		{
			var subCategoryIds = GetAllSubCategoryIds(request.CategoryId);

			var products = await _appDbContext.products.Include(x=>x.Category)
				.Where(x => subCategoryIds.Contains(x.CategoryId) && x.Status == true)
				.ToListAsync();

			return products;
		}
		private List<int> GetAllSubCategoryIds(int categoryId)
		{
			var categoryIds = new List<int> { categoryId };

			var subCategories = _appDbContext.categories
				.Where(c => c.ParentId == categoryId)
				.Select(c => c.Id)
				.ToList();

			foreach (var subCategoryId in subCategories)
			{
				categoryIds.AddRange(GetAllSubCategoryIds(subCategoryId));
			}

			return categoryIds;
		}

		public async Task<List<Product>> GetProductsBySearch(string search)
		{
			var products = await _appDbContext.products
				.Where(x=>x.ProductName.ToLower().Contains(search.ToLower()))
				.ToListAsync();
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

		public async Task<bool> DeleteCheckedProducts(List<int> ids)
		{
			try
			{
				var products = _appDbContext.products.Where(x => ids.Contains(x.Id));
				_appDbContext.products.RemoveRange(products);
				await _appDbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
			
		}
	}
}
