using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaker.Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        T Get(long id);

        Task<T> GetAsync(long id);

        List<T> GetAll();

        Task<List<T>> GetAllAsync();

        List<T> Find(Expression<Func<T, bool>> predicate);

        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

        T Add(T entity);

        void AddRange(List<T> entities);

        void Remove(T entity);

        void RemoveRange(List<T> entities);
    }
}
