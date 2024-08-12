using DataAccessLayer.IRepositories.IGeneric;
using DataAccessLayer.Repositories.Generic;
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
        Task<bool> UpdateTrendyolCategories(List<Category> category);
    }
}
