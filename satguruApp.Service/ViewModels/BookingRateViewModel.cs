using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
        public partial class BookingRateViewModel
        {
            public Guid Id { get; set; }

            public string VehicleType { get; set; }

            public decimal? MinDistanceKm { get; set; }

            public decimal? MaxDistanceKm { get; set; }

            public decimal? BaseRatePerKm { get; set; }

            public decimal? ExtraWeightChargePerTon { get; set; }

            public DateTime? EffectiveFrom { get; set; }

            public bool? IsActive { get; set; }

            public bool? IsDeleted { get; set; }
        }
}
