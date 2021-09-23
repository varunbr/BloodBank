using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<PagedList<MemberDto>> GetUsers(UserParams userParams);
        Task<IList<string>> GetUserNames(IEnumerable<string> userNames);
        Task<int> GetUserIdByUserName(string userName);
        Task<string> GetUserNameById(int id);
    }
}
