using System;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BankRepository : Repository, IBankRepository
    {
        public BankRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
        }

        public async Task<PagedList<BankDto>> GetBanks(BankParams bankParams)
        {
            var query = DataContext.Banks.AsQueryable();

            query = query.BuildQuery(bankParams);
            return await PagedList<BankDto>.CreateAsync(
                query.ProjectTo<BankDto>(Mapper.ConfigurationProvider).AsNoTracking(),
                bankParams.PageSize, bankParams.PageNumber);
        }

        public async Task<PagedList<BankModeratorDto>> GetBanksForModeration(BankParams bankParams, int userId)
        {
            var query = DataContext.Moderator
                .Where(m => m.UserId == userId)
                .Select(m => m.Bank).AsQueryable();

            query = query.BuildQuery(bankParams);
            var result = await PagedList<BankModeratorDto>.CreateAsync(
                query.ProjectTo<BankModeratorDto>(Mapper.ConfigurationProvider).AsNoTracking(),
                bankParams.PageSize, bankParams.PageNumber);

            foreach (var item in result)
            {
                item.UpdateRole(userId);
            }
            return result;
        }

        public async Task<BankModeratorDto> GetBankForModeration(int bankId, int userId)
        {
            var bank = await DataContext.Banks.AsQueryable()
                .Where(b => b.Id == bankId)
                .ProjectTo<BankModeratorDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            bank?.UpdateRole(userId);
            return bank;
        }

        public async Task<BankModeratorDto> UpdateBloodData(BloodGroupUpdateDto updateDto, int userId)
        {
            var bank = await DataContext.Banks
                .Include(b => b.Address)
                .Include(b => b.Photo)
                .Include(b => b.BloodGroups)
                .Include(b => b.Moderators)
                .FirstOrDefaultAsync(b => b.Id == updateDto.BankId);

            foreach (var gp in updateDto.Groups)
            {
                bank.BloodGroups.First(g => g.Group.Equals(gp.Group)).Value = gp.Value;
            }

            bank.LastUpdated = DateTime.UtcNow;

            if (await DataContext.SaveChangesAsync() <= 0) return null;

            var bankDto = Mapper.Map<Bank, BankModeratorDto>(bank);
            bankDto.UpdateRole(userId);

            return bankDto;
        }

        public async Task<bool> IsBankModerator(int bankId, int userId)
        {
            return await DataContext.Moderator.AnyAsync(m => m.UserId == userId && m.BankId == bankId);
        }
    }
}
