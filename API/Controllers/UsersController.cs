using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UsersController(UserManager<AppUser> userManager, IMapper mapper, IUserRepository userRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _userManager.Users
                .Include(u => u.Address)
                .Include(u => u.Photo)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.UserName == username.ToLower());

            if (user == null)
                return BadRequest("Failed to get user.");

            return user;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.BloodGroup = HttpUtility.UrlDecode(userParams.BloodGroup);
            userParams.CurrentUserName = HttpContext.User.GetUserName();
            var users = await _userRepository.GetUsers(userParams);
            Response.AddPaginationHeader(users.PageNumber, users.PageSize, users.TotalPages, users.TotalCount);
            return Ok(users);
        }
    }
}
