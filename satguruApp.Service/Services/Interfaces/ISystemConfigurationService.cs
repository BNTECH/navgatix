using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace satguruApp.Service.Services.Interfaces
{
    public interface ISystemConfigurationService : IRepository<SystemConfiguration>
    {
        public Task<SystemConfigurationViewModel> GetById(int? id, string name = "");
        public Task<int> SaveChangeAsync(SystemConfigurationViewModel courseView);
        public Task<int> Delete(int id);
        public Task<List<SystemConfigurationViewModel>> GetAll(string name = "");
        public Task<SystemConfigurationViewModel> GetConfiguration(string name = "");
      public Task<T> GetConfigurationDetails<T>(string keyField="");
    }
}
