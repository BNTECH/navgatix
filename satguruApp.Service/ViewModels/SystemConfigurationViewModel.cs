using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class SystemConfigurationViewModel
    {
        public static Expression<Func<SystemConfiguration, SystemConfigurationViewModel>> ModelMapFrom = (model) => new SystemConfigurationViewModel
        { 
        Id=model.Id,
            Field = model.Field,
            Value = model.Value,
            IsDeleted = model.IsDeleted,
            FieldText = model.FieldText,
            CTTableId = model.CTTableId,
            IsJson = model.IsJson


        };
        public void ModelMapTo(SystemConfiguration model)
        {
            model.Id = Id;
            model.Field = Field;
            model.Value = Value;
            model.IsDeleted = IsDeleted;
            model.FieldText = FieldText;
            model.CTTableId = CTTableId;
            model.IsJson = IsJson;
        }
        public int Id { get; set; }

        public string Field { get; set; }

        public string Value { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public DateTime? UpdateDatetime { get; set; }

        public string FieldText { get; set; }

        public int? CTTableId { get; set; }

        public bool? IsJson { get; set; }
    }
}
