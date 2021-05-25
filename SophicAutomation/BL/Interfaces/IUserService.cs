using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Dto;

namespace BL.Interfaces
{
    public interface IUserService : IGenericService<UserDto>
    {
        public List<UserDto> GetPage(int pageIndex, int pageSize, string searchString, out int count);

        public UserDto FindById(string id);

        public bool Any(string id);

        public Task<int> Delete(string id);

        public Task<int> UpdatePersonalInfo(UserDto applicationUser);
    }
}
