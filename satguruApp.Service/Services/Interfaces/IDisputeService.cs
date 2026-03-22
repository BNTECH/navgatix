using satguruApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace satguruApp.Service.Services.Interfaces
{
    public interface IDisputeService
    {
        Task<DisputeResultViewModel> ReportComplaintAsync(ComplaintReportViewModel model);
        Task<DisputeResultViewModel> ReportRideIssueAsync(ComplaintReportViewModel model);
        Task<List<DisputeItemViewModel>> GetByRideAsync(long rideId);
    }
}
