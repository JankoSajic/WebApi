using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace WebApiEF.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public IEnumerable<T>? GetAll()
            => dbSet.AsNoTracking().ToList();

        public async Task<IEnumerable<T>?> GetAsync(Expression<Func<T, bool>>? expression = null, ICollection<string>? includes = null)
        {
            IQueryable<T> _dbSet = dbSet;

            if (expression != null)
                _dbSet = _dbSet.Where(expression);

            if (includes != null)
            {
                foreach (var include in includes)
                    _dbSet = _dbSet.Include(include);
            }

            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression, ICollection<string>? includes = null)
        {
            IQueryable<T> _dbSet = dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                    _dbSet = _dbSet.Include(include);
            }

            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(expression);
        }

        public IEnumerable<T>? Get(Func<T, bool> expression)
            => dbSet.AsNoTracking().Where(expression);

        public T? GetSingle(Func<T, bool> expression)
            => dbSet.AsNoTracking().SingleOrDefault(expression);

        public void Add(IEnumerable<T> entities)
            => dbSet.AddRange(entities);

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
            => dbSet.Remove(entity);

        public void DeleteRange(IEnumerable<T> entities)
            => dbSet.RemoveRange(entities);

        public bool Exists(Func<T, bool> expression)
            => dbSet.Any(expression);

        public void Add(T entity)
            => dbSet.Add(entity);
    }
}
