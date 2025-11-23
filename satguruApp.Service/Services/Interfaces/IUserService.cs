using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IUserService
    {
        public Task<string> UserRegisterAsync(UserViewModel model);
        public Task<AuthenticationViewModel> GetTokenAsync(TokenRequestViewModel model);
        public Task<string> UpdateUserRoleAsync(RoleViewModel model);
        public Task<AuthenticationViewModel> Login(LoginViewModel model);
        public Task<ApplicationUser> FindUserByUserName(string userName);
        public Task<ApplicationUser> FindUserByUserId(string userId);
        public Task<ApplicationUser> FindByEmailAsync(string email);
        Task<string> CreateRoles(string roleName);
        Task<ApplicationRole> GetRoleAsync(string roleName);
        Task<List<ApplicationRole>> GetRolesAsync(string roleName = "");
        Task<string> UpdateUserRolesAsync(RoleViewModel model);
        Task<string> UpdateUser(UserViewModel model);
        Task<string> ChangePassword(UserViewModel model);
    }
}
