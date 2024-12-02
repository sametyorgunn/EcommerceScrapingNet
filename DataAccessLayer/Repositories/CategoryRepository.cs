using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories.Generic;
using EntityLayer.Dto.RequestDto.Category;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using EntityLayer.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {

		private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

		public async Task<Category> AddCategory(Category category)
		{
			if(category.ParentId == null)
			{
				category.ParentId = 0;
				await _appDbContext.categories.AddAsync(category);
				await _appDbContext.SaveChangesAsync();
				return category;
			}
			else
			{
				await _appDbContext.categories.AddAsync(category);
				await _appDbContext.SaveChangesAsync();
				return category;
			}
		}

		public async Task<List<Category>> GetMainCategories()
		{
			var categories = await _appDbContext.categories.Where(x=>x.ParentId == 0).Take(6).ToListAsync();
			return categories;
		}

        public async Task<List<Category>> GetSubCategories(GetCategoriesByFilterDto request)
        {
			var categories = await _appDbContext.categories.Where(x => x.ParentId == request.Id)
				.ToListAsync();
			return categories;
        }

		public async Task<bool> UpdateN11Categories(List<CategoryMarketPlace> N11category)
		{
			foreach(var n11category in N11category)
			{
				var exist = _appDbContext.categoriesMarketplace.Any(x=>x.CategoryName == n11category.CategoryName && x.PlatformID == (int)Platform.n11);
				if (exist)
				{
					foreach(var subcategory in n11category.SubCategories)
					{
						var existsubcategories = _appDbContext.subCategoriesMarketplace.Any(x => x.CategoryName == subcategory.CategoryName && x.PlatformID ==(int)Platform.n11);
						if (existsubcategories)
						{
							continue;
						}
						else
						{
							await _appDbContext.subCategoriesMarketplace.AddAsync(subcategory);
							await _appDbContext.SaveChangesAsync();
						}
					}
				}
				else
				{
					await _appDbContext.categoriesMarketplace.AddRangeAsync(N11category);
					await _appDbContext.SaveChangesAsync();
				}
				
			}
			return true;
		}

        public async Task<bool> UpdateTrendyolCategories(List<CategoryMarketPlace> TrendyolcategoryDto)
        {
            foreach (var trendyolCat in TrendyolcategoryDto)
            {
                var exist = _appDbContext.categoriesMarketplace.Any(x => x.CategoryName == trendyolCat.CategoryName && x.PlatformID == (int)Platform.trendyol);
                if (exist)
                {
                    foreach (var subcategory in trendyolCat.SubCategories)
                    {
                        var existsubcategories = _appDbContext.subCategoriesMarketplace.Any(x => x.CategoryName == subcategory.CategoryName && x.PlatformID == (int)Platform.trendyol);
                        if (existsubcategories)
                        {
                            continue;
                        }
                        else
                        {
                            await _appDbContext.subCategoriesMarketplace.AddAsync(subcategory);
                            await _appDbContext.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    await _appDbContext.categoriesMarketplace.AddRangeAsync(trendyolCat);
                    await _appDbContext.SaveChangesAsync();
                }

            }
            return true;
        }
    }
}
