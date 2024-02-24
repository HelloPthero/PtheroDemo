using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PtheroDemo.Domain.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PtheroDemo.Domain
{
    public class Repository<T, K> : IRepository<T, K> where T : Entity<K> where K : struct
    {
        private readonly IDBContext _dbContext;

        private readonly DbSet<T> _dbSet;

        private ICurrentUser _currentUser;

        //public IHttpContextAccessor HttpContextAccessor;

        public Repository(IDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _currentUser = httpContextAccessor.HttpContext.Items["CurrentUser"] as ICurrentUser;
            //HttpContextAccessor = httpContextAccessor;
        }

        public async Task DeleteAsync(K id)
        {
            var e = await _dbSet.FindAsync(id);
            if (e != null)
            {
                if(e is ISoftDeleted entity)
                {
                    entity.IsDeleted = true;
                    entity.DeleteUserId = _currentUser.UserId;
                    entity.DeletedTime = DateTime.Now;
                }
                else
                {
                    _dbSet.Remove(e);
                }
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

        public async Task InsertAsync(List<T> entities) 
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<T> entities) 
        {
            _dbContext.Entry(entities).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IQueryable<T>> GetQueryableAsync(Expression<Func<T, bool>> expression = null) 
        {
            IQueryable<T> queryable = _dbContext.Set<T>();

            // Check if T implements ISoftDeleted
            if (typeof(ISoftDeleted).IsAssignableFrom(typeof(T)))
            {
                // Create parameter expression
                var parameter = Expression.Parameter(typeof(T));

                // Get property expression for IsDeleted property
                var isDeletedProperty = Expression.Property(parameter, "IsDeleted");

                // Create constant expression for false
                var falseValue = Expression.Constant(false);

                // Create equality expression for IsDeleted == false
                var isNotDeletedExpression = Expression.Equal(isDeletedProperty, falseValue);

                // Combine the expressions
                if (expression != null)
                {
                    // Combine the original expression with IsDeleted condition
                    var combinedExpression = Expression.AndAlso(expression.Body, isNotDeletedExpression);
                    var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
                    queryable = queryable.Where(lambda);
                }
                else
                {
                    // If no original expression provided, filter by IsDeleted == false directly
                    queryable = queryable.Where(Expression.Lambda<Func<T, bool>>(isNotDeletedExpression, parameter));
                }
            }
            else if (expression != null)
            {
                // Apply the original expression to the queryable
                queryable = queryable.Where(expression);
            }

            return await Task.FromResult(queryable);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            // 检查 T 是否实现了 IDeleted 接口
            if (typeof(ISoftDeleted).IsAssignableFrom(typeof(T))) 
            {
                // 获取需要软删除的对象列表
                var entitiesToDelete = await _dbContext.Set<T>().Where(predicate).ToListAsync();

                // 遍历列表，将每个对象的 IsDeleted 属性设置为 true
                foreach (var entity in entitiesToDelete)
                {
                    if (entity is ISoftDeleted deletableEntity)
                    {
                        deletableEntity.IsDeleted = true;
                        deletableEntity.DeleteUserId = _currentUser.UserId;
                        deletableEntity.DeletedTime = DateTime.Now;
                        // 如果有必要，也可以设置删除时间等其他属性

                    }
                }
                // 提交更改到数据库
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // 如果 T 没有实现 IDeleted 接口，则直接删除符合条件的记录
                var entitiesToDelete = await _dbContext.Set<T>().Where(predicate).ToListAsync();
                _dbContext.Set<T>().RemoveRange(entitiesToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
