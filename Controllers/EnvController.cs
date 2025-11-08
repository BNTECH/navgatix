using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using satguruApp.Service.Services.Interfaces;

namespace YourProjectName.Controllers
{
    public class EnvController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public EnvController(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpGet("getenv")]
        public IActionResult Get()
        {
            return Ok(new { Environment = _env.EnvironmentName });
        }
    }
}
