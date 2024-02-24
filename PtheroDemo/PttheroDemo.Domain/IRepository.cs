using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain
{
    public interface IRepository<T,K> where T : Entity<K> where K : struct
    {
        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(K id);

        Task DeleteAsync(K id);

        Task<IQueryable<T>> GetQueryableAsync(Expression<Func<T, bool>> expression = null);

        Task DeleteAsync(Expression<Func<T, bool>> predicate);

        Task InsertAsync(List<T> entities);

        Task UpdateAsync(List<T> entities); 



    }
}
