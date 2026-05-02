using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System.Threading.Tasks;

namespace navgatix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportController : ControllerBase
    {
        private readonly ITransportService _transportService;

        public TransportController(ITransportService transportService)
        {
            _transportService = transportService;
        }

        [HttpGet("getDashboardSummary")]
        public async Task<IActionResult> GetDashboardSummary(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("UserId is required.");
            return Ok(await _transportService.GetDashboardSummary(userId));
        }

        [HttpGet("getFleetOverview")]
        public async Task<IActionResult> GetFleetOverview(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("UserId is required.");
            return Ok(await _transportService.GetFleetOverview(userId));
        }

        [HttpGet("getTransporterAnalytics")]
        public async Task<IActionResult> GetTransporterAnalytics(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("UserId is required.");
            return Ok(await _transportService.GetTransporterAnalytics(userId));
        }
        
        [HttpPost("getTransporterDetails")]
        public async Task<IActionResult> GetTransporterDetails([FromBody] UserSearchViewModel model)
        {
            return Ok(await _transportService.GetTransporterDetails(model.Id));
        }

        [HttpPost("saveTransporterDetails")]
        public async Task<IActionResult> SaveTransporterDetails([FromBody] TransporterViewModel model)
        {
            return Ok(await _transportService.SaveTransporterAsync(model));
        }

        [HttpGet("getDriversList")]
        public async Task<IActionResult> GetDriversList(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("UserId is required.");
            return Ok(await _transportService.GetDriversList(userId));
        }

        [HttpGet("getVehiclesList")]
        public async Task<IActionResult> GetVehiclesList(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("UserId is required.");
            return Ok(await _transportService.GetVehiclesList(userId));
        }

        [HttpGet("getTransporterEarnings")]
        public async Task<IActionResult> GetTransporterEarnings(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("UserId is required.");
            return Ok(await _transportService.GetTransporterEarningsAsync(userId));
        }
    }
}
