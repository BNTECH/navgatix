using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IStateService : IRepository<State>
    {
        public Task<StatelistViewModel> GetById(int? id, string name = "");
        public Task<StatelistViewModel> SaveChangeAsync(StatelistViewModel courseView);
        public Task<int> Delete(int id);
        public Task<List<StatelistViewModel>> GetAll(string name = "", int countryId = 0);

    }
}
