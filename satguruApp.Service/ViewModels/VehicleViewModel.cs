using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public partial class VehicleViewModel
    {
        public static Expression<Func<Vehicle, VehicleViewModel>> ModelMapFrom = (model) => new VehicleViewModel
        {
            Id = model.Id,
            VehicleNumber = model.VehicleNumber,
            TransporterId = model.TransporterId.GetValueOrDefault(),
            CT_VehicleType = model.CT_VehicleType.GetValueOrDefault(),
            //TransporterName =model.Transporter.c // model.Transporter.FirstName + (!string.IsNullOrEmpty(model.Transporter.LastName) ? model.Transporter.LastName : string.Empty),
            CapacityTons = model.CapacityTons,
            SizeCubicMeters = model.SizeCubicMeters,
            RCNumber = model.RCNumber,
            InsuranceExpiry = model.InsuranceExpiry,
            RoadTaxExpiry = model.RoadTaxExpiry,
            PermitExpiry = model.PermitExpiry,
            CurrentLatitude = model.CurrentLatitude,
            CurrentLongitude = model.CurrentLongitude,
            UploadPhoneUrl = model.UploadPhoneUrl,
            IsAvailable = model.IsAvailable,
            CTBodyType = model.CTBodyType,
            
            IsDeleted = model.IsDeleted
        };


        public Guid Id { get; set; }

        public long TransporterId { get; set; }
        public string? TransporterName { get; set; }

        public string? VehicleNumber { get; set; }

        public string? VehicleTypeName { get; set; }
        public int? CT_VehicleType { get; set; }

        public decimal? CapacityTons { get; set; }

        public decimal? SizeCubicMeters { get; set; }

        public string? RCNumber { get; set; }

        public DateTime? InsuranceExpiry { get; set; }

        public DateTime? RoadTaxExpiry { get; set; }

        public DateTime? PermitExpiry { get; set; }

        public decimal? CurrentLatitude { get; set; }

        public decimal? CurrentLongitude { get; set; }

        public string? UploadPhoneUrl { get; set; }

        public bool? IsAvailable { get; set; }

        public bool? IsDeleted { get; set; }
        public int? CTBodyType { get; set; }
        public string? BodyTypeName { get; set; }
        public string? Message { get; set; }

        public virtual ICollection<BookingViewModel> Bookings { get; set; } = new List<BookingViewModel>();

        public virtual ICollection<DocumentViewModel> Documents { get; set; } = new List<DocumentViewModel>();

        public virtual ICollection<LiveVehicleTrackingViewModel> LiveVehicleTrackings { get; set; } = new List<LiveVehicleTrackingViewModel>();

        public virtual UserViewModel Transporter { get; set; }
        public void ModelMapTo(Vehicle model) {
            model.Id = Id;
            model.VehicleNumber = VehicleNumber;
            model.TransporterId = TransporterId;
            model.CT_VehicleType = CT_VehicleType;
           // model.VehicleType = VehicleTypeName;
            model.CapacityTons = CapacityTons;
            model.SizeCubicMeters = SizeCubicMeters;
            model.RCNumber = RCNumber;
            model.InsuranceExpiry = InsuranceExpiry;
            model.RoadTaxExpiry = RoadTaxExpiry;
            model.PermitExpiry = PermitExpiry;
            model.CurrentLatitude = CurrentLatitude;
            model.CurrentLongitude = CurrentLongitude;
            model.UploadPhoneUrl = UploadPhoneUrl;
            model.IsAvailable = IsAvailable;
            model.CTBodyType = CTBodyType;
        }
    }

}
