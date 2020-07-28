using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _set;
        public Repository(DbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }
        public T GetById(int id)
        {
            return _set.Find(id);
        }

        public IEnumerable<T> Get()
        {
            return _set;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        public void Add(T entity)
        {
            _set.Add(entity);
            _context.SaveChanges();

        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
