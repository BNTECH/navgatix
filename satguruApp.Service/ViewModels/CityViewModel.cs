using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class CityViewModel
    {
        public static Expression<Func<City, CityViewModel>> ModelMapFrom = (model) => new CityViewModel
        {
            Id = model.Id,
            CityName = model.CityName,
            StateId = model.StateId,
            IsDeleted = model.IsDeleted,
        };
        public void ModelMapTo(City model)
        {
            model.Id = Id;
            model.CityName = CityName;
            model.StateId = StateId;
            model.IsDeleted = IsDeleted;
        }
        public int Id { get; set; }

        public string? CityName { get; set; }

        public int? StateId { get; set; }

        public bool? IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedDatetime { get; set; }

        public DateTime? UpdatedDatetime { get; set; }
        public string? Message { get; set; }
    }

}
