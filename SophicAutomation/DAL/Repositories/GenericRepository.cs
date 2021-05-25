using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable
        where TEntity : class //HasIdBase<string>
    {
        protected ApplicationDbContext databaseContext;

        protected DbSet<TEntity> databaseSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.databaseContext = context;
            this.databaseSet = this.databaseContext.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return this.databaseSet.ToList();
        }

        public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return this.databaseSet.Where(predicate).ToList();
        }

        public Task<int> CountAsync()
        {
            return this.databaseSet.CountAsync();
        }

        public Task<List<TEntity>> GetPageAsync(int pageIndex, int pageSize)
        {
            return this.databaseSet.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public Task<int> Insert(TEntity item)
        {
            this.databaseSet.Add(item);
            return this.databaseContext.SaveChangesAsync();
        }

        public Task<int> Update(TEntity entity)
        {
            this.databaseSet.Attach(entity);
            this.SetEntityStateModified(entity);
            return this.databaseContext.SaveChangesAsync();
        }

        public void SetEntityStateModified(TEntity entity)
        {
            this.databaseContext.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<TEntity> GetPage(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate, out int count)
        {
            var result = this.databaseSet.Where(predicate);
            count = result.ToList().Count;
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.databaseContext.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
