using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface ICityService : IRepository<City>
    {
        public Task<CityViewModel> GetById(int? id, string name = "");
        public Task<CityViewModel> SaveChangeAsync(CityViewModel cityView);
        public Task<int> Delete(int id);
        public Task<List<CityViewModel>> GetAll(string name = "", int stateId = 0);
    }
}
