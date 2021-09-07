using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegisterDto, Address>();
            CreateMap<UserRegisterDto, AppUser>()
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName.ToLower()));
        }
    }
}
