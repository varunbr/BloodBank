using System;
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

            CreateMap<Address, MemberDto>();
            CreateMap<Photo, MemberDto>();
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => Util.GetAge(src.DateOfBirth)))
                .IncludeMembers(s => s.Address, s => s.Photo);

            CreateMap<Address, BankDto>();
            CreateMap<Photo, BankDto>();
            CreateMap<BloodGroup, BloodGroupDto>();
            CreateMap<Bank, BankDto>()
                .IncludeMembers(s => s.Address, s => s.Photo)
                .ForMember(dest => dest.BloodGroups,
                    opt => opt.MapFrom(src => src.BloodGroups));

            CreateMap<Address, BankModeratorDto>();
            CreateMap<Photo, BankModeratorDto>();
            CreateMap<Moderator, ModeratorDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<Bank, BankModeratorDto>()
                .IncludeMembers(s => s.Address, s => s.Photo)
                .ForMember(dest => dest.BloodGroups,
                    opt => opt.MapFrom(src => src.BloodGroups));

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}
