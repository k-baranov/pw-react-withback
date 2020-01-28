using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.DataAccess.Interfaces;
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

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
                
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var email = HttpContext.User.Identity.Name;
            var result = await _userRepository.FindByAsync(x => x.Email != email);

            return Ok(result);
        }
    }
}
