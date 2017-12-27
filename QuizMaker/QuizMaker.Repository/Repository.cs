using QuizMaker.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        public Repository(DbContext context)
        {
            Context = context;
        }
        public T Add(T entity)
        {
           T t = Context.Set<T>().Add(entity);
            return t;
        }
        public void AddRange(List<T> entities)
        {
            Context.Set<T>().AddRange(entities);
        }
        public List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate).ToList();
        }
        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().Where(predicate).ToListAsync();
        }
        public T Get(long id)
        {
            return Context.Set<T>().Find(id);
        }
        public async Task<T> GetAsync(long id)
        {
            return await Context.Set<T>().FindAsync(id);
        }
        public List<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }
        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }
        public void RemoveRange(List<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
