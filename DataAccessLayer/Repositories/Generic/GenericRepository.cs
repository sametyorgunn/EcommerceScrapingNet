using DataAccessLayer.Contexts;
using DataAccessLayer.IRepositories.IGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> TUpdateRangeAsync(List<T> t)
        {
            _appDbContext.UpdateRange(t);
            _appDbContext.SaveChanges();
            return true;
        }

        async Task IGenericRepository<T>.DeleteAsync(T t)
        {
            _appDbContext.Remove(t);
            _appDbContext.SaveChanges();
        }

        async Task<T> IGenericRepository<T>.GetByIdAsync(int id)
        {
            return _appDbContext.Set<T>().Find(id);
        }

        async Task<List<T>> IGenericRepository<T>.GetListAllAsync()
        {
            return _appDbContext.Set<T>().ToList();
        }

        //async Task<List<T>> IGenericRepository<T>.GetListAllFilterAsync(Expression<Func<T, bool>> filter)
        //{
        //    return _appDbContext.Set<T>().Where(filter).ToList();
        //}

        async Task IGenericRepository<T>.InsertAsync(T t)
        {
            _appDbContext.Add(t);
            _appDbContext.SaveChanges();
        }

        async Task<bool> IGenericRepository<T>.InsertManyAsync(List<T> t)
        {
           await _appDbContext.AddRangeAsync(t);
           await _appDbContext.SaveChangesAsync();
           return true;
        }

        async Task IGenericRepository<T>.UpdateAsync(T t)
        {
            _appDbContext.Update(t);
            _appDbContext.SaveChanges();
        }
    }
}
