using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BL.Dto;
using BL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using LinqKit;

namespace BL.Services
{
    public class UserService : GenericDbServiceBase<ApplicationUser, UserDto>, IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper)
        {
            this.userRepository = repository;
        }

        public List<UserDto> GetPage(int pageIndex, int pageSize, string searchString, out int count)
        {
            IEnumerable<ApplicationUser> users = this.userRepository.GetPage(pageIndex, pageSize, this.BuildExpressionToSearchByFields(searchString), out count);
            return this.Mapper.Map<IEnumerable<ApplicationUser>, List<UserDto>>(users);
        }

        public UserDto FindById(string id)
        {
            var user = this.userRepository.FindById(id);
            return this.Mapper.Map<UserDto>(user);
        }

        public bool Any(string id)
        {
            return this.userRepository.Any(id);
        }

        public Task<int> Delete(string id)
        {
            return this.userRepository.Delete(id);
        }

        public Task<int> UpdatePersonalInfo(UserDto userDto)
        {
            var applicationUser = this.Mapper.Map<ApplicationUser>(userDto);
            return this.userRepository.UpdatePersonalInfo(applicationUser);
        }

        /// <summary>
        /// Build expression to search by Name, Surname, City, Street, Zip
        /// </summary>
        /// <param name="searchString">the value for searching operation (contains) </param>
        /// <returns>Predicate </returns>
        private Expression<Func<ApplicationUser, bool>> BuildExpressionToSearchByFields(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return item => true;
            }

            var predicate = PredicateBuilder.
                Or<ApplicationUser>(user => user.Name != null && user.Name.Contains(searchString),
                    user => user.Surname != null && user.Surname.Contains(searchString))
                .Or(user => user.UserName != null && user.UserName.Contains(searchString))
                .Or(user => user.Email != null && user.Email.Contains(searchString))
                .Or(user => user.City != null && user.City.Contains(searchString))
                .Or(user => user.Street != null && user.Street.Contains(searchString))
                .Or(user => user.Zip != null && user.Zip.Contains(searchString));
            return predicate;
        }
    }
}
