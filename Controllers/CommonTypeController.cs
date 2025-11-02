using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System.Threading.Tasks;

namespace navgatix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonTypeController : ControllerBase
    {
        private readonly ICommonTypeService _commonTypeService;

        public CommonTypeController(ICommonTypeService commonTypeService)
        {
            _commonTypeService = commonTypeService;
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _commonTypeService.GetAll());
        }
        [HttpPost("save")]
        [AllowAnonymous]
        public async Task<IActionResult> Save([FromBody] CommonTypeViewModel model)
        {
            await _commonTypeService.SaveUpdate(model);

            return Ok(model);
        }

        [HttpGet("getbyid/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _commonTypeService.GetById(id));
        }
        [HttpGet("Delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _commonTypeService.Delete(id));
        }

        [HttpGet("getcommontypeWithKeys/{code}/{flag}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommonTypeListWithAndId(string code, string flag = "")
        {
            return Ok(await _commonTypeService.GetCommonTypeListWithAndId(code, flag));
        }
        [HttpPost("filterdata")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFilterdata([FromBody] CommonTypeSearchViewModel model)
        {
            return Ok(await _commonTypeService.FilterCommonTypes(model));
        }
    }
}
