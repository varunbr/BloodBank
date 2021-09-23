using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BankRepository : Repository, IBankRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;

        public BankRepository(DataContext dataContext, IMapper mapper, IUserRepository userRepository, UserManager<AppUser> userManager) : base(dataContext, mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
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

        public async Task<BankModeratorDto> UpdateRoles(BankRoleUpdateDto updateDto, int userId)
        {
            var bank = await DataContext.Banks
                .Include(b => b.Address)
                .Include(b => b.Photo)
                .Include(b => b.BloodGroups)
                .Include(b => b.Moderators)
                .SingleAsync(b => b.Id == updateDto.BankId);

            var modifiedIds = new HashSet<int>();

            foreach (var moderator in updateDto.Moderators)
            {
                moderator.UserId = await _userRepository.GetUserIdByUserName(moderator.UserName);
                modifiedIds.Add(moderator.UserId);
                if (moderator.UserId == userId)
                    continue;

                var existingModerator = bank.Moderators.FirstOrDefault(m => m.UserId == moderator.UserId);
                if (existingModerator == null) //Add
                {
                    bank.Moderators.Add(new Moderator
                    {
                        UserId = moderator.UserId,
                        Type = moderator.Type
                    });
                }
                else //Update
                {
                    existingModerator.Type = moderator.Type;
                }
            }

            for (var i = bank.Moderators.Count - 1; i >= 0; i--) //Remove
            {
                var moderator = bank.Moderators.ElementAt(i);

                if (moderator.UserId != userId &&
                    updateDto.Moderators.All(m => moderator.UserId != m.UserId))
                {
                    modifiedIds.Add(moderator.UserId);
                    bank.Moderators.Remove(moderator);
                }
            }

            var result = await DataContext.SaveChangesAsync();
            if (result <= 0) return null;

            await UpdateUserRole(modifiedIds);

            var bankDto = Mapper.Map<Bank, BankModeratorDto>(bank);
            bankDto.UpdateRole(userId);
            return bankDto;
        }

        public async Task<bool> IsBankModerator(int bankId, int userId)
        {
            return await DataContext.Moderator.AnyAsync(m => m.UserId == userId && m.BankId == bankId);
        }

        public async Task<bool> IsBankAdmin(int bankId, int userId)
        {
            return await DataContext.Moderator.AnyAsync(m => m.UserId == userId &&
                                                             m.BankId == bankId && m.Type == "BankAdmin");
        }

        private async Task UpdateUserRole(IEnumerable<int> userIds)
        {
            foreach (var userId in userIds)
            {
                var currentRoles = await DataContext.Moderator
                    .Where(m => m.UserId == userId)
                    .GroupBy(m => m.Type)
                    .Select(x => x.Key).ToListAsync();

                var removedRoles = Util.GetBankRoles().Except(currentRoles).ToList();

                var user = await DataContext.Users.FindAsync(userId);

                foreach (var removedRole in removedRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, removedRole);
                }

                foreach (var currentRole in currentRoles)
                {
                    await _userManager.AddToRoleAsync(user, currentRole);
                }
            }
        }
    }
}
