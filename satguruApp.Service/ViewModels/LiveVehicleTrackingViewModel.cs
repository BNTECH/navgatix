using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class LiveVehicleTrackingViewModel
    {
        public long Id { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid? VehicleId { get; set; }
        public string VehicleNumber { get; set; }

        public decimal? LastLatitude { get; set; }

        public decimal? LastLongitude { get; set; }

        public DateTime? LastUpdated { get; set; }
        public string? Message   { get; set; }

        public virtual VehicleViewModel Vehicle { get; set; }
    }
}
