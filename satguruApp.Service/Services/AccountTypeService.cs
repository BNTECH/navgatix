using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class AccountTypeService : Repository<AccountType>, IAccountTypeService
    {
        public AccountTypeService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<AccountTypeViewModel> GetById(int? id, string name = "")
        {
            return await (from accountType in _db.AccountTypes
                    where accountType.Id == id || accountType.Name == name
                    select new AccountTypeViewModel
                    {
                        IsDeleted = accountType.IsDeleted,
                        Id = accountType.Id,
                        Name = accountType.Name,
                    }).FirstOrDefaultAsync();
        }
        public async Task<int> SaveChangeAsync([FromBody] AccountTypeViewModel courseView)
        {
            var accountType = await _db.AccountTypes.Where(x => x.Id == courseView.Id || x.Name.ToLower() == courseView.Name.ToLower()).FirstOrDefaultAsync();
            if (accountType != null)
            {
                accountType.Name = courseView.Name.ToUpper();
                accountType.UpdatedDate = DateTime.Now;
            }
            else
            {
                accountType = new AccountType();
                accountType.Name = courseView.Name.ToUpper();
                accountType.CreatedDate = DateTime.Now;
                accountType.IsDeleted = false;
                _db.AccountTypes.Add(accountType);
            }
            return await _db.SaveChangesAsync();
        }
        public async Task<int> Delete(int id)
        {
            var accountType = await _db.AccountTypes.Where(x => x.Id == id).FirstOrDefaultAsync();
            accountType.IsDeleted = !accountType.IsDeleted;
            return  await _db.SaveChangesAsync();
        }
        public async Task<List<AccountTypeViewModel>> GetAll(string name = "")
        {
            return await(from accountType in _db.AccountTypes
                    where accountType.IsDeleted == false
                    select new AccountTypeViewModel
                    {
                        IsDeleted = accountType.IsDeleted,
                        Id = accountType.Id,
                        Name = accountType.Name,
                    }).ToListAsync();
        }

       
    }
}
