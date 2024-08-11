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
        public void Delete(T t)
        {
            using (AppDbContext c = new AppDbContext())
            {
                c.Remove(t);
                c.SaveChanges();
            }
        }

        public T GetById(int id)
        {
            using (AppDbContext c = new AppDbContext())
            {
                return c.Set<T>().Find(id);
            }
        }

        public List<T> GetListAll()
        {
            using (AppDbContext c = new AppDbContext())
            {
                return c.Set<T>().ToList();
            }
        }

        public List<T> GetListAll(Expression<Func<T, bool>> filter)
        {
            using (AppDbContext c = new AppDbContext())
            {
                return c.Set<T>().Where(filter).ToList();
            }
        }

        public void Insert(T t)
        {
            using (AppDbContext c = new AppDbContext())
            {
                c.Add(t);
                c.SaveChanges();
            }
        }

        public void Update(T t)
        {
            using (AppDbContext c = new AppDbContext())
            {
                c.Update(t);
                c.SaveChanges();
            }
        }
    }
}
