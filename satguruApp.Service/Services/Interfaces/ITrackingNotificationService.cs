using System.Threading.Tasks;
using satguruApp.Service.ViewModels;

namespace satguruApp.Service.Services.Interfaces
{
    public interface ITrackingNotificationService
    {
        Task NotifyDriverLocationUpdatedAsync(RideTrackingSnapshotViewModel snapshot);
        Task NotifyRideAssignedAsync(RideTrackingSnapshotViewModel snapshot);
        Task NotifyRideStatusChangedAsync(RideTrackingSnapshotViewModel snapshot);
    }
}
