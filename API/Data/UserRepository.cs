using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService) : base(dataContext, mapper, photoService)
        {
        }

        public async Task<MemberDto> GetUser(string userName)
        {
            return await DataContext.Users
                .Include(u => u.Address)
                .Include(u => u.Photo)
                .ProjectTo<MemberDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<PagedList<MemberDto>> GetUsers(UserParams userParams)
        {
            var query = DataContext.Users.AsQueryable();

            query = query.BuildQuery(userParams);
            return await PagedList<MemberDto>.CreateAsync(
                query.ProjectTo<MemberDto>(Mapper.ConfigurationProvider).AsNoTracking(),
                userParams.PageSize, userParams.PageNumber);
        }

        public async Task<IList<string>> GetUserNames(IEnumerable<string> userNames)
        {
            return await DataContext.Users.Where(u => userNames.Contains(u.UserName))
                .Select(u => u.UserName)
                .ToListAsync();
        }

        public async Task<bool> LogUserActive(int id)
        {
            var user = await DataContext.Users.FindAsync(id);
            user.LastActive = DateTime.UtcNow;
            return await DataContext.SaveChangesAsync() > 0;
        }

        public async Task<UserProfileDto> GetProfile(int id)
        {
            var user = await DataContext.Users.AsQueryable()
                .Where(u => u.Id == id)
                .ProjectTo<UserProfileDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<AppUser> UpdateProfile(UserProfileDto profileDto)
        {
            var user = await DataContext.Users
                .Include(x => x.Address)
                .Include(x => x.Photo)
                .FirstOrDefaultAsync(x => x.Id == profileDto.Id);
            Mapper.Map(profileDto, user);
            return user;
        }

        public async Task<string> UpdateUserPhoto(IFormFile file, int userId)
        {
            return await UpdatePhoto(file, userId, null);
        }

        public async Task DeleteUserPhoto(int userId)
        {
            await DeletePhoto(userId, null);
        }
    }
}
