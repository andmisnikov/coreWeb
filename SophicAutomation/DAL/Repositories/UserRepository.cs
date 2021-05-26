using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<ApplicationUser> GetPage(int pageIndex, int pageSize, Expression<Func<ApplicationUser, bool>> predicate, out int count)
        {
            var result = this.databaseSet.Where(predicate);
            count = result.ToList().Count;
            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public ApplicationUser FindById(string id)
        {
            return this.databaseSet.SingleOrDefault(e => e.Id == id);
        }

        public bool Any(string id)
        {
            return this.databaseSet.Any(e => e.Id == id);
        }

        public Task<int> Delete(string id)
        {
            var user = this.databaseSet.Find(id);
            if (user == null)
            {
                return Task.FromResult(0);
            }

            if (this.databaseContext.Entry(user).State == EntityState.Detached)
            {
                this.databaseSet.Attach(user);
            }

            this.databaseSet.Remove(user);
            return this.databaseContext.SaveChangesAsync();
        }

        public Task<int> UpdatePersonalInfo(ApplicationUser appUser)
        {
            this.databaseSet.Attach(appUser);
            this.databaseContext.Entry(appUser).Property(p => p.Name).IsModified = true;
            this.databaseContext.Entry(appUser).Property(p => p.Surname).IsModified = true;
            this.databaseContext.Entry(appUser).Property(p => p.Street).IsModified = true;
            this.databaseContext.Entry(appUser).Property(p => p.Zip).IsModified = true;
            this.databaseContext.Entry(appUser).Property(p => p.City).IsModified = true;
            return this.databaseContext.SaveChangesAsync();
        }

        public Task<List<UsersRegisteredPerDay>> UsersRegisteredPerDay()
        {
            return this.databaseSet
                .Where(w => w.RegisterDate != null)
                .GroupBy(g => g.RegisterDate.Value.Date)
                .OrderBy(o => o.Key)
                .Select(grp => new UsersRegisteredPerDay {Day = grp.Key, UsersRegistered = grp.Count()}).ToListAsync();
        }
    }
}
