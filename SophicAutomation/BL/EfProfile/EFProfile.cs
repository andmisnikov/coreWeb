using AutoMapper;
using BL.Dto;
using DAL.Entities;

namespace BL.EfProfile
{
    public class EfProfile : Profile
    {
        public EfProfile()
        {
            this.CreateMap<UserDto, ApplicationUser>().ReverseMap();
        }
    }
}
