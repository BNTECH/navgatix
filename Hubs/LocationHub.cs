using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace navgatix.Hubs
{
    public class LocationHub : Hub
    {
        public async Task JoinRide(long bookingId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Ride_{bookingId}");
        }

        public async Task LeaveRide(long bookingId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Ride_{bookingId}");
        }
    }
}
