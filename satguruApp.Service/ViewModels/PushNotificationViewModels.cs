using System.Collections.Generic;

namespace satguruApp.Service.ViewModels
{
    public class PushDeviceTokenRegistrationViewModel
    {
        public string? UserId { get; set; }
        public string? DeviceToken { get; set; }
        public string? Platform { get; set; }
        public string? DeviceId { get; set; }
    }

    public class PushDeviceTokenRemovalViewModel
    {
        public string? UserId { get; set; }
        public string? DeviceToken { get; set; }
    }

    public class PushNotificationPayload
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public Dictionary<string, string> Data { get; set; } = new();
    }

    public class TestPushNotificationRequest
    {
        public string? UserId { get; set; }
        public string? DeviceToken { get; set; }
        public string Title { get; set; } = "Test Notification";
        public string Body { get; set; } = "Firebase push notification is working.";
    }
}
