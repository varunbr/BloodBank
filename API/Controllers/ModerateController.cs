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

        [HttpPost("bank-role/{bankId}"), Authorize(Roles = "BankAdmin")]
        public async Task<ActionResult> AddBankRole(RoleDto roleDto, int bankId)
        {
            var userId = HttpContext.User.GetUserId();
            var roleUserId = await _uow.UserRepository.GetUserIdByUserName(roleDto.UserName);

            if (!await _uow.BankRepository.IsBankAdmin(bankId, userId))
                return BadRequest("You are not admin for this bank!");

            if (!Util.GetBankRoles().Contains(roleDto.Role))
                return BadRequest("Invalid Role.");

            if (await _uow.BankRepository.IsBankModerator(bankId, roleUserId))
                return BadRequest("User already in role.");

            await _uow.RoleRepository.AddBankRole(roleDto.Role, bankId, roleUserId);

            if (!await _uow.SaveChanges()) 
                return BadRequest("Failed to add role.");

            await _uow.RoleRepository.UpdateUserRole(roleUserId, roleDto.Role);
            return Ok(await _uow.BankRepository.GetBankForModeration(bankId, userId));
        }

        [HttpDelete("bank-role/{bankId}"), Authorize(Roles = "BankAdmin")]
        public async Task<ActionResult> RemoveBankRole(RoleDto roleDto, int bankId)
        {
            var userId = HttpContext.User.GetUserId();
            var userName = HttpContext.User.GetUserName();
            var roleUserId = await _uow.UserRepository.GetUserIdByUserName(roleDto.UserName);

            if (!await _uow.BankRepository.IsBankAdmin(bankId, userId))
                return BadRequest("You are not admin for this bank!");

            if (!Util.GetBankRoles().Contains(roleDto.Role))
                return BadRequest("Invalid Role.");

            if (roleDto.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase))
                return BadRequest("You cannot remove your role.");

            await _uow.RoleRepository.RemoveBankRole(roleDto.Role, bankId, roleUserId);

            if (!await _uow.SaveChanges())
                return BadRequest("Failed to add role.");

            await _uow.RoleRepository.UpdateUserRole(roleUserId, roleDto.Role, false);
            return Ok(await _uow.BankRepository.GetBankForModeration(bankId, userId));
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
