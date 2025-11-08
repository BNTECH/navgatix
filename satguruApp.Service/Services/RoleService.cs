using Microsoft.AspNet.Identity;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace satguruApp.Service.Services
{
    public class RoleService : Repository<Role>, IRoleService
    {
        private readonly IUserService _userService;
        public RoleService(SatguruDBContext context, IUserService userService) : base(context)
        {
            _userService = userService;
        }
        SatguruDBContext db => (SatguruDBContext)_context;
        public async Task<List<RoleViewModel>> GetRoleList(RoleFilterViewModel model)
        {
            var skip = model.Page <= 1 ? 0 : (model.Page - 1 * model.PageSize);
            var roles = await (from role in db.Roles
                               where role.Name.ToLower().Contains(model.Filter.ToLower())
                               select new RoleViewModel
                               {
                                   Id = role.Id.ToString(),
                                   Name = role.Name,
                                   NormalizedName = role.NormalizedName
                               }).ToListAsync();
            return roles.Skip(skip).Take(model.PageSize).ToList();
        }
        public async Task<int> AddRole(RoleViewModel model)
        {
            var roles = await (from role in db.Roles where role.Name.ToLower() == model.Name.ToLower() select new ApplicationRole { Name = role.Name, NormalizedName = role.NormalizedName}).FirstOrDefaultAsync();
            if (roles == null)
            {
                roles = new ApplicationRole() {
                    Name = model.Name, NormalizedName = model.NormalizedName
                   // , Description = model.Descriminator
                   }
                ;
                db.Roles.Add(roles);
                await db.SaveChangesAsync();
            }
            return 1;
        }
        public async Task<int> UpdateRole(RoleViewModel model)
        {
            var roles = await (from role in db.Roles where role.Name.ToLower() == model.Name.ToLower() && role.Id.ToString() != model.Id select role).FirstOrDefaultAsync();
            if (roles == null)
            {
                roles = await (from role in db.Roles where role.Name.ToLower() == model.Name.ToLower() && role.Id.ToString() == model.Id select role).FirstOrDefaultAsync();
                await db.SaveChangesAsync();
            }
            return 1;
        }
        public async Task<int> DeleteRole(string id)
        {
            var roles = await (from role in db.Roles where role.Id.ToString() != id select role).FirstOrDefaultAsync();
            if (roles != null)
            {
                db.Roles.Remove(roles);
                await db.SaveChangesAsync();
            }
            return 1;
        }
        public async Task<RoleViewModel> GetRole(string id)
        {
            var role = await db.Roles.Where(x => x.Id.ToString() == id).Select(x => new RoleViewModel { Id = x.Id.ToString(), Name = x.Name, NormalizedName = x.NormalizedName
               // , Descriminator = x.Description
            }).FirstOrDefaultAsync();
            return role;
        }


    }
}
