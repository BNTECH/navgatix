using Microsoft.AspNetCore.Mvc;
using satguruApp.DLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourProjectName.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NavigationController : ControllerBase
    {
        //private readonly INavigations _navigate;
        //public NavigationController(INavigations navData)
        //{
        //    _navigate = navData;
        //}
        //[HttpPost("GetallNavLink")]
        //public IActionResult GetallNavLink([FromBody] NavLinkViewModel vmModel)
        //{
        //    List<NavLinkViewModel> filter = new List<NavLinkViewModel>();
        //    filter = _navigate.GetallNavLinks(vmModel);
        //    var pagedSet = new PaginationSet<object>(filter, vmModel.PageSize, vmModel.Page, vmModel.TotalCount);
        //    return Ok(pagedSet);



        //}
        //[HttpPost("GetParentNavLink")]
        //public IActionResult GetaParentNavLink([FromBody] NavLinkViewModel vmModel)
        //{
        //    List<NavLinkViewModel> filter = new List<NavLinkViewModel>();
        //    filter = _navigate.GetaParentNavLink(vmModel);
        //    return Ok(filter);



        //}
        //[HttpPost("GetAllParentNavLink")]
        //public IActionResult GetAllParentNavLink([FromBody] NavLinkViewModel vmModel)
        //{
        //    //var _roles = Roles;
        //    List<NavLinkViewModel> filter = new List<NavLinkViewModel>();
        //    filter = _navigate.GetAllParentNavLink(vmModel);
        //    return Ok(filter);



        //}



        //[HttpPost("AddNavLink")]
        //public IActionResult AddNavLink([FromBody] NavLinkViewModel model)
        //{
        //    int a = _navigate.AddNavLink(model);
        //    if (a == 1)
        //    {
        //        return Ok(model);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
        //[HttpPut("UpdateNavLink")]
        //public IActionResult UpdateNavLink([FromBody] NavLinkViewModel model)
        //{
        //    int a = _navigate.UpdateNavLink(model);
        //    if (a == 1)
        //    {
        //        return Ok(model);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}



        //[HttpGet("GetallNavRoles")]
        //public IActionResult GetallNavRole()
        //{
        //    List<RoleNavigationModel> filter = new List<RoleNavigationModel>();
        //    filter = _navigate.GetallNavRoles();
        //    return Ok(filter);
        //}
        //[HttpPost("GetallRoleMapping")]
        //public IActionResult GetallRoleMapping([FromBody] RoleNavigationMapping vmModel)
        //{
        //    var filter = _navigate.GetallNavRolesMapping(vmModel);
        //    return Ok(filter);
        //}
        //[HttpPost("AddNavRole")]
        //public IActionResult AddNavRole([FromBody] RoleNavigationMapping abc)
        //{
        //    int a = _navigate.AddNavRole(abc);
        //    if (a == 1)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
        //[HttpPut("UpdateNavRole")]
        //public IActionResult UpdateNavRole([FromBody] RoleNavigationMapping abc)
        //{
        //    int a = _navigate.UpdateNavRole(abc);
        //    if (a == 1)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}



        //[HttpGet(@"GetMenuItems")]
        //public async Task<IActionResult> GetMenuItems()
        //{
        //    List<NavLinkViewModel> filter = new List<NavLinkViewModel>();
        //    filter = await _navigate.GetMenuItems(UserId);
        //    return Ok(filter);
        //}



        //[HttpGet("GetallNavTree")]
        //public IActionResult GetallNavLinkTree()
        //{
        //    List<NavTreeViewModel> filter = new List<NavTreeViewModel>();
        //    filter = _navigate.GetallNavTree();
        //    return Ok(filter);
        //}
        //[HttpPost("AddNavTree")]
        //public IActionResult AddNavTree([FromBody] NavTreeViewModel model)
        //{
        //    int a = _navigate.AddNavTree(model);
        //    if (a == 1)
        //    {
        //        return Ok(model);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
        //[HttpPut("UpdateNavTree")]
        //public IActionResult UpdateNavTree([FromBody] NavTreeViewModel model)
        //{
        //    int a = _navigate.UpdateNavTree(model);
        //    if (a == 1)
        //    {
        //        return Ok(model);
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
