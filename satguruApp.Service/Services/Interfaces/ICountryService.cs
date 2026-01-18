using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface ICountryService : IRepository<Country>
    {
        public Task<CountryViewModel> GetById(int? id, string name = "");
        public Task<CountryViewModel> SaveChangeAsync(CountryViewModel courseView);
        public Task<int> Delete(int id);
        public Task<List<CountryViewModel>> GetAll(string name = "");
    }
}
