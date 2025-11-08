using Microsoft.AspNetCore.Mvc;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static satguruApp.Service.ViewModels.CompanyDetailViewModel;

namespace navgatix.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _Company;
        public CompanyController(ICompanyService Company)
        {
            _Company = Company;
        }





        [HttpGet("listt")]
        [ProducesResponseType(200, Type = typeof(List<CompanyViewModel>))]
        public IActionResult ListCompany()
        {
            CompanyFilterViewModel filter = new CompanyFilterViewModel();
            filter.Page = 0;
            filter.PageSize = 500;

            var list = _Company.GetCompanyList(filter);
            return Ok(list);
        }





        [HttpPost(@"list")]
        //  [Authorize("CompanyList")]
        [ProducesResponseType(200, Type = typeof(List<CompanyFilterViewModel>))]
        public IActionResult ListCompany([FromBody] CompanyFilterViewModel filter)
        {
            if (filter.Page < 0) { filter.Page = 0; }
            if (filter.PageSize > 500) { filter.PageSize = 500; }
            if (filter.PageSize < 1) { filter.PageSize = 2; }
            var list = _Company.GetCompanyList(filter);
            return Ok(list);
        }




        [HttpPost("add")]
        //  [Authorize("CompanyAdd")]
        [ProducesResponseType(201, Type = typeof(CompanyViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddCompany([FromBody] CompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Errors:" + ModelState.ErrorCount.ToString() + " " + String.Join("\n", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }
            model = await _Company.AddCompany(model);
            return Ok(model);
        }

        [Route("details/{id:int}")]
        public async Task<IActionResult> Get(HttpRequestMessage request, int id)
        {

            CompanyEditViewModel VMModel = await _Company.GetCompanyDetailsById(id);

            return Ok(VMModel);
        }
        [HttpPut("update")]
        //  [Authorize("CompanyUpdate")]
        [ProducesResponseType(201, Type = typeof(CompanyViewModel))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> UpdCompany([FromBody] CompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Errors:" + ModelState.ErrorCount.ToString() + " " + String.Join("\n", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
            }
            model = await _Company.UpdCompany(model);
            return Ok(model);
        }





        [HttpGet(@"get/{Id:int}")]
        //  [Authorize("CompanyUpdate")]
        [ProducesResponseType(200, Type = typeof(CompanyViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCompanyById(int Id)
        {
            var rkComp = await _Company.GetCompany(Id);
            return Ok(rkComp);
        }



        [HttpGet(@"getCompanyDetail/{Id:int}")]
        //   [Authorize("CompanyUpdate")]
        [ProducesResponseType(200, Type = typeof(CompanyViewModel))]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCompanyDetailById(int Id)
        {
            var rkComp = await _Company.GetCompanyDetail(Id);
            return Ok(rkComp);
        }



        ////[HttpPost]
        ////[Route(@"doActiveInActive")]
        ////public async Task<IActionResult> DoActiveAndInActive([FromBody] CompanyEditViewModel VMModel)
        ////{



        ////    VMModel = await _Company.DoActiveAndInactiveCompany(VMModel);
        ////    return Ok(VMModel);
        ////}



        #region ERPKick Company screen apis

        //[HttpPost(@"filterlist_ERP")]
        //public IActionResult FilterList([FromBody] CompanyFilterViewModel VMModel)
        //{
        //    if (VMModel.PageSize == 0)
        //    {
        //        VMModel.PageSize = 10;
        //        VMModel.Page = 1;
        //    }
        //    List<CompanyListViewModel> filterCompanyList = new List<CompanyListViewModel>();
        //    filterCompanyList = _Company.FilterCompanyList_ERP(VMModel);
        //    return Ok(new PaginationSet<object>(filterCompanyList, VMModel.PageSize, VMModel.Page, VMModel.TotalCount));
        //}



        //[HttpPost(@"adupdated_ERP")]
        //[ProducesResponseType(201, Type = typeof(CompanyEditViewModel))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(403)]
        //public IActionResult AddCompany_ERP([FromBody] CompanyEditViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Errors:" + ModelState.ErrorCount.ToString() + " " + String.Join("\n", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
        //    }

        //    if (model.ID > 0)
        //    {
        //        model = _Company.UpdateCompany_ERP(model);



        //    }
        //    else
        //    {
        //        model = _Company.AddCompany_ERP(model);
        //    }

        //    return Ok(model);
        //}



        //[HttpGet(@"get_ERP/{Id:int}")]
        //[ProducesResponseType(200, Type = typeof(CompanyEditViewModel))]
        //[ProducesResponseType(403)]
        //[ProducesResponseType(404)]
        //public async Task<IActionResult> GetCompanyById_ERP(int Id)
        //{
        //    var rkComp = await _Company.GetCompany_ERP(Id);
        //    return Ok(rkComp);
        //}
        #endregion
    }
}
