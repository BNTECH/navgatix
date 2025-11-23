using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface ITransportService :  IRepository<Driver>
    {
        Task<int> SaveDriverAsync(DriverViewModel driverInfo);
        Task<int> SaveTransporterAsync(TransporterViewModel driverInfo);
        Task<DriverViewModel> GetDriverDetails(string userId);
        Task<TransporterViewModel> GetTransporterDetails(string userId);
    }
}
