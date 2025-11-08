using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface ICommonTypeService : IRepository<CommonType>
    {
        public Task<List<CommonTypeViewModel>> GetAll();
        public Task<List<CommonTypeViewModel>> FilterCommonTypes(CommonTypeSearchViewModel model);
        public Task<CommonTypeViewModel> GetById(int id);
        public Task<int> SaveUpdate(CommonTypeViewModel commonTypeView);
        public Task<int> Delete(int id);
        public Task<List<CommonTypeWithKeyViewModel>> GetCommonTypeListWithAndId(String code, string flag);
    }
}
