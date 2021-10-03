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

        public async Task AddBankRole(string role, int bankId, int userId)
        {
            var bank = await DataContext.Banks
                .Include(b => b.Moderators)
                .SingleAsync(b => b.Id == bankId);

            bank.Moderators.Add(new Moderator
            {
                UserId = userId,
                Type = role
            });
        }

        public async Task RemoveBankRole(string role, int bankId, int userId)
        {
            var bank = await DataContext.Banks
                .Include(b => b.Moderators)
                .SingleAsync(b => b.Id == bankId);

            var moderator = bank.Moderators.Single(m => m.UserId == userId);
            bank.Moderators.Remove(moderator);
        }

        public async Task UpdateUserRole(int userId, string role, bool add = true)
        {
            var currentRoles = await _context.Moderator
                .Where(m => m.UserId == userId)
                .GroupBy(m => m.Type)
                .Select(x => x.Key).ToListAsync();

            var user = await _context.Users.FindAsync(userId);
            if (add)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            else if(!currentRoles.Contains(role))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
        }

        public async Task<PagedList<RoleDto>> GetAdminRoles(AdminRoleParams roleParams)
        {
            var query = DataContext.UserRoles.AsQueryable();
            query = query.BuildQuery(roleParams);
            return await PagedList<RoleDto>.CreateAsync(
                query.ProjectTo<RoleDto>(Mapper.ConfigurationProvider).AsNoTracking(),
                roleParams.PageSize, roleParams.PageNumber);
        }

        public async Task<RoleDto> GetAdminRole(RoleDto roleDto)
        {
            var query = DataContext.UserRoles.AsQueryable();
            query = query.Where(r => r.Role.Name == roleDto.Role && r.User.UserName == roleDto.UserName);
            return await query.ProjectTo<RoleDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IdentityResult> AddAdminRole(RoleDto roleDto)
        {
            if (!Util.GetAdminRoles().Contains(roleDto.Role))
                return IdentityResult.Failed(new IdentityError { Description = "Invalid role." });
            var user = await _userManager.FindByNameAsync(roleDto.UserName);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User doesn't exist." });

            return await _userManager.AddToRoleAsync(user, roleDto.Role);
        }

        public async Task<IdentityResult> RemoveAdminRole(RoleDto roleDto)
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
