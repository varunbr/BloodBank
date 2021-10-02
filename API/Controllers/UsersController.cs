using System.Collections.Generic;
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

    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUnitOfWork _uow;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _uow.UserRepository.GetUser(username);

            return user == null ? BadRequest("Failed to get user.") : user;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.BloodGroup = HttpUtility.UrlDecode(userParams.BloodGroup);
            userParams.CurrentUserName = HttpContext.User.GetUserName();
            var users = await _uow.UserRepository.GetUsers(userParams);
            Response.AddPaginationHeader(users.PageNumber, users.PageSize, users.TotalPages, users.TotalCount);
            return Ok(users);
        }
    }
}
