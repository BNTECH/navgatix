using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System.Threading.Tasks;

namespace navgatix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisputeController : ControllerBase
    {
        private readonly IDisputeService _disputeService;

        public DisputeController(IDisputeService disputeService)
        {
            _disputeService = disputeService;
        }

        [HttpPost("reportComplaint")]
        [AllowAnonymous]
        public async Task<IActionResult> ReportComplaint([FromBody] ComplaintReportViewModel model)
        {
            return Ok(await _disputeService.ReportComplaintAsync(model));
        }

        [HttpPost("reportRideIssue")]
        [AllowAnonymous]
        public async Task<IActionResult> ReportRideIssue([FromBody] ComplaintReportViewModel model)
        {
            return Ok(await _disputeService.ReportRideIssueAsync(model));
        }

        [HttpGet("ride/{rideId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRideDisputes(long rideId)
        {
            return Ok(await _disputeService.GetByRideAsync(rideId));
        }
    }
}
