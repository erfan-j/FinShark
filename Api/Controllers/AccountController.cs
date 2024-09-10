using Api.Dtos.Account;
using Api.Interfaces;
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
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register(RegisterDto input)
        {
            try
            {
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
                        var result = new UserRegisteredResultDto
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            Token = _tokenService.CreateToken(user),
                        };
                        return Ok(result);
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
