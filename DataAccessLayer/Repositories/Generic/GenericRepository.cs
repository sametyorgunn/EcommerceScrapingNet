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
        async Task IGenericRepository<T>.DeleteAsync(T t)
        {
            using (AppDbContext c = new AppDbContext())
            {
                c.Remove(t);
                c.SaveChanges();
            }
        }

        async Task<T> IGenericRepository<T>.GetByIdAsync(int id)
        {
            using (AppDbContext c = new AppDbContext())
            {
                return c.Set<T>().Find(id);
            }
        }

        async Task<List<T>> IGenericRepository<T>.GetListAllAsync()
        {
            using (AppDbContext c = new AppDbContext())
            {
                return c.Set<T>().ToList();
            }
        }

        async Task<List<T>> IGenericRepository<T>.GetListAllAsync(Expression<Func<T, bool>> filter)
        {
            using (AppDbContext c = new AppDbContext())
            {
                return c.Set<T>().Where(filter).ToList();
            }
        }

        async Task IGenericRepository<T>.InsertAsync(T t)
        {
            using (AppDbContext c = new AppDbContext())
            {
                c.Add(t);
                c.SaveChanges();
            }
        }

        async Task<bool> IGenericRepository<T>.InsertManyAsync(List<T> t)
        {
            using (AppDbContext c = new AppDbContext())
            {
                c.AddRange(t);
                c.SaveChanges();
                return true;
            }
        }

        async Task IGenericRepository<T>.UpdateAsync(T t)
        {
            using (AppDbContext c = new AppDbContext())
            {
                c.Update(t);
                c.SaveChanges();
            }
        }
    }
}
