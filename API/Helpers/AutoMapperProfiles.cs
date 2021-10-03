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
            CreateMap<Photo, RoleDto>();
            CreateMap<Moderator, RoleDto>()
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src => src.User.Photo.Url))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<Bank, BankModeratorDto>()
                .IncludeMembers(s => s.Address, s => s.Photo)
                .ForMember(dest => dest.BloodGroups,
                    opt => opt.MapFrom(src => src.BloodGroups));

            CreateMap<BankModeratorDto, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<BankModeratorDto, Bank>()
                .ForMember(dest => dest.BloodGroups, opt => opt.Ignore())
                .ForMember(dest => dest.Moderators, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src));

            CreateMap<AppUserRole, RoleDto>()
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src => src.User.Photo.Url))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.User.Name));

            CreateMap<BankRegisterDto, Address>();
            CreateMap<BankRegisterDto, Bank>()
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => src));

            CreateMap<Address, UserProfileDto>();
            CreateMap<Photo, UserProfileDto>();
            CreateMap<AppUser, UserProfileDto>()
                .IncludeMembers(s => s.Address, s => s.Photo);

            CreateMap<UserProfileDto, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UserProfileDto, AppUser>()
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName.ToLower()));

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}
