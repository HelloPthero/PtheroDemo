using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain
{
    public class Repository<T, K> : IRepository<T, K> where T : Entity<K> where K : struct
    {
        private readonly IDBContext _dbContext;

        private readonly DbSet<T> _dbSet;

        public Repository(IDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task DeleteAsync(K id)
        {
            var e = await _dbSet.FindAsync(id);
            if (e != null)
            {
                 _dbSet.Remove(e);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(K id)
        {
            var e = await _dbSet.FindAsync(id);
            return e;
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
