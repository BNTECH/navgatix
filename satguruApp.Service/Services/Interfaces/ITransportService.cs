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
        Task<int> SaveDriverKYCAsync(DriverKYCViewModel kycInfo);
        Task<List<DriverKYCViewModel>> GetDriverKYCAsync(Guid driverId);
        Task<int> UpdateProfileStatusAsync(Guid driverId, string status);
        Task<TransporterDashboardSummaryViewModel> GetDashboardSummary(string userId);
        Task<TransporterAnalyticsViewModel> GetTransporterAnalytics(string userId);
        Task<List<TransporterFleetItemViewModel>> GetFleetOverview(string userId);
        Task<List<DriverViewModel>> GetDriversList(string userId);
        Task<List<VehicleViewModel>> GetVehiclesList(string userId);
    }
}
