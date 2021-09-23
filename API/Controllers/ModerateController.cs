using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Moderator,BankAdmin,BankModerator")]
    public class ModerateController : BaseController
    {
        private readonly IBankRepository _bankRepository;
        private readonly IUserRepository _userRepository;

        public ModerateController(IBankRepository bankRepository, IUserRepository userRepository)
        {
            _bankRepository = bankRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetBanks([FromQuery] BankParams bankParams)
        {
            bankParams.BloodGroup = HttpUtility.UrlDecode(bankParams.BloodGroup);
            var userId = HttpContext.User.GetUserId();
            var banks = await _bankRepository.GetBanksForModeration(bankParams, userId);
            Response.AddPaginationHeader(banks.PageNumber, banks.PageSize, banks.TotalPages, banks.TotalCount);
            return Ok(banks);
        }

        [HttpGet("{bankId}")]
        public async Task<ActionResult> GetBank(int bankId)
        {
            var userId = HttpContext.User.GetUserId();

            if (!await _bankRepository.IsBankModerator(bankId, userId))
                return BadRequest("You are not moderator for this bank!");

            var bank = await _bankRepository.GetBankForModeration(bankId, userId);
            return Ok(bank);
        }

        [HttpPut("blood-data")]
        public async Task<ActionResult> UpdateBloodData(BloodGroupUpdateDto updateDto)
        {
            var userId = HttpContext.User.GetUserId();

            if (!await _bankRepository.IsBankModerator(updateDto.BankId, userId))
                return BadRequest("You are not moderator for this bank!");

            if (!Validator.ValidateBloodData(updateDto))
                return BadRequest("Invalid data.");

            var result = await _bankRepository.UpdateBloodData(updateDto, userId);
            if (result == null)
                return BadRequest("Failed to update.");

            return Ok(result);
        }

        [HttpPut("bank-role"), Authorize(Roles = "BankAdmin")]
        public async Task<ActionResult> UpdateBankRoles(BankRoleUpdateDto updateDto)
        {
            var userId = HttpContext.User.GetUserId();

            if (!await _bankRepository.IsBankAdmin(updateDto.BankId, userId))
                return BadRequest("You are not admin for this bank!");

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

            var result = await _bankRepository.UpdateRoles(updateDto, userId);

            return result == null ?
                BadRequest("Failed to update roles") :
                Ok(result);
        }
    }
}
