using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;

namespace API.Interfaces
{
    public interface IBankRepository
    {
        Task<PagedList<BankDto>> GetUsers(BankParams userParams);
    }
}
