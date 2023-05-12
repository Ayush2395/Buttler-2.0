using Buttler.Test.API.Services;
using Buttler.Test.Application.DTO;
using Buttler.Test.Domain.Data;
using Buttler.Test.Domain.Enums;
using Buttler.Test.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Web;

namespace Buttler.Test.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenGenerateService _tokenGenerateService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ButtlerContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly CallbackURLModel _callbackURLModel;
        private readonly EmailService _emailService;

        public AuthenticationController(ITokenGenerateService tokenGenerateService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ButtlerContext context, SignInManager<ApplicationUser> signInManager, IOptions<CallbackURLModel> options, EmailService emailService)
        {
            _tokenGenerateService = tokenGenerateService;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _callbackURLModel = options.Value;
            _emailService = emailService;
        }

        #region Login user
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null) { return BadRequest("User not found"); }
            var tokenGenerate = _tokenGenerateService.GenerateToken(login);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenGenerate);
            var roles = await _userManager.GetRolesAsync(user);
            if (_userManager.CheckPasswordAsync(user, login.Password).Result)
            {
                if (!user.EmailConfirmed && !user.TwoFactorEnabled)
                {
                    var authenticateModel = new AuthenticateModel
                    {
                        AccessToken = token,
                        Email = user.Email,
                        Roles = roles,
                        UserName = user.UserName,
                        IsEmailVerified = user.EmailConfirmed,
                        IsTwoFactorAuthentication = user.TwoFactorEnabled,
                        ExpiresIn = tokenGenerate.ValidTo
                    };
                    return token != null ? Ok(authenticateModel) : Unauthorized();
                }
                var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                return result.Succeeded ? Ok($"verification?email={user.Email}") : Unauthorized();
            }
            return BadRequest("Password is wrong.");

        }
        #endregion

        #region Register new user

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterNewUser(RegisterUserDto newUser)
        {
            var IsUserExist = await _userManager.FindByEmailAsync(newUser.Email);
            var IsRoleExist = await _roleManager.RoleExistsAsync("staff");
            var role = new IdentityRole(Enums.UserRole.staff.ToString());
            var claimRole = await _roleManager.GetClaimsAsync(role);

            if (IsUserExist != null && IsRoleExist)
            {
                return BadRequest($"{newUser.Email} email already exist.");
            }
            else if (IsUserExist == null && newUser.PhoneNumber.Length == 10 && newUser.PhoneNumber.Length > 0)
            {
                var user = new ApplicationUser
                {
                    UserName = newUser.FirstName,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber
                };
                IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                    await _userManager.AddClaimsAsync(user, claimRole);

                    _context.UserDetails.Add(new()
                    {
                        Uid = user.Id,
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        Age = newUser.Age,
                        Gender = newUser.Gender,
                    });

                    await _context.SaveChangesAsync();

                    return Ok("User successfully registered.");
                }
                else
                {
                    return BadRequest("Unable create user and role.");
                }
            }
            return BadRequest("User not created.");
        }
        #endregion

        #region Send email verification mail
        [HttpPost("SendEmailVerificationMail")]
        public async Task<IActionResult> SendEmailVericationMail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return NotFound("User not found."); }
            if (user != null && !user.EmailConfirmed)
            {
                var verificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodeCode = HttpUtility.UrlEncode(verificationCode);
                var callbackUrl = $"{_callbackURLModel.CallBackURL}/verification?email={user.Email}&code={encodeCode}";
                var isEmailSent = _emailService.SendEmailAsync(user.Email, callbackUrl);
                if (isEmailSent)
                {
                    return Ok("Email sent successfully, Please check your email.");
                }
            }
            return BadRequest();
        }
        #endregion

        #region Confirm email
        [HttpPost("EmailConfirmation")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return NotFound(new ResultDto<bool>(false, "User not found.")); }
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    return Ok(new ResultDto<bool>(true, "Your email is verified."));
                }
                return BadRequest(new ResultDto<bool>(false, "Invalid Token or Token is expired."));
            }
            return BadRequest(new ResultDto<bool>(false, "Something went wrong."));
        }
        #endregion

        #region Enable Two Factor Authentication
        [HttpPost("Enable2FA")]
        public async Task<IActionResult> Enable2FA(string email, bool isEnable)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return NotFound(new ResultDto<bool>(false, "User not found.")); }
            if (user != null)
            {
                if (isEnable)
                {
                    var enable = await _userManager.SetTwoFactorEnabledAsync(user, isEnable);
                    await _userManager.UpdateAsync(user);
                    if (enable.Succeeded)
                    {
                        return Ok(new ResultDto<bool>(true, "Two factor authentication is enabled."));
                    }
                    return BadRequest(new ResultDto<bool>(false, "Something went wrong."));
                }
                else if (!isEnable)
                {
                    var disable = await _userManager.SetTwoFactorEnabledAsync(user, isEnable);
                    await _userManager.UpdateAsync(user);
                    if (disable.Succeeded)
                    {
                        return Ok(new ResultDto<bool>(true, "Two factor authentication is disabled."));
                    }
                    return BadRequest(new ResultDto<bool>(false, "Something went wrong."));
                }
            }
            return BadRequest(new ResultDto<bool>(false, "Something went wrong."));
        }
        #endregion
    }
}
