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


        public async Task<int> SaveVehicleAsync(VehicleViewModel vehicleView)
        {
            Vehicle vehicleVM = new Vehicle();
            if (vehicleView.Id == null || vehicleView.Id == Guid.Empty)
            {

                vehicleVM.Id = Guid.NewGuid();
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
                vehicleVM = await _db.Vehicles.Where(x => x.Id == vehicleView.Id).FirstOrDefaultAsync();
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
            return await _db.SaveChangesAsync();
        }
        public async Task<VehicleViewModel> GetVehicleDetails(Guid vehicleId)
        {
            var vehicleVM = await (from vehicle in _db.Vehicles
                                   join trans in _db.TransporterDetails on vehicle.TransporterId equals trans.Id
                                   join cmn in _db.CommonTypes on vehicle.CT_VehicleType  equals cmn.Id into vType
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
            var vehicle = await _db.Vehicles.Where(x => x.Id == model.VehicleId && x.IsAvailable == true).FirstOrDefaultAsync();
            if (vehicle != null)
            {
                vehicle.IsAvailable = false;
                _db.Vehicles.Update(vehicle);
                var bookingExists = await (from x in _db.Bookings
                                           join comn in _db.CommonTypes on x.CT_BookingStatus  equals comn.Id
                                           where x.VehicleId == model.VehicleId && x.DriverId == model.DriverId && x.CustomerId == model.CustomerId && comn.Name != "Completed" && comn.Name != "Cancelled" select x).FirstOrDefaultAsync();
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
                        IsDeleted = false
                    };
                    _db.Bookings.Add(bookingExists);
                }
                await _db.SaveChangesAsync();
                return model;
            }
            else
            {
                throw new Exception("Vehicle is not available for booking.");
            }
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
                return model;
            }
            else
            {
                throw new Exception("Booking not found or already cancelled.");
            }
        }
    }
}
