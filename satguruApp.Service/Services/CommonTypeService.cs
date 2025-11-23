using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace satguruApp.Service.Services
{
    public class CommonTypeService : Repository<CommonType>, ICommonTypeService
    {
        public CommonTypeService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<List<CommonTypeViewModel>> GetAll()
        {
            var cmn = new List<CommonTypeViewModel>();
            try
            {
                cmn =
            await (from common in _db.CommonTypes where common.IsDeleted == false select common).Select(x => new CommonTypeViewModel
            {
                Id = x.Id,
                Name = x.Name ?? "",
                Code = x.Code ?? "",
                CTID = x.CTID,
                CreatedBy = x.CreatedBy,
                CreatedDatetime = x.CreatedDatetime,
                IsDeleted = x.IsDeleted,
                IsSystem = x.IsSystem,
                Keys = x.Keys ?? "",
                OrderBy = x.OrderBy,
                Source = x.Source ?? "",
                ValueDesc = x.ValueDesc ?? "",
                ValueDT = x.ValueDT,
                ValueInt = x.ValueInt,
                ValueStr = x.ValueStr ?? ""
            }).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return cmn;

        }
        public async Task<CommonTypeViewModel> GetById(int id)
        {
            return await (from common in _db.CommonTypes where common.IsDeleted == false && common.Id == id select common).Select(x => new CommonTypeViewModel
            {
                Id = x.Id,
                Name = x.Name ?? "",
                Code = x.Code ?? "",
                CTID = x.CTID,
                CreatedBy = x.CreatedBy,
                CreatedDatetime = x.CreatedDatetime,
                IsDeleted = x.IsDeleted,
                IsSystem = x.IsSystem,
                Keys = x.Keys ?? "",
                OrderBy = x.OrderBy,
                Source = x.Source ?? "",
                ValueDesc = x.ValueDesc ?? "",
                ValueDT = x.ValueDT,
                ValueInt = x.ValueInt,
                ValueStr = x.ValueStr ?? ""
            }).FirstOrDefaultAsync();
        }
        public async Task<int> SaveUpdate(CommonTypeViewModel commonTypeView)
        {
            var commonTp = await _db.CommonTypes.Where(x => x.Id == commonTypeView.Id || x.Name.ToLower().Trim() == commonTypeView.Name.ToLower().Trim()).FirstOrDefaultAsync();
            if (commonTp != null)
            {
                commonTypeView.ModelMapTo(commonTp);
            }
            else
            {
                commonTp = new CommonType();
                commonTypeView.ModelMapTo(commonTp);
            }
            return await _db.SaveChangesAsync();
        }
        public async Task<int> Delete(int id)
        {
            var commonTp = await _db.CommonTypes.Where(x => x.Id == id).FirstOrDefaultAsync();
            commonTp.IsDeleted = true;
            return await _db.SaveChangesAsync();
        }
        public async Task<List<CommonTypeWithKeyViewModel>> GetCommonTypeListWithAndId(String code, string flag)
        {
            return await (from common in _db.CommonTypes
                          join comParent in _db.CommonTypes on common.CTID equals comParent.Id
                          where common.IsDeleted == false && comParent.Code == code && (string.IsNullOrEmpty(flag) || common.Code.ToLower().Contains(flag.ToLower()))
                          select new CommonTypeWithKeyViewModel
                          {
                              Id = common.Id,
                              Code = "<i class='fa fa-square' style ='color:" + common.ValueStr ?? "" + "'></i>",
                              Name = common.Name ?? "",
                              IsDisabled = false,
                              CodeValue = common.Code ?? "",
                              Keys = common.Keys ?? "",
                              ValueDesc = common.ValueDesc ?? "",
                              ValueInt = common.ValueInt.GetValueOrDefault(),
                              OrderBy = common.OrderBy.GetValueOrDefault(),
                              ValueStr = common.ValueStr ?? ""
                          }).ToListAsync();
        }
        public async Task<List<CommonTypeViewModel>> FilterCommonTypes(CommonTypeSearchViewModel model)
        {
            return await (from common in _db.CommonTypes where common.IsDeleted == (model.IsDeleted ?? false) && common.Name.ToLower().Contains(model.Name.ToLower()) && common.CTID == (model.ParentId ?? common.CTID) && common.Keys == (model.Keys ?? common.Keys) select common).Select(x => new CommonTypeViewModel
            {
                Id = x.Id,
                Name = x.Name ?? "",
                Code = x.Code ?? "",
                CTID = x.CTID,
                CreatedBy = x.CreatedBy,
                CreatedDatetime = x.CreatedDatetime,
                IsDeleted = x.IsDeleted,
                IsSystem = x.IsSystem,
                Keys = x.Keys ?? "",
                OrderBy = x.OrderBy,
                Source = x.Source ?? "",
                ValueDesc = x.ValueDesc ?? "",
                ValueDT = x.ValueDT,
                ValueInt = x.ValueInt,
                ValueStr = x.ValueStr ?? ""
            }).ToListAsync();

        }
    }
}
