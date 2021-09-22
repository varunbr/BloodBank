using System.Threading.Tasks;
using System.Web;
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
        public async Task<ActionResult> GetBanks([FromQuery] BankParams bankParams)
        {
            bankParams.BloodGroup = HttpUtility.UrlDecode(bankParams.BloodGroup);
            var banks = await _bankRepository.GetBanks(bankParams);
            Response.AddPaginationHeader(banks.PageNumber, banks.PageSize, banks.TotalPages, banks.TotalCount);
            return Ok(banks);
        }
    }
}
