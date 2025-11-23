using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using static satguruApp.Service.ViewModels.CompanyDetailViewModel;

namespace satguruApp.Service.Services.Interfaces
{
    public interface ICompanyService : IRepository<Company>
    {

       Task< object> GetCompanyList(CompanyFilterViewModel filter);
       Task< CompanyViewModel> AddCompany(CompanyViewModel model);
       Task< CompanyViewModel> UpdCompany(CompanyViewModel model);
       ////Task< CompanyEditViewModel> DoActiveAndInactiveCompany(CompanyEditViewModel VMModel);
       Task< CompanyEditViewModel> GetCompanyDetailsById(int companyId);
        Task<CompanyViewModel> GetCompany(int id);
        Task<CompanyViewModel> GetCompanyDetail(int id);



        #region interface same ERPKick Comapny screen
        //List<CompanyListViewModel> FilterCompanyList_ERP(CompanyFilterViewModel filterVM);
        //CompanyEditViewModel AddCompany_ERP(CompanyEditViewModel model);
        //CompanyEditViewModel UpdateCompany_ERP(CompanyEditViewModel model);
        //Task<CompanyEditViewModel> GetCompany_ERP(int id);
        #endregion
    }
}
