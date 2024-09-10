using Api.Dtos.Account;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register(RegisterDto input)
        {
            try {
                var user = new User
                {
                    UserName = input.UserName,
                    Email = input.EmailAddress,
                };
                var createdUser = await _userManager.CreateAsync(user, input.Password);

                if (createdUser.Succeeded)
                {
                    var userRoles = await _userManager.AddToRoleAsync(user, "user");
                    if (userRoles.Succeeded)
                    {
                        return Ok("User registered successfully");
                    }
                    else
                    {
                        return StatusCode(500, userRoles.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
