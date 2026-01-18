using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using satguruApp.Service.Services;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System.Threading.Tasks;

namespace navgatix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }
        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _stateService.GetAll());
        }
        [HttpPost("save")]
        [AllowAnonymous]
        public async Task<IActionResult> Save([FromBody] StatelistViewModel model)
        {
            await _stateService.SaveChangeAsync(model);

            return Ok(model);
        }

        [HttpGet("getbyid/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _stateService.GetById(id));
        }
        [HttpGet("Delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _stateService.Delete(id));
        }
    }
}
