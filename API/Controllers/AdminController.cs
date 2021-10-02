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
        private readonly IUnitOfWork _uow;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> GetBanks([FromQuery] BankParams bankParams)
        {
            var banks = await _uow.BankRepository.GetBanksForAdmin(bankParams);
            Response.AddPaginationHeader(banks.PageNumber, banks.PageSize, banks.TotalPages, banks.TotalCount);
            return Ok(banks);
        }

        [HttpGet("{bankId}")]
        public async Task<ActionResult> GetBank(int bankId)
        {
            if (!await _uow.BankRepository.IsBankExist(bankId))
                return BadRequest("Bank doesn't exist.");
            var bank = await _uow.BankRepository.GetBankForAdmin(bankId);
            return bank == null ? BadRequest("Bank not available") : Ok(bank);
        }

        [HttpPost("register-bank")]
        public async Task<ActionResult> RegisterBank(BankRegisterDto registerDto)
        {
            if (!await _uow.UserRepository.UserExist(registerDto.BankAdmin))
                return BadRequest($"The user {registerDto.BankAdmin} doesn't exist.");
            var userId = await _uow.UserRepository.GetUserIdByUserName(registerDto.BankAdmin);
            var bank = _uow.BankRepository.RegisterBank(registerDto, userId);
            if (!await _uow.SaveChanges())
                return BadRequest("Failed to register bank.");
            await _uow.RoleRepository.ResetBankUserRole(new[] { userId });
            return Ok(bank.Id);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBank(BankModeratorDto bank)
        {
            if (!await _uow.BankRepository.IsBankExist(bank.Id))
                return BadRequest("Bank doesn't exist.");
            await _uow.BankRepository.UpdateBank(bank);
            if (!await _uow.SaveChanges())
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
            var existingUsers = await _uow.UserRepository.GetUserNames(moderators);
            if (moderators.Count != existingUsers.Count)
            {
                var nonExistingUsers = moderators.Except(existingUsers, StringComparer.OrdinalIgnoreCase);
                return BadRequest($"The user(s) {string.Join(", ", nonExistingUsers.ToArray())} are not available.");
            }

            return await _uow.RoleRepository.UpdateBankRoles(updateDto) ?
                Ok(await _uow.BankRepository.GetBankForAdmin(updateDto.BankId)) :
                BadRequest("Failed to update roles");
        }

        [HttpGet("roles"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAdminRoles([FromQuery] AdminRoleParams roleParams)
        {
            if (!await _uow.BankRepository.IsAdmin(HttpContext.User.GetUserId()))
                return BadRequest("You are not admin");

            var roles = await _uow.RoleRepository.GetAdminRoles(roleParams);
            Response.AddPaginationHeader(roles.PageNumber, roles.PageSize, roles.TotalPages, roles.TotalCount);
            return Ok(roles);
        }

        [HttpPost("roles"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddAdminRole(AdminRoleDto roleDto)
        {
            if (!await _uow.BankRepository.IsAdmin(HttpContext.User.GetUserId()))
                return BadRequest("You are not admin");

            var result = await _uow.RoleRepository.AddAdminRole(roleDto);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }

            return Ok(await _uow.RoleRepository.GetAdminRole(roleDto));
        }

        [HttpDelete("roles"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveAdminRole(AdminRoleDto roleDto)
        {
            if (!await _uow.BankRepository.IsAdmin(HttpContext.User.GetUserId()))
                return BadRequest("You are not admin");
            if (roleDto.UserName.Equals(HttpContext.User.GetUserName(), StringComparison.OrdinalIgnoreCase))
                return BadRequest("You cannot remove your role.");

            var result = await _uow.RoleRepository.RemoveAdminRole(roleDto);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }
            return Ok();
        }
    }
}
