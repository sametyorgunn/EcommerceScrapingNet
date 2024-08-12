using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IRepositories.IGeneric
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T t);
        Task<bool> InsertManyAsync(List<T> t);
        Task DeleteAsync(T t);
        Task UpdateAsync(T t);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetListAllAsync();
        Task<List<T>> GetListAllAsync(Expression<Func<T, bool>> filter);
    }
}
