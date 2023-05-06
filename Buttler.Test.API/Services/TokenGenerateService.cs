using Buttler.Test.Application.DTO;
using Buttler.Test.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Buttler.Test.API.Services
{
    public class TokenGenerateService : ITokenGenerateService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public TokenGenerateService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public JwtSecurityToken GenerateToken(LoginDto login)
        {
            var user = _userManager.FindByEmailAsync(login.Email).Result;
            if (user != null)
            {
                var key = SignKey();
                var claims = GetClaims(login);
                return new JwtSecurityToken(
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(2),
                    signingCredentials: key
                    );
            }
            return null!;
        }


        private SigningCredentials SignKey()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims(LoginDto login)
        {
            var user = _userManager.FindByEmailAsync(login.Email).Result;
            var claims = new List<Claim>();
            var userRole = _userManager.GetRolesAsync(user).Result;
            foreach (var role in userRole)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, _configuration["JWT:Subject"]));
                claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("permission", "create"));
                claims.Add(new Claim("permission", "read"));
                claims.Add(new Claim("permission", "delete"));
                claims.Add(new Claim("uid", user.Id));
                claims.Add(new Claim("Email", user.Email));
            }

            return claims;
        }
    }

    public interface ITokenGenerateService
    {
        JwtSecurityToken GenerateToken(LoginDto login);
    }
}
