using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace navgatix.Hubs
{
    /// <summary>
    /// Real-time chat hub scoped to a booking/ride ID.
    /// Each booking gets its own SignalR group so messages are isolated.
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>Joins the caller to the chat room for a given booking.</summary>
        public async Task JoinBookingChat(long bookingId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{bookingId}");
        }

        /// <summary>Removes the caller from the chat room for a given booking.</summary>
        public async Task LeaveBookingChat(long bookingId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{bookingId}");
        }

        /// <summary>
        /// Broadcasts a chat message to everyone in the booking's room
        /// (both the driver and the customer).
        /// </summary>
        public async Task SendMessage(long bookingId, string senderName, string message)
        {
            await Clients.Group($"Chat_{bookingId}").SendAsync(
                "ReceiveMessage",
                senderName,
                message,
                System.DateTime.UtcNow.ToString("o")
            );
        }
    }
}
