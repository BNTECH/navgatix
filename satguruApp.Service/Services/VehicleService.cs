using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class VehicleService : Repository<Vehicle>, IVehicleService
    {
        public VehicleService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;


        public async Task<VehicleViewModel> SaveVehicleAsync(VehicleViewModel vehicleView)
        {
            var saveCnt = 0;
            try
            {
                var vehicleVM = await (from vehicle in _db.Vehicles where (vehicle.Id == vehicleView.Id || (vehicle.VehicleNumber.ToLower() == vehicleView.VehicleNumber || vehicle.RCNumber.ToLower() == vehicleView.RCNumber.ToLower())) select vehicle).FirstOrDefaultAsync();
                if (vehicleView.Id == Guid.Empty && vehicleVM == null)
                {
                    vehicleVM = new Vehicle();
                    vehicleVM.Id = vehicleView.Id = Guid.NewGuid();
                    vehicleVM.TransporterId = vehicleView.TransporterId;
                    vehicleVM.CurrentLatitude = vehicleView.CurrentLatitude;
                    vehicleVM.RCNumber = vehicleView.RCNumber;
                    vehicleVM.CurrentLongitude = vehicleView.CurrentLongitude;
                    vehicleVM.SizeCubicMeters = vehicleView.SizeCubicMeters;
                    vehicleVM.CapacityTons = vehicleView.CapacityTons;
                    vehicleVM.InsuranceExpiry = vehicleView.InsuranceExpiry;
                    vehicleVM.RoadTaxExpiry = vehicleView.RoadTaxExpiry;
                    vehicleVM.IsAvailable = vehicleView.IsAvailable;
                    vehicleVM.UploadPhoneUrl = vehicleView.UploadPhoneUrl;
                    vehicleVM.VehicleNumber = vehicleView.VehicleNumber;
                    vehicleVM.CT_VehicleType = vehicleView.CT_VehicleType;
                    vehicleVM.CTBodyType = vehicleView.CTBodyType;
                    vehicleVM.IsDeleted = false;
                    _db.Vehicles.Add(vehicleVM);
                }
                else
                {
                    vehicleView.Id = vehicleVM.Id;
                    vehicleVM.TransporterId = vehicleView.TransporterId;
                    vehicleVM.CurrentLatitude = vehicleView.CurrentLatitude;
                    vehicleVM.RCNumber = vehicleView.RCNumber;
                    vehicleVM.CurrentLongitude = vehicleView.CurrentLongitude;
                    vehicleVM.SizeCubicMeters = vehicleView.SizeCubicMeters;
                    vehicleVM.CapacityTons = vehicleView.CapacityTons;
                    vehicleVM.InsuranceExpiry = vehicleView.InsuranceExpiry;
                    vehicleVM.RoadTaxExpiry = vehicleView.RoadTaxExpiry;
                    vehicleVM.IsAvailable = vehicleView.IsAvailable;
                    vehicleVM.UploadPhoneUrl = vehicleView.UploadPhoneUrl;
                    vehicleVM.VehicleNumber = vehicleView.VehicleNumber;
                    vehicleVM.CT_VehicleType = vehicleView.CT_VehicleType;
                    vehicleVM.CTBodyType = vehicleView.CTBodyType;
                    vehicleVM.IsDeleted = false;
                }
                saveCnt = await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            if (saveCnt > 0)
                vehicleView.Message = "Success";
            else
                vehicleView.Message = "Failed";
            return vehicleView;
        }
        public async Task<VehicleViewModel> GetVehicleDetails(Guid vehicleId)
        {
            var vehicleVM = await (from vehicle in _db.Vehicles
                                   join trans in _db.TransporterDetails on vehicle.TransporterId equals trans.Id
                                   join cmn in _db.CommonTypes on vehicle.CT_VehicleType equals cmn.Id into vType
                                   from cmn in vType.DefaultIfEmpty()
                                   join cmnbdy in _db.CommonTypes on vehicle.CTBodyType equals cmnbdy.Id into VbodyType
                                   from cmnbdy in VbodyType.DefaultIfEmpty()
                                   where vehicle.Id == vehicleId
                                   select new VehicleViewModel
                                   {
                                       Id = vehicle.Id,
                                       CapacityTons = vehicle.CapacityTons,
                                       CurrentLatitude = vehicle.CurrentLatitude,
                                       CurrentLongitude = vehicle.CurrentLongitude,
                                       InsuranceExpiry = vehicle.InsuranceExpiry,
                                       IsAvailable = vehicle.IsAvailable,
                                       IsDeleted = vehicle.IsDeleted,
                                       TransporterName = trans.CompanyName,
                                       PermitExpiry = vehicle.PermitExpiry,
                                       RCNumber = vehicle.RCNumber,
                                       RoadTaxExpiry = vehicle.RoadTaxExpiry,
                                       SizeCubicMeters = vehicle.SizeCubicMeters,
                                       TransporterId = vehicle.TransporterId.GetValueOrDefault(),
                                       UploadPhoneUrl = vehicle.UploadPhoneUrl,
                                       VehicleNumber = vehicle.VehicleNumber,
                                       CT_VehicleType = vehicle.CT_VehicleType,
                                       VehicleTypeName = cmn.Name,
                                       CTBodyType = vehicle.CTBodyType,
                                       BodyTypeName = cmnbdy.Name,
                                   }).FirstOrDefaultAsync();
            return vehicleVM;
        }
        public async Task<int> Delete(Guid Id, bool isDeleted)
        {
            var vehicleVM = await (from vehicle in _db.Vehicles where vehicle.Id == Id select vehicle).FirstOrDefaultAsync();
            if (vehicleVM != null)
            {
                vehicleVM.IsDeleted = isDeleted;
            }
            return await _db.SaveChangesAsync();
        }

        public async Task<BookingViewModel> BookingVehicle(BookingViewModel model)
        {
            var vehicle = await _db.Vehicles.Where(x => x.Id == model.VehicleId && (x.IsAvailable == true || x.IsAvailable == null)).FirstOrDefaultAsync();
            if (vehicle != null)
            {
                vehicle.IsAvailable = false;
                _db.Vehicles.Update(vehicle);
                var bookingExists = await (from x in _db.Bookings
                                           join comn in _db.CommonTypes on x.CT_BookingStatus equals comn.Id
                                           where x.VehicleId == model.VehicleId && x.DriverId == model.DriverId && x.CustomerId == model.CustomerId && comn.Name != "Completed" && comn.Name != "Cancelled"
                                           select x).FirstOrDefaultAsync();
                if (bookingExists == null)
                {
                    bookingExists = new Booking
                    {
                        VehicleId = model.VehicleId,
                        CustomerId = model.CustomerId,
                        DriverId = model.DriverId,
                        PickupAddress = model.PickupAddress,
                        PickupLat = model.PickupLat,
                        PickupLng = model.PickupLng,
                        DropAddress = model.DropAddress,
                        DropLat = model.DropLat,
                        DropLng = model.DropLng,
                        GoodsType = model.GoodsType,
                        GoodsWeight = model.GoodsWeight,
                        EstimatedFare = model.EstimatedFare,
                        FinalFare = model.FinalFare,
                        CT_BookingStatus = model.CT_BookingStatus,
                        ScheduledTime = model.ScheduledTime,
                        CreatedAt = DateTime.UtcNow,
                        IsAvailable = true,
                        IsDeleted = false,
                        DeptStateId = model.DeptStateId,
                        DeptCityId = model.DeptCityId,
                        ArrivalStateId = model.ArrivalStateId,
                        ArrivalCityId = model.ArrivalCityId,
                        CustomerName = model.CustomerName,
                    };
                    _db.Bookings.Add(bookingExists);
                }
                await _db.SaveChangesAsync();
                model.Id = bookingExists.Id;
            }
            else
            {
                model.Message = "Vehicle is not available for booking.";
            }
            return model;
        }
        public async Task<BookingViewModel> CancelBookingVehicleRide(BookingViewModel model)
        {
            var booking = await _db.Bookings.Where(x => x.Id == model.Id && x.VehicleId == model.VehicleId && x.DriverId == model.DriverId && x.CustomerId == model.CustomerId).FirstOrDefaultAsync();
            if (booking != null)
            {
                booking.CT_BookingStatus = model.CT_BookingStatus;
                booking.IsAvailable = true;
                _db.Bookings.Update(booking);
                var vehicle = await _db.Vehicles.Where(x => x.Id == booking.VehicleId).FirstOrDefaultAsync();
                if (vehicle != null)
                {
                    vehicle.IsAvailable = true;
                    _db.Vehicles.Update(vehicle);
                }
                await _db.SaveChangesAsync();
                model.Message = "Your Booking has been cancelled.";
            }
            else
            {
                model.Message = "Booking not found or already cancelled.";
            }
            return model;
        }
        public async Task<List<BookingViewModel>> BookingVehicleRides(string userId)
        {
            var bookings = await (from book in _db.Bookings
                                  join comonType in _db.CommonTypes on book.CT_BookingStatus equals comonType.Id
                                  where book.CustomerId == userId && comonType.Name != "Cancelled"
                                  select new BookingViewModel
                                  {
                                      Id = book.Id,
                                      CustomerId = book.CustomerId,
                                      DriverId = book.DriverId,
                                      PickupAddress = book.PickupAddress,
                                      PickupLat = book.PickupLat,
                                      PickupLng = book.PickupLng,
                                      DropAddress = book.DropAddress,
                                      DropLat = book.DropLat,
                                      DropLng = book.DropLng,
                                      GoodsType = book.GoodsType,
                                      GoodsWeight = book.GoodsWeight,
                                      EstimatedFare = book.EstimatedFare,
                                      FinalFare = book.FinalFare,
                                      CT_BookingStatus = book.CT_BookingStatus,
                                      ScheduledTime = book.ScheduledTime,
                                      CreatedAt = book.CreatedAt,
                                      IsAvailable = book.IsAvailable,
                                      IsDeleted = book.IsDeleted,
                                  }).ToListAsync();
            if (bookings.Any())
            {
                return bookings;
            }
            else
            {
                return new List<BookingViewModel>() { new BookingViewModel { Message = "You haven't any booking or cancelled your booking" } };
            }
        }
        public async Task<LiveVehicleTrackingViewModel> SaveLiveVehicleTrackings(LiveVehicleTrackingViewModel liveVehicle)
        {
            var vehicle = await _db.Vehicles.Where(x => x.Id == liveVehicle.VehicleId && !x.IsDeleted.GetValueOrDefault()).FirstOrDefaultAsync();
            if (vehicle == null)
            {
                var liveVehicleTrack = new LiveVehicleTracking
                {
                    VehicleId = vehicle.Id,
                    DeviceId = liveVehicle.DeviceId,
                    LastLatitude = vehicle.CurrentLatitude,
                    LastLongitude = vehicle.CurrentLongitude,
                    LastUpdated = DateTime.Now,
                    IsDeleted = false,
                };
                _db.LiveVehicleTrackings.Add(liveVehicleTrack);
                await _db.SaveChangesAsync();
                liveVehicle.Id = liveVehicleTrack.Id;
                return liveVehicle;
            }

            else
            {
                return new LiveVehicleTrackingViewModel { Message = "Vehicle not available now" };
            }
        }
        public async Task<List<LiveVehicleTrackingViewModel>> GetLiveVehicleTrackings(Guid vehicleId, string deviceId)
        {
            var liveVehicleTrack = await _db.LiveVehicleTrackings.Where(x => x.VehicleId == vehicleId && x.DeviceId == deviceId && !x.IsDeleted.GetValueOrDefault()).Select(x => new LiveVehicleTrackingViewModel
            {
                Id = x.Id,
                VehicleId = x.VehicleId,
                DeviceId = x.DeviceId,
                LastLatitude = x.LastLatitude,
                LastLongitude = x.LastLongitude,
                LastUpdated = x.LastUpdated,
            }).ToListAsync();
            if (liveVehicleTrack.Any())
            {
                return liveVehicleTrack;
            }
            else
            {
                return new List<LiveVehicleTrackingViewModel> { new LiveVehicleTrackingViewModel { Message = "Live tracking data not found for the specified vehicle." } };
            }
        }
    }
}
