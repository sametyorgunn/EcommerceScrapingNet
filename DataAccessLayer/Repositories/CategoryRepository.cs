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

                // Değişiklikleri tek seferde kaydedin
                await c.SaveChangesAsync();
            }
            return true;
        }
    }
}
