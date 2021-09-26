using System;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminController : BaseController
    {
        private readonly IBankRepository _bankRepository;
        private readonly IUserRepository _userRepository;

        public AdminController(IBankRepository bankRepository, IUserRepository userRepository)
        {
            _bankRepository = bankRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetBanks([FromQuery] BankParams bankParams)
        {
            var banks = await _bankRepository.GetBanksForAdmin(bankParams);
            Response.AddPaginationHeader(banks.PageNumber, banks.PageSize, banks.TotalPages, banks.TotalCount);
            return Ok(banks);
        }

        [HttpGet("{bankId}")]
        public async Task<ActionResult> GetBank(int bankId)
        {
            if (!await _bankRepository.IsBankExist(bankId))
                return BadRequest("Bank doesn't exist.");
            var bank = await _bankRepository.GetBankForAdmin(bankId);
            return bank == null ? BadRequest("Bank not available") : Ok(bank);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBank(BankModeratorDto bank)
        {
            if (!await _bankRepository.IsBankExist(bank.Id))
                return BadRequest("Bank doesn't exist.");

            if (!await _bankRepository.UpdateBank(bank))
                return BadRequest("Failed to update.");
            return await GetBank(bank.Id);
        }

        [HttpPut("bank-role")]
        public async Task<ActionResult> UpdateBankRoles(BankRoleUpdateDto updateDto)
        {
            if (!Validator.ValidateBankRole(updateDto))
                return BadRequest("Invalid Role Type.");

            if (updateDto.Moderators.Count != updateDto.Moderators.GroupBy(u => u.UserName, StringComparer.OrdinalIgnoreCase).Count())
                return BadRequest("Duplicate users not allowed.");

            var moderators = updateDto.Moderators.Select(m => m.UserName).ToList();
            var existingUsers = await _userRepository.GetUserNames(moderators);
            if (moderators.Count != existingUsers.Count)
            {
                var nonExistingUsers = moderators.Except(existingUsers, StringComparer.OrdinalIgnoreCase);
                return BadRequest($"The user(s) {string.Join(", ", nonExistingUsers.ToArray())} are not available.");
            }

            return await _bankRepository.UpdateRoles(updateDto) ?
                Ok(await _bankRepository.GetBankForAdmin(updateDto.BankId)) :
                BadRequest("Failed to update roles");
        }

        [HttpGet("roles"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAdminRoles([FromQuery] AdminRoleParams roleParams)
        {
            if (!await _bankRepository.IsAdmin(HttpContext.User.GetUserId()))
                return BadRequest("You are not admin");

            var roles = await _bankRepository.GetAdminRoles(roleParams);
            Response.AddPaginationHeader(roles.PageNumber, roles.PageSize, roles.TotalPages, roles.TotalCount);
            return Ok(roles);
        }

        [HttpPost("roles"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddAdminRole(AdminRoleDto roleDto)
        {
            if (!await _bankRepository.IsAdmin(HttpContext.User.GetUserId()))
                return BadRequest("You are not admin");

            var result =await _bankRepository.AddAdminRole(roleDto);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }

            return Ok(await _bankRepository.GetAdminRole(roleDto));
        }

        [HttpDelete("roles"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveAdminRole(AdminRoleDto roleDto)
        {
            if (!await _bankRepository.IsAdmin(HttpContext.User.GetUserId()))
                return BadRequest("You are not admin");
            if (roleDto.UserName.Equals(HttpContext.User.GetUserName(), StringComparison.OrdinalIgnoreCase))
                return BadRequest("You cannot remove your role.");

            var result = await _bankRepository.RemoveAdminRole(roleDto);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            return Ok();
        }
    }
}
