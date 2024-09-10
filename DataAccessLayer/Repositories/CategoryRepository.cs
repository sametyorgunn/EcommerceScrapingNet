using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories.Generic;
using EntityLayer.Dto.RequestDto.Category;
using EntityLayer.Entity;
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
		public async Task<List<Category>> GetTrendyolCategoriesByPlatform(GetCategoriesByFilterDto request)
		{
			using (var c = new AppDbContext())
			{
				var categories =await c.categories.Where(x=>x.PlatformId == request.PlatformId)
                    .ToListAsync();
                return categories;
			}
		}

		public async Task<bool> UpdateTrendyolCategories(List<Category> category)
        {
            using (var c = new AppDbContext())
            {
                foreach (var item in category)
                {
                    var existingCategory = c.categories.FirstOrDefault(x => x.Name == item.Name && x.PlatformId == 0);
                    if (existingCategory != null && existingCategory.Name != item.Name)
                    {
                        existingCategory.Name = item.Name;
                        c.categories.Update(existingCategory);
                    }
                    else if (existingCategory == null)
                    {
                        c.categories.Add(item);
                    }
                }

                await c.SaveChangesAsync();
            }
            return true;
        }
		//public async Task<bool> UpdateTrendyolCategories(List<Category> category)
		//{

		//	List<Category> categories = new List<Category>();
		//	using (var c = new AppDbContext())
		//	{
		//		c.categories.Add(new Category { Name = "Elektronik", ParentId = 0, PlatformId = 0 });
		//		foreach (var item in category)
		//		{
		//			var existingCategory = c.categories.FirstOrDefault(x => x.Name == item.Name && x.PlatformId == 0);
		//			if (existingCategory != null && existingCategory.Name != item.Name)
		//			{
		//				existingCategory.Name = item.Name;
		//				c.categories.Update(existingCategory);
		//			}
		//			else if (existingCategory == null)
		//			{
		//				categories.Add(new Category { Name = item.Name, ParentId = 0, PlatformId = 0 });
		//			}
		//		}

		//		// Değişiklikleri tek seferde kaydedin
		//		c.categories.AddRangeAsync(categories);
		//		await c.SaveChangesAsync();
		//	}
		//	return true;
		//}
	}
}
