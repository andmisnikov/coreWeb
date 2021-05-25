using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IGenericService<T>
    {
        public Task<int> CountAsync();

        public List<T> GetPage(int pageIndex, int pageSize);

        public Task<int> Insert(T entity);

        public Task<int> Update(T entity);

        IEnumerable<T> GetPage(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate, out int count);

        public void Dispose();
    }
}
