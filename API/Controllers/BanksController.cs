using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class BanksController : BaseController
    {
        private readonly IBankRepository _bankRepository;

        public BanksController(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        [HttpGet]
        public async Task<ActionResult<BankDto>> GetBanks([FromQuery]BankParams bankParams)
        {
            var banks = await _bankRepository.GetUsers(bankParams);
            Response.AddPaginationHeader(banks.PageNumber,banks.PageSize,banks.TotalPages,banks.TotalCount);
            return Ok(banks);
        }
    }
}
