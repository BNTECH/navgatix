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
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        [HttpGet("/getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string name = "")
        {
            return Ok(await _countryService.GetAll(name));
        }
        [HttpPost("save")]
        [AllowAnonymous]
        public async Task<IActionResult> Save([FromBody] CountryViewModel model)
        {
            await _countryService.SaveChangeAsync(model);

            return Ok(model);
        }

        [HttpGet("getbyid/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _countryService.GetById(id));
        }
        [HttpGet("Delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _countryService.Delete(id));
        }

    }
}
