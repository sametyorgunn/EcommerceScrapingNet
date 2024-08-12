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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public async Task<bool> UpdateTrendyolCategories(List<Category> category)
        {
            using (var c = new AppDbContext())
            {
                foreach (var item in category)
                {
                    var existingCategory = c.categories.FirstOrDefault(x => x.Id == item.Id);
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
