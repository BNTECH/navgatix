using Microsoft.AspNetCore.Mvc;
using satguruApp.Service.Services.Interfaces;

namespace navgatix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService _role;
        public RoleController(IRoleService role)
        {
            _role = role;
        }
    }
}
