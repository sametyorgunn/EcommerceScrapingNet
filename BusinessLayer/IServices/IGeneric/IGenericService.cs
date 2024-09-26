using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IServices.IGeneric
{
    public interface IGenericService<T>
    {
        Task TAddAsync(T t);
        Task<bool> TAddRangeAsync(List<T> t);
        Task<bool> TUpdateRangeAsync(List<T> t);
        Task TDeleteAsync(T t);
        Task TUpdateAsync(T t);
        Task<List<T>> GetListAsync();
        //Task<List<T>> GetListByFilterAsync(Expression<Func<T, bool>> filter);
        Task<T> TGetByIdAsync(int id);
    }
}
