using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IUserInfoService : IRepository<ApplicationUser>
    {
        public Task<int> SaveAsync(UserInfoViewModel userInfo);
        Task<UserInfoViewModel> GetUserDetailbyId(string userId);
        Task<List<UserInfoViewModel>> GetUserList(UserSearchViewModel userSearch);
        Task<List<UserInfoViewModel>> GetUserDetailList(UserSearchViewModel userSearch);
        Task<UserInfoViewModel> GetUserDetail(string userId);
    }
}
