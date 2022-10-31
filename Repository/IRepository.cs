using System;
using System.Linq.Expressions;
using WebApiEF.Models.Data;

namespace WebApiEF.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T>? GetAll();
        Task<IEnumerable<T>?> GetAsync(Expression<Func<T, bool>>? expression = null, ICollection<string>? includes = null);
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression, ICollection<string>? includes = null);

        IEnumerable<T>? Get(Func<T, bool> expression);
        T? GetSingle(Func<T, bool> expression);

        void Add(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        bool Exists(Func<T, bool> expression);
        void Add(T entity);
    }
}
