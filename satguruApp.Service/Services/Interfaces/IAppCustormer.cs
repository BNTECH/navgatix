using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IAppCustormer:IRepository<CustomerDetail>
    {
        public Task<CustomerDetailViewModel> GetById(int? id, string name = "");
        public Task<int> SaveChangeAsync(CustomerDetailViewModel courseView);
        public Task<int> Delete(int id);
        public Task<List<CustomerDetailViewModel>> GetAll(string name = "");
    }
}
