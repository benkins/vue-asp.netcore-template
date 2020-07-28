using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Data
{
    public interface IRepository<T>
    {
        T GetById(int id);
        IEnumerable<T> Get();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
