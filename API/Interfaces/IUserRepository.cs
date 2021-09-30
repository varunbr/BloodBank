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
        Task<PagedList<MemberDto>> GetUsers(UserParams userParams);
        Task<IList<string>> GetUserNames(IEnumerable<string> userNames);
        Task<int> GetUserIdByUserName(string userName);
        Task<bool> LogUserActive(int id);
        Task<string> GetUserNameById(int id);
        Task<UserProfileDto> GetProfile(int id);
        Task<UserProfileDto> UpdateProfile(UserProfileDto profileDto);
        Task<string> UpdateUserPhoto(int userId, IFormFile file);
        Task<bool> DeleteUserPhoto(int userId);
    }
}
