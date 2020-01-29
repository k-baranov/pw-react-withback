using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.DataAccess.Interfaces;
using PW.DataTransferObjects.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
                
        [HttpGet]
        public async Task<ActionResult<UserDto>> AllExceptCurrent()
        {
            var email = HttpContext.User.Identity.Name;
            var user = await _userRepository.FindByAsync(x => x.Email != email);
            var result = _mapper.Map<UserDto>(user);
            return Ok(result);
        }
    }
}
