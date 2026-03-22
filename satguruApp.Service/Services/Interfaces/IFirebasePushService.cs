using satguruApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IFirebasePushService
    {
        Task<string> RegisterDeviceTokenAsync(PushDeviceTokenRegistrationViewModel model);
        Task<string> RemoveDeviceTokenAsync(PushDeviceTokenRemovalViewModel model);
        Task<string> SendTestAsync(TestPushNotificationRequest model);
        Task SendToUserAsync(string? userId, PushNotificationPayload payload);
        Task SendToUsersAsync(IEnumerable<string?> userIds, PushNotificationPayload payload);
    }
}
