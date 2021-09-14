using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDto registerDto)
        {
            if (await UserNameExist(registerDto.UserName))
                return BadRequest("Username is taken.");
            if (!Util.GetGenderList().Contains(registerDto.Gender))
                return BadRequest("Invalid gender.");
            if (!Util.GetBloodGroupList().Contains(registerDto.BloodGroup))
                return BadRequest("Invalid blood group.");

            var user = _mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.UserName.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            result = await _userManager.AddToRoleAsync(user, "Member");
            if (!result.Succeeded) return BadRequest(result.Errors);

            var token = await _tokenService.CreateToken(user);

            return new UserDto
            {
                Name = user.Name,
                PhotoUrl = user.Photo?.Url,
                Token = token,
                UserName = user.UserName
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(u => u.Photo)
                .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName.ToLower());

            if (user == null) return Unauthorized("Invalid user");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();

            var token = await _tokenService.CreateToken(user);

            return new UserDto
            {
                Name = user.Name,
                PhotoUrl = user.Photo.Url,
                Token = token,
                UserName = user.UserName
            };
        }

        private Task<bool> UserNameExist(string userName)
        {
            return _userManager.Users.AnyAsync(u => u.UserName == userName.ToLower());
        }
    }
}