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
    [Authorize(Roles = "BankAdmin,BankModerator")]
    public class ModerateController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public ModerateController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult> GetBanks([FromQuery] BankParams bankParams)
        {
            bankParams.BloodGroup = HttpUtility.UrlDecode(bankParams.BloodGroup);
            var userId = HttpContext.User.GetUserId();
            var banks = await _uow.BankRepository.GetBanksForModeration(bankParams, userId);
            Response.AddPaginationHeader(banks.PageNumber, banks.PageSize, banks.TotalPages, banks.TotalCount);
            return Ok(banks);
        }

        [HttpGet("{bankId}")]
        public async Task<ActionResult> GetBank(int bankId)
        {
            var userId = HttpContext.User.GetUserId();

            if (!await _uow.BankRepository.IsBankModerator(bankId, userId))
                return BadRequest("You are not moderator for this bank!");

            var bank = await _uow.BankRepository.GetBankForModeration(bankId, userId);
            return bank == null ? BadRequest("Bank not available") : Ok(bank);
        }

        [HttpPut("blood-data")]
        public async Task<ActionResult> UpdateBloodData(BloodGroupUpdateDto updateDto)
        {
            var userId = HttpContext.User.GetUserId();

            if (!await _uow.BankRepository.IsBankModerator(updateDto.BankId, userId))
                return BadRequest("You are not moderator for this bank!");

            if (!Validator.ValidateBloodData(updateDto))
                return BadRequest("Invalid data.");

            await _uow.BankRepository.UpdateBloodData(updateDto, userId);
            if (!await _uow.SaveChanges())
                return BadRequest("Failed to update.");

            return await GetBank(updateDto.BankId);
        }

        [HttpPut("bank-role"), Authorize(Roles = "BankAdmin")]
        public async Task<ActionResult> UpdateBankRoles(BankRoleUpdateDto updateDto)
        {
            var userId = HttpContext.User.GetUserId();

            if (!await _uow.BankRepository.IsBankAdmin(updateDto.BankId, userId))
                return BadRequest("You are not admin for this bank!");

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

            return await _uow.RoleRepository.UpdateBankRoles(updateDto, userId) ?
                Ok(await _uow.BankRepository.GetBankForModeration(updateDto.BankId, userId)) :
                BadRequest("Failed to update roles");
        }

        [HttpPost("{bankId}/change-photo"), Authorize(Roles = "BankAdmin")]
        public async Task<ActionResult> UpdateBankPhoto([FromForm] PhotoUpdateDto updateDto, int bankId)
        {
            var userId = HttpContext.User.GetUserId();
            if (!await _uow.BankRepository.IsBankAdmin(bankId, userId))
                return BadRequest("You are not admin for this bank!");

            if (updateDto.Remove)
            {
                await _uow.BankRepository.DeleteBankPhoto(bankId);
                return await _uow.SaveChanges()
                    ? Ok(new { PhotoUrl = "" })
                    : BadRequest("Failed to update photo.");
            }

            var photoUrl = await _uow.BankRepository.UpdateBankPhoto(bankId, updateDto.File);
            return !string.IsNullOrEmpty(photoUrl) && await _uow.SaveChanges()
                ? Ok(new { PhotoUrl = photoUrl })
                : BadRequest("Failed to update photo.");
        }
    }
}
