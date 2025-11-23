using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class GeoLocationPointViewModel
    {
        public long ID { get; set; }
        public Nullable<int> AddressId { get; set; }
        public string Title { get; set; }
        public NetTopologySuite.Geometries.Geometry GeoPoint { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDateTime { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTimeOffset> UpdatedDateTime { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<double> Radius { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string LocationCode { get; set; }
    }

    public class StateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

    }
}
