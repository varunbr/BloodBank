using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using Microsoft.AspNetCore.Identity;

namespace API.Interfaces
{
    public interface IRoleRepository
    {
        Task<bool> UpdateBankRoles(BankRoleUpdateDto updateDto, int userId = 0);
        Task ResetBankUserRole(IEnumerable<int> userIds);
        Task<PagedList<AdminRoleDto>> GetAdminRoles(AdminRoleParams roleParams);
        Task<AdminRoleDto> GetAdminRole(AdminRoleDto roleDto);
        Task<IdentityResult> AddAdminRole(AdminRoleDto roleDto);
        Task<IdentityResult> RemoveAdminRole(AdminRoleDto roleDto);
    }
}
