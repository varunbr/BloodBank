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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BankRepository : BaseRepository, IBankRepository
    {
        public BankRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService) : base(dataContext, mapper, photoService)
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

        public async Task UpdateBloodData(BloodGroupUpdateDto updateDto, int userId)
        {
            var bank = await DataContext.Banks
                .Include(b => b.BloodGroups)
                .FirstOrDefaultAsync(b => b.Id == updateDto.BankId);

            foreach (var gp in updateDto.Groups)
            {
                bank.BloodGroups.First(g => g.Group.Equals(gp.Group)).Value = gp.Value;
            }

            bank.LastUpdated = DateTime.UtcNow;
        }

        public async Task<PagedList<BankModeratorDto>> GetBanksForAdmin(BankParams bankParams)
        {
            var query = DataContext.Banks.AsQueryable();

            query = query.BuildQuery(bankParams);
            return await PagedList<BankModeratorDto>.CreateAsync(
                query.ProjectTo<BankModeratorDto>(Mapper.ConfigurationProvider).AsNoTracking(),
                bankParams.PageSize, bankParams.PageNumber);
        }

        public async Task<BankModeratorDto> GetBankForAdmin(int bankId)
        {
            return await DataContext.Banks.AsQueryable()
                .Where(b => b.Id == bankId)
                .ProjectTo<BankModeratorDto>(Mapper.ConfigurationProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<Bank> UpdateBank(BankModeratorDto bankDto)
        {
            var bank = await DataContext.Banks
                .Include(b => b.Address)
                .FirstOrDefaultAsync(b => b.Id == bankDto.Id);

            Mapper.Map(bankDto, bank);
            bank.LastUpdated = DateTime.UtcNow;

            return bank;
        }

        public Bank RegisterBank(BankRegisterDto registerDto, int adminUserId)
        {
            var bank = Bank.Create();
            bank = Mapper.Map(registerDto, bank);
            bank.Moderators.Add(new Moderator
            {
                UserId = adminUserId,
                Type = "BankAdmin"
            });
            bank.LastUpdated = DateTime.UtcNow;
            DataContext.Banks.Add(bank);
            return bank;
        }

        public async Task<string> UpdateBankPhoto(int bankId, IFormFile file)
        {
            return await UpdatePhoto(file, null, bankId);
        }

        public async Task DeleteBankPhoto(int bankId)
        {
            await DeletePhoto(null, bankId);
        }
    }
}
