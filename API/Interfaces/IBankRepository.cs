using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;

namespace API.Interfaces
{
    public interface IBankRepository
    {
        Task<PagedList<BankDto>> GetBanks(BankParams userParams);
        Task<PagedList<BankModeratorDto>> GetBanksForModeration(BankParams bankParams, int userId);
        Task<BankModeratorDto> GetBankForModeration(int bankId, int userId);
        Task<bool> UpdateBloodData(BloodGroupUpdateDto updateDto, int userId);
        Task<bool> UpdateRoles(BankRoleUpdateDto updateDto, int userId = 0);
        Task<bool> IsBankExist(int bankId);
        Task<bool> IsBankModerator(int bankId, int userId);
        Task<bool> IsBankAdmin(int bankId, int userId);
        Task<PagedList<BankModeratorDto>> GetBanksForAdmin(BankParams bankParams);
        Task<BankModeratorDto> GetBankForAdmin(int bankId);
        Task<bool> UpdateBank(BankModeratorDto bankDto);
    }
}
