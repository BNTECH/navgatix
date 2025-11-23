using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace satguruApp.Service.ViewModels
{

    public partial class CommonTypeViewModel
    {

        public static Expression<Func<CommonType, CommonTypeViewModel>> ModelMapFrom = (model) => new CommonTypeViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Code = model.Code,
            CTID = model.CTID,
            CreatedBy = model.CreatedBy,
            CreatedDatetime = model.CreatedDatetime,
            IsDeleted = model.IsDeleted,
            IsSystem = model.IsSystem,
            Keys = model.Keys,
            OrderBy = model.OrderBy,
            Source = model.Source,
            ValueDesc = model.ValueDesc,
            ValueDT = model.ValueDT,
            ValueInt = model.ValueInt,
            ValueStr = model.ValueStr
        };
        public void ModelMapTo(CommonType model)
        {
            Id = model.Id;
            Name = model.Name;
            Code = model.Code;
            CTID = model.CTID;
            CreatedBy = model.CreatedBy;
            CreatedDatetime = model.CreatedDatetime;
            IsDeleted = model.IsDeleted;
            IsSystem = model.IsSystem;
            Keys = model.Keys;
            OrderBy = model.OrderBy;
            Source = model.Source;
            ValueDesc = model.ValueDesc;
            ValueDT = model.ValueDT;
            ValueInt = model.ValueInt;
            ValueStr = model.ValueStr;
        }

        public int Id { get; set; }

        public string? Name { get; set; }

        public int? CTID { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDatetime { get; set; }

        public DateTime? CreatedDatetime { get; set; }

        public int? UpdatedBy { get; set; }

        public string? Keys { get; set; }

        public string? Code { get; set; }

        public bool? IsSystem { get; set; }

        public int? ValueInt { get; set; }

        public string? ValueStr { get; set; }

        public DateTime? ValueDT { get; set; }

        public string? ValueDesc { get; set; }

        public string? Source { get; set; }

        public int? OrderBy { get; set; }
    }

    public class CommonTypeSearchViewModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Keys { get; set; }
        public int? ParentId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
