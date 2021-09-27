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

        public BankRepository(DataContext dataContext, IMapper mapper, IUserRepository userRepository,
            UserManager<AppUser> userManager) : base(dataContext, mapper)
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

        public async Task<bool> UpdateBloodData(BloodGroupUpdateDto updateDto, int userId)
        {
            var bank = await DataContext.Banks
                .Include(b => b.BloodGroups)
                .FirstOrDefaultAsync(b => b.Id == updateDto.BankId);

            foreach (var gp in updateDto.Groups)
            {
                bank.BloodGroups.First(g => g.Group.Equals(gp.Group)).Value = gp.Value;
            }

            bank.LastUpdated = DateTime.UtcNow;

            return await DataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRoles(BankRoleUpdateDto updateDto, int userId = 0)
        {
            var bank = await DataContext.Banks
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

            if (await DataContext.SaveChangesAsync() > 0)
            {
                await UpdateUserRole(modifiedIds);
                return true;
            }

            return false;
        }

        public async Task<bool> IsBankExist(int bankId)
        {
            return await DataContext.Banks.AnyAsync(b => b.Id == bankId);
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

        public async Task<bool> IsAdmin(int userId)
        {
            return await DataContext.UserRoles.AnyAsync(r => r.UserId == userId && r.Role.Name == "Admin");
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

        public async Task<bool> UpdateBank(BankModeratorDto bankDto)
        {
            var bank = await DataContext.Banks
                .Include(b => b.Address)
                .FirstOrDefaultAsync(b => b.Id == bankDto.Id);

            Mapper.Map(bankDto, bank);
            bank.LastUpdated = DateTime.UtcNow;

            return await DataContext.SaveChangesAsync() > 0;
        }

        public async Task<PagedList<AdminRoleDto>> GetAdminRoles(AdminRoleParams roleParams)
        {
            var query = DataContext.UserRoles.AsQueryable();
            query = query.BuildQuery(roleParams);
            return await PagedList<AdminRoleDto>.CreateAsync(
                query.ProjectTo<AdminRoleDto>(Mapper.ConfigurationProvider).AsNoTracking(),
                roleParams.PageSize, roleParams.PageNumber);
        }

        public async Task<IdentityResult> AddAdminRole(AdminRoleDto roleDto)
        {
            if (!Util.GetAdminRoles().Contains(roleDto.Role))
                return IdentityResult.Failed(new IdentityError { Description = "Invalid role." });
            var user = await _userManager.FindByNameAsync(roleDto.UserName);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User doesn't exist." });

            return await _userManager.AddToRoleAsync(user, roleDto.Role);
        }

        public async Task<IdentityResult> RemoveAdminRole(AdminRoleDto roleDto)
        {
            if (!Util.GetAdminRoles().Contains(roleDto.Role))
                return IdentityResult.Failed(new IdentityError { Description = "Invalid role." });
            var user = await _userManager.FindByNameAsync(roleDto.UserName);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User doesn't exist." });

            return await _userManager.RemoveFromRoleAsync(user, roleDto.Role);
        }

        public async Task<AdminRoleDto> GetAdminRole(AdminRoleDto roleDto)
        {
            var query = DataContext.UserRoles.AsQueryable();
            query = query.Where(r => r.Role.Name == roleDto.Role && r.User.UserName == roleDto.UserName);
            return await query.ProjectTo<AdminRoleDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<int> RegisterBank(BankRegisterDto registerDto)
        {
            var bank = Bank.Create();
            var userId = await _userRepository.GetUserIdByUserName(registerDto.BankAdmin);
            bank = Mapper.Map(registerDto, bank);
            bank.Moderators.Add(new Moderator
            {
                UserId = userId,
                Type = "BankAdmin"

            });
            DataContext.Banks.Add(bank);
            await DataContext.SaveChangesAsync();
            await UpdateUserRole(new[] { userId });
            return bank.Id;
        }
    }
}
