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

        public ModerateController(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
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

    }
}
