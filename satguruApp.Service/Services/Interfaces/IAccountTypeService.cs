using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IAccountTypeService : IRepository<AccountType>
    {
        public Task<AccountTypeViewModel> GetById(int? id, string name = "");
        public Task<int> SaveChangeAsync(AccountTypeViewModel courseView);
        public Task<int> Delete(int id);
        public Task< List<AccountTypeViewModel>> GetAll(string name = "");
    }
}
