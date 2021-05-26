using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Models;

namespace DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetPage(int pageIndex, int pageSize, Expression<Func<ApplicationUser, bool>> predicate, out int count);
        ApplicationUser FindById(string id);
        public bool Any(string id);
        public Task<int> Delete(string id);
        public Task<int> UpdatePersonalInfo(ApplicationUser applicationUser);
        public Task<List<UsersRegisteredPerDay>> UsersRegisteredPerDay();
    }
}
