using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<int> GetUserIdByUserName(string userName);
        Task<string> GetUserNameById(int id);
        Task<bool> UserExist(int id);
        Task<bool> UserExist(string userName);
        Task<MemberDto> GetUser(string userName);
        Task<PagedList<MemberDto>> GetUsers(UserParams userParams);
        Task<IList<string>> GetUserNames(IEnumerable<string> userNames);
        Task<bool> LogUserActive(int id);
        Task<UserProfileDto> GetProfile(int id);
        Task<AppUser> UpdateProfile(UserProfileDto profileDto);
        Task<string> UpdateUserPhoto(IFormFile file, int userId);
        Task DeleteUserPhoto(int userId);
    }
}
