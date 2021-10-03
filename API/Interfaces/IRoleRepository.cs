using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using Microsoft.AspNetCore.Identity;

namespace API.Interfaces
{
    public interface IRoleRepository
    {
        Task AddBankRole(string role, int bankId, int userId);
        Task RemoveBankRole(string role, int bankId, int userId);
        Task UpdateUserRole(int userId, string role, bool add = true);
        Task<PagedList<RoleDto>> GetAdminRoles(AdminRoleParams roleParams);
        Task<RoleDto> GetAdminRole(RoleDto roleDto);
        Task<IdentityResult> AddAdminRole(RoleDto roleDto);
        Task<IdentityResult> RemoveAdminRole(RoleDto roleDto);
        Task<List<RoleDto>> GetRolesForAbout();
    }
}
