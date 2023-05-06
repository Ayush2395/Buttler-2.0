using Buttler.Test.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buttler.Test.Application.DTO
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Username reuired")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone number required")]
        public string PhoneNumber { get; set; }
        public Enums.UserRole Roles = Enums.UserRole.staff;
    }
}
