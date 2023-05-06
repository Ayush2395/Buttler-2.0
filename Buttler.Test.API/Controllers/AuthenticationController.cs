using Buttler.Test.API.Services;
using Buttler.Test.Application.DTO;
using Buttler.Test.Domain.Enums;
using Buttler.Test.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Buttler.Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenGenerateService _tokenGenerateService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(ITokenGenerateService tokenGenerateService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _tokenGenerateService = tokenGenerateService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("Login")]
        public ActionResult LoginUser(LoginDto login)
        {
            var user = _userManager.FindByEmailAsync(login.Email).Result;
            if (user == null) { return BadRequest("User not found"); }
            var tokenGenerate = _tokenGenerateService.GenerateToken(login);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenGenerate);
            var roles = _userManager.GetRolesAsync(user).Result;
            var authenticateModel = new AuthenticateModel
            {
                AccessToken = token,
                Email = user.Email,
                Roles = roles,
                UserName = user.UserName,
                ExpiresIn = tokenGenerate.ValidTo
            };

            return token != null ? Ok(authenticateModel) : Unauthorized();
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterNewUser(RegisterUserDto newUser)
        {
            var IsUserExist = await _userManager.FindByEmailAsync(newUser.Email);
            var IsRoleExist = await _roleManager.RoleExistsAsync("staff");
            var role = new IdentityRole(Enums.UserRole.staff.ToString());

            if (IsUserExist != null && IsRoleExist)
            {
                return BadRequest($"{newUser.Email} email already exist.");
            }
            else if (IsUserExist == null && newUser.PhoneNumber.Length == 10 && newUser.PhoneNumber.Length > 0)
            {
                var user = new ApplicationUser
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber
                };
                IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
                if (!result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                    return Ok("User successfully registered.");
                }
                else
                {
                    return BadRequest("Unable create user and role.");
                }
            }
            return BadRequest("User not created.");
        }
    }
}
