using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        Task<int> CountAsync();
        Task<List<TEntity>> GetPageAsync(int pageIndex, int pageSize);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Func<TEntity, Boolean> predicate);
        Task<int> Insert(TEntity item);
        Task<int> Update(TEntity item);
        IEnumerable<TEntity> GetPage(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> predicate, out int count);
        public void Dispose();
    }
}
