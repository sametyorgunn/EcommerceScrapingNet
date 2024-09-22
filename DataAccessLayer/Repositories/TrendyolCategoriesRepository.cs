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
    public class TrendyolCategoriesRepository : GenericRepository<TrendyolCategory>, ITrendyolCategoriesRepository
    {
        private readonly AppDbContext _appDbContext;

        public TrendyolCategoriesRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> UpdateTrendyolCategories(List<TrendyolCategory> category)
        {

            foreach (var item in category)
            {
                var existingCategory = _appDbContext.trendyolCategories.FirstOrDefault(x => x.Name == item.Name);
                if (existingCategory != null && existingCategory.Name != item.Name)
                {
                    existingCategory.Name = item.Name;
                    _appDbContext.trendyolCategories.Update(existingCategory);
                }
                else if (existingCategory == null)
                {
                    _appDbContext.trendyolCategories.Add(item);
                }
            }

            await _appDbContext.SaveChangesAsync();

            return true;
        }
    }
}
