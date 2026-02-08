using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? NewPassword { get; set; }
        public string PhoneNumber { get; set; }
        //public int? AppUserId { get; set; }
        public DateTime? DOB { get; set; }

    }
    public class LoginViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }

    }
}
