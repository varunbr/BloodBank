using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IUserRepository _userRepository;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IMapper mapper, ITokenService tokenService, IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _userRepository = userRepository;
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
            user.Photo = new Photo();
            user.LastActive = DateTime.UtcNow;

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
                Gender = user.Gender,
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
                PhotoUrl = user.Photo?.Url,
                Token = token,
                Gender = user.Gender,
                UserName = user.UserName
            };
        }

        [Authorize]
        [HttpPost("token-update")]
        public async Task<ActionResult<UserDto>> GetUpdatedToken()
        {
            var id = HttpContext.User.GetUserId();
            var user = await _userManager.Users
                .Include(u => u.Photo)
                .FirstOrDefaultAsync(u => u.Id == id);

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var token = await _tokenService.CreateToken(user, accessToken);

            return new UserDto
            {
                Name = user.Name,
                PhotoUrl = user.Photo?.Url,
                Token = token,
                Gender = user.Gender,
                UserName = user.UserName
            };
        }

        [HttpGet("{userName}")]
        public async Task<bool> UserNameExist(string userName)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == userName);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult> GetUserProfile()
        {
            var id = HttpContext.User.GetUserId();
            return Ok(await _userRepository.GetProfile(id));
        }

        [Authorize]
        [HttpPost("profile")]
        public async Task<ActionResult> UpdateUserProfile(UserProfileDto profileDto)
        {
            profileDto.Id = HttpContext.User.GetUserId();
            var profile = await _userRepository.UpdateProfile(profileDto);
            return profile == null ? BadRequest("Failed to update.") : Ok(profile);
        }

        [Authorize]
        [HttpPost("change-photo")]
        public async Task<ActionResult> UpdateUserPhoto([FromForm] PhotoUpdateDto updateDto)
        {
            var id = HttpContext.User.GetUserId();
            if (updateDto.Remove)
            {
                return await _userRepository.DeleteUserPhoto(id)
                    ? Ok(new { PhotoUrl = "" })
                    : BadRequest("Failed to change photo.");
            }
            var result = await _userRepository.UpdateUserPhoto(id, updateDto.File);
            return string.IsNullOrEmpty(result) ? BadRequest("Failed to change photo.") : Ok(new { PhotoUrl = result });
        }
    }
}