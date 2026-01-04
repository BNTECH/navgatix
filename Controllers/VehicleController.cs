using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;

namespace navgatix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IBookingService _bookingService;
        public VehicleController(IVehicleService vehicleService, IBookingService bookingService)
        {
            _vehicleService = vehicleService;
            _bookingService = bookingService;
        }
        [AllowAnonymous]
        [HttpPost("vehicleRegistration")]
        public async Task<IActionResult> VehicleRegistration([FromBody] VehicleViewModel model)
        {
            await _vehicleService.SaveVehicleAsync(model);
            return Ok(model);
        }
        [AllowAnonymous]
        [HttpGet("getVehicle/{vehicleId}")]
        public async Task<IActionResult> GetVehicle(Guid vehicleId)
        {
            return Ok(await _vehicleService.GetVehicleDetails(vehicleId));
        }
        [AllowAnonymous]
        [HttpGet("deletevehicle/{vehicleId}/{status}")]
        public async Task<IActionResult> Deletevehicle(Guid vehicleId, bool status)
        {
            return Ok(await _vehicleService.Delete(vehicleId, status));
        }
        [HttpPost("bookVehicle")]
        [AllowAnonymous]
        public async Task<IActionResult> BookOfVehicle(BookingViewModel model)
        {
            return Ok(await _vehicleService.BookingVehicle(model));
        }
        [HttpPost("cancelbookingVehicleride")]
        [AllowAnonymous]
        public async Task<IActionResult> CancelBookingVehicle(BookingViewModel model)
        {
            return Ok(await _vehicleService.CancelBookingVehicleRide(model));
        }
    }
}
