using DataAccessLayer.IRepositories.IGeneric;
using DataAccessLayer.Repositories.Generic;
using EntityLayer.Dto.RequestDto.Category;
using EntityLayer.Dto.ResponseDto;
using EntityLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepositories
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
		public Task<List<Category>> GetTrendyolCategoriesByPlatform(GetCategoriesByFilterDto request);
		public Task<List<Category>> GetMainCategories();
		public Task<List<Category>> GetSubCategories(GetCategoriesByFilterDto request);
	}
}
