using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class GeoLocationPointViewModel
    {
        public long ID { get; set; }
        public Nullable<int> AddressId { get; set; }
        public string? Title { get; set; }
        public NetTopologySuite.Geometries.Geometry GeoPoint { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDateTime { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTimeOffset> UpdatedDateTime { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public Nullable<double> Radius { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string? LocationCode { get; set; }
    }

    public class StateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }
    public class StatelistViewModel
    {
        public static Expression<Func<State, StatelistViewModel>> ModelMapFrom = (model) => new StatelistViewModel
        {
            Id = model.Id,
            Name = model.Name,
            CountryId = model.CountryId,
            StateCode = model.StateCode,
            IsDeleted = model.IsDeleted,
        };
        public void ModelMapTo(State model)
        {
            model.Id = Id;
            model.Name = Name;
            model.CountryId = CountryId;
            model.StateCode = StateCode;
            model.IsDeleted = IsDeleted;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? CountryId { get; set; }

        public string? StateCode { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedDatetime { get; set; }

        public DateTime? UpdatedDatetime { get; set; }

        public bool? IsDeleted { get; set; }
        public string? Message { get; set; }
    }
}
