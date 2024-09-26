using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories;
using DataAccessLayer.Repositories.Generic;
using EntityLayer.Dto.RequestDto.Category;
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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {

		private readonly AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

		public async Task<List<Category>> GetMainCategories()
		{
			var categories = await _appDbContext.categories.Where(x=>x.ParentId == 0).Take(6).ToListAsync();
			return categories;
		}

        public async Task<List<Category>> GetSubCategories(GetCategoriesByFilterDto request)
        {
			var categories = await _appDbContext.categories.Where(x => x.ParentId == request.ParentId)
				.ToListAsync();
			return categories;
        }

        public async Task<List<Category>> GetTrendyolCategoriesByPlatform(GetCategoriesByFilterDto request)
		{
			
			var categories =await _appDbContext.categories.Where(x=>x.PlatformId == request.PlatformId)
                .ToListAsync();
            return categories;
			
		}
	}
}
