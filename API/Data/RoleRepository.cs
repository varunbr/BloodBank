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
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;

        public RoleRepository(DataContext dataContext, IMapper mapper, IPhotoService photoService,
            UserManager<AppUser> userManager) : base(dataContext, mapper, photoService)
        {
            _userManager = userManager;
            _context = dataContext;
        }

        public async Task<bool> UpdateBankRoles(BankRoleUpdateDto updateDto, int userId = 0)
        {
            var bank = await DataContext.Banks
                .Include(b => b.Moderators)
                .SingleAsync(b => b.Id == updateDto.BankId);

            var modifiedIds = new HashSet<int>();

            foreach (var moderator in updateDto.Moderators)
            {
                moderator.UserId = await GetUserIdByUserName(moderator.UserName);
                modifiedIds.Add(moderator.UserId);
                if (moderator.UserId == userId) //Prevent Role update of User
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
                await ResetBankUserRole(modifiedIds);
                return true;
            }

            return false;
        }

        public async Task ResetBankUserRole(IEnumerable<int> userIds)
        {
            foreach (var userId in userIds)
            {
                var currentRoles = await _context.Moderator
                    .Where(m => m.UserId == userId)
                    .GroupBy(m => m.Type)
                    .Select(x => x.Key).ToListAsync();

                var removedRoles = Util.GetBankRoles().Except(currentRoles).ToList();

                var user = await _context.Users.FindAsync(userId);

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

        public async Task<PagedList<AdminRoleDto>> GetAdminRoles(AdminRoleParams roleParams)
        {
            var query = DataContext.UserRoles.AsQueryable();
            query = query.BuildQuery(roleParams);
            return await PagedList<AdminRoleDto>.CreateAsync(
                query.ProjectTo<AdminRoleDto>(Mapper.ConfigurationProvider).AsNoTracking(),
                roleParams.PageSize, roleParams.PageNumber);
        }

        public async Task<AdminRoleDto> GetAdminRole(AdminRoleDto roleDto)
        {
            var query = DataContext.UserRoles.AsQueryable();
            query = query.Where(r => r.Role.Name == roleDto.Role && r.User.UserName == roleDto.UserName);
            return await query.ProjectTo<AdminRoleDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
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
    }
}
