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
    public class BookingService : Repository<Booking>, IBookingService
    {
        public BookingService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<int> SaveBookingPrices(BookingRateViewModel model)
        {
            var booking = await (from bkrates in _db.BookingRates where bkrates.IsDeleted == false && bkrates.VehicleType == model.VehicleType && bkrates.MinDistanceKm == model.MinDistanceKm && bkrates.MaxDistanceKm == model.MaxDistanceKm && bkrates.EffectiveFrom == model.EffectiveFrom select bkrates).FirstOrDefaultAsync();
            if (booking != null)
            {
                booking.BaseRatePerKm = model.BaseRatePerKm; booking.VehicleType = model.VehicleType;
                booking.MinDistanceKm = model.MinDistanceKm;
                booking.MaxDistanceKm = model.MaxDistanceKm;
                booking.EffectiveFrom = model.EffectiveFrom;
                booking.ExtraWeightChargePerTon = model.ExtraWeightChargePerTon;
            }
            else
            {
                booking = new BookingRate { BaseRatePerKm = model.BaseRatePerKm, EffectiveFrom = model.EffectiveFrom, ExtraWeightChargePerTon = model.ExtraWeightChargePerTon, IsActive = model.IsActive, MaxDistanceKm = model.MaxDistanceKm, MinDistanceKm = model.MinDistanceKm, VehicleType = model.VehicleType, IsDeleted = false };
            }
            return await _db.SaveChangesAsync();
        }
        public async Task<int> Delete(Guid Id, bool isDeleted)
        {
            var bkrates = await (from bookingrate in _db.BookingRates where bookingrate.Id == Id select bookingrate).FirstOrDefaultAsync();
            if (bkrates != null)
            {
                bkrates.IsDeleted = isDeleted;
            }
            return await _db.SaveChangesAsync();
        }
        public async Task<BookingRateViewModel> GetBookingPriceDetails(Guid bkRateId)
        {
            var bkRateVM = await (from bkrate in _db.BookingRates
                                  where bkrate.Id == bkRateId
                                  select new BookingRateViewModel
                                  {
                                      Id = bkrate.Id,
                                      IsDeleted = bkrate.IsDeleted,
                                      VehicleType = bkrate.VehicleType,
                                      BaseRatePerKm = bkrate.BaseRatePerKm,
                                      EffectiveFrom = bkrate.EffectiveFrom,
                                      ExtraWeightChargePerTon = bkrate.ExtraWeightChargePerTon,
                                      IsActive = bkrate.IsActive,
                                      MaxDistanceKm = bkrate.MaxDistanceKm,
                                      MinDistanceKm = bkrate.MinDistanceKm

                                  }).FirstOrDefaultAsync();
            return bkRateVM;
        }
        public async Task<BookingViewModel> ScheduleLuggagePickup(BookingViewModel model)
        {

            return new BookingViewModel();
        }

    }
}
