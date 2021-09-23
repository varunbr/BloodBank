using System.Collections.Generic;
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
        Task<BankModeratorDto> UpdateBloodData(BloodGroupUpdateDto updateDto, int userId);
        Task<BankModeratorDto> UpdateRoles(BankRoleUpdateDto updateDto, int userId);
        Task<bool> IsBankModerator(int bankId, int userId);
        Task<bool> IsBankAdmin(int bankId, int userId);
    }
}
