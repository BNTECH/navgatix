using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System.Threading.Tasks;

namespace navgatix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PushNotificationsController : ControllerBase
    {
        private readonly IFirebasePushService _firebasePushService;

        public PushNotificationsController(IFirebasePushService firebasePushService)
        {
            _firebasePushService = firebasePushService;
        }

        [HttpPost("device-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterDeviceToken([FromBody] PushDeviceTokenRegistrationViewModel model)
        {
            var message = await _firebasePushService.RegisterDeviceTokenAsync(model);
            return Ok(new { message });
        }

        [HttpDelete("device-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RemoveDeviceToken([FromBody] PushDeviceTokenRemovalViewModel model)
        {
            var message = await _firebasePushService.RemoveDeviceTokenAsync(model);
            return Ok(new { message });
        }

        [HttpPost("test")]
        [AllowAnonymous]
        public async Task<IActionResult> SendTestPush([FromBody] TestPushNotificationRequest model)
        {
            var message = await _firebasePushService.SendTestAsync(model);
            return Ok(new { message });
        }
    }
}
