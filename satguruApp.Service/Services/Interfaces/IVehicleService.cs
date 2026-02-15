using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IVehicleService : IRepository<Vehicle>
    {
        public Task<VehicleViewModel> SaveVehicleAsync(VehicleViewModel driverInfo);
        public Task<VehicleViewModel> GetVehicleDetails(Guid vehicleId);
        public Task<int> Delete(Guid Id, bool isDeleted);
        public Task<BookingViewModel> BookingVehicle(BookingViewModel model);
        public Task<BookingViewModel> CancelBookingVehicleRide(BookingViewModel model);
        Task<List<BookingViewModel>> BookingVehicleRides(string userId);
        Task<LiveVehicleTrackingViewModel> SaveLiveVehicleTrackings(LiveVehicleTrackingViewModel liveVehicle);
        Task<List<LiveVehicleTrackingViewModel>> GetLiveVehicleTrackings(Guid vehicleId, string deviceId);
        Task<List<VehicleViewModel>> GetVehicleList(VehicleViewModel vehicleView);
    }
}
