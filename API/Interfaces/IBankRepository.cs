using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IBankRepository
    {
        Task<PagedList<BankDto>> GetBanks(BankParams userParams);
        Task<PagedList<BankModeratorDto>> GetBanksForModeration(BankParams bankParams, int userId);
        Task<BankModeratorDto> GetBankForModeration(int bankId, int userId);
        Task UpdateBloodData(BloodGroupUpdateDto updateDto, int userId);
        Task<bool> IsBankExist(int bankId);
        Task<bool> IsBankModerator(int bankId, int userId);
        Task<bool> IsBankAdmin(int bankId, int userId);
        Task<bool> IsAdmin(int userId);
        Task<PagedList<BankModeratorDto>> GetBanksForAdmin(BankParams bankParams);
        Task<BankModeratorDto> GetBankForAdmin(int bankId);
        Task<Bank> UpdateBank(BankModeratorDto bankDto);
        Bank RegisterBank(BankRegisterDto registerDto, int adminUserId);
        Task<string> UpdateBankPhoto(int bankId, IFormFile file);
        Task DeleteBankPhoto(int bankId);
    }
}
