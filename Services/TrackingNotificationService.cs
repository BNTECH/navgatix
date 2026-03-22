using Microsoft.AspNetCore.SignalR;
using navgatix.Hubs;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System.Threading.Tasks;

namespace navgatix.Services
{
    public class TrackingNotificationService : ITrackingNotificationService
    {
        private readonly IHubContext<LocationHub> _hubContext;

        public TrackingNotificationService(IHubContext<LocationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyDriverLocationUpdatedAsync(RideTrackingSnapshotViewModel snapshot)
        {
            await _hubContext.Clients.Group($"Ride_{snapshot.BookingId}")
                .SendAsync("driverLocationUpdated", snapshot);
        }

        public async Task NotifyRideAssignedAsync(RideTrackingSnapshotViewModel snapshot)
        {
            await _hubContext.Clients.Group($"Ride_{snapshot.BookingId}")
                .SendAsync("rideAssigned", snapshot);
        }

        public async Task NotifyRideStatusChangedAsync(RideTrackingSnapshotViewModel snapshot)
        {
            await _hubContext.Clients.Group($"Ride_{snapshot.BookingId}")
                .SendAsync("rideStatusChanged", snapshot);
        }
    }
}
