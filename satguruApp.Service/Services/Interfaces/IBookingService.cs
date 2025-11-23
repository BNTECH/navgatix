using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IBookingService : IRepository<Booking>
    {
        Task<int> SaveBookingPrices(BookingRateViewModel model);
        Task<int> Delete(Guid Id, bool isDeleted);
        Task<BookingRateViewModel> GetBookingPriceDetails(Guid bkRateId);
        Task<BookingViewModel> ScheduleLuggagePickup(BookingViewModel model);
    }
}
