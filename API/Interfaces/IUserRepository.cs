using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<PagedList<MemberDto>> GetUsers(UserParams userParams);
    }
}
