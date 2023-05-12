using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buttler.Test.Application.DTO
{
    public class AuthenticateModel
    {
        public string AccessToken { get; set; }
        public IList<string> Roles { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsTwoFactorAuthentication { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
