using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.DataAccess.Interfaces;
using PW.DataTransferObjects;
using PW.Entities;
using PW.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PW.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private const string InvalidUserDataMessage = "Invalid user data";
        private const string CurrentUserNotFoundMessage = "Current user not found";

        private readonly IMembershipService _membershipService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AccountController(IMembershipService membershipService,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _membershipService = membershipService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            PwUser user = null;
            ClaimsPrincipal claimsPrincipal = null;
            try
            {
                user = await _membershipService.GetUserAsync(loginDto.Email, loginDto.Password);
                claimsPrincipal = _membershipService.GetUserClaimsPrincipal(user);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Ok(_mapper.Map<UserBalanceDto>(user));
        }
                
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
                
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] SignUpDto signUpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(InvalidUserDataMessage);
            }

            try
            {
                await _membershipService.CreateUserAsync(signUpDto.UserName, signUpDto.Email, signUpDto.Password);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Balance()
        {
            var email = HttpContext.User.Identity.Name;
            var user = await _userRepository.GetSingleByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(CurrentUserNotFoundMessage);
            }

            var result = _mapper.Map<UserBalanceDto>(user);
            return Ok(result);
        }
    }
}
