using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IRoleService : IRepository<Role>
    {
       Task< List<RoleViewModel>> GetRoleList(RoleFilterViewModel model);
        Task<int> DeleteRole(string id);
        Task<int> AddRole(RoleViewModel model);
        Task<int> UpdateRole(RoleViewModel model);
        Task<RoleViewModel> GetRole(string id);
    }
}
