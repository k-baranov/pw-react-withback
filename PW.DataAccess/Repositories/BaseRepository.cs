using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PW.DataAccess.Interfaces;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PW.DataAccess.Repositories
{
    public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
        where TContext : DbContext, new()
    {
        public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
        {
            using var context = new TContext();
            return await context.Set<TEntity>().ToListAsync();
        }
        
        public virtual async Task<long> CountAsync()
        {
            using var context = new TContext();
            return await context.Set<TEntity>().CountAsync();
        }
        
        public virtual async Task<IReadOnlyCollection<TEntity>> AllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using var context = new TContext();
            var query = GetQueryWithProperties(context, includeProperties);
            return await query.ToListAsync();
        }
        
        public virtual async Task<TEntity> GetSingleAsync(long id)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        
        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        
        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            using var context = new TContext();
            var query = GetQueryWithProperties(context, includeProperties);
            return await query.Where(predicate).FirstOrDefaultAsync();
        }
        
        public virtual async Task<IReadOnlyCollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var context = new TContext();
            return await context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            using var context = new TContext();
            EntityEntry dbEntityEntry = context.Entry(entity);
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            using var context = new TContext();
            EntityEntry dbEntityEntry = context.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            using var context = new TContext();
            EntityEntry dbEntityEntry = context.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public virtual async Task DeleteByIdAsync(long id)
        {
            await DeleteWhereAsync(t => t.Id.Equals(id));
        }

        public virtual async Task DeleteWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var context = new TContext();
            IEnumerable<TEntity> entities = context.Set<TEntity>().Where(predicate);

            foreach (var entity in entities)
            {
                context.Entry(entity).State = EntityState.Deleted;
            }
            await context.SaveChangesAsync();
        }
        
        private IQueryable<TEntity> GetQueryWithProperties(TContext context, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }
    }
}
