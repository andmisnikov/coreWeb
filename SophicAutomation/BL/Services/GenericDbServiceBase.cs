using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using BL.Interfaces;
using DAL.Interfaces;

namespace BL.Services
{
    public abstract class GenericDbServiceBase<TEntity, TDomainObject> : IGenericService<TDomainObject>
         where TEntity : class, new() // HasIdBase<string> where TDomainObject : HasIdBase<string>
    {
        /// <summary>
        /// The repository.
        /// </summary>
        protected readonly IGenericRepository<TEntity> Repository;

        /// <summary>
        /// The mapper.
        /// </summary>
        protected readonly IMapper Mapper;

        protected GenericDbServiceBase(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            this.Repository = repository;
            this.Mapper = mapper;
        }

        public Task<int> CountAsync()
        {
            return this.Repository.CountAsync();
        }

        public List<TDomainObject> GetPage(int pageIndex, int pageSize)
        {
            var customers = this.Repository.GetPageAsync(pageIndex, pageSize).Result;
            return this.Mapper.Map<List<TDomainObject>>(customers);
        }

        public Task<int> Insert(TDomainObject entity)
        {
            var tEntity = this.Mapper.Map<TEntity>(entity);
            return this.Repository.Insert(tEntity);
        }

        public Task<int> Update(TDomainObject entity)
        {
            var tEntity = this.Mapper.Map<TEntity>(entity);
            return this.Repository.Update(tEntity);
        }

        public IEnumerable<TDomainObject> GetPage(int pageIndex, int pageSize, Expression<Func<TDomainObject, bool>> predicate, out int count)
        {
            var efExpression = this.Mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);
            IEnumerable<TEntity> customers = this.Repository.GetPage(pageIndex, pageSize, efExpression, out count);
            return this.Mapper.Map<IEnumerable<TEntity>, List<TDomainObject>>(customers);
        }

        public void Dispose()
        {
            this.Repository.Dispose();
        }
    }
}
