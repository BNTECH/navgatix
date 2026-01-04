using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public partial class BookingViewModel
    {
        public static Expression<Func<Booking, BookingViewModel>> ModelMapFrom = (model) => new BookingViewModel
        {
            Id = model.Id,
            CustomerId = model.CustomerId,
            CustomerName = model.Customer.FirstName+ (!string.IsNullOrEmpty(model.Customer.LastName)? model.Customer.LastName: string.Empty),
            VehicleId = model.VehicleId,
            VehicleNumber = model.Vehicle.VehicleNumber,
            DriverId = model.DriverId,
            DriverName = model.Driver.Name,
            PickupAddress = model.PickupAddress,
            PickupLat = model.PickupLat,
            PickupLng = model.PickupLng,
            DropAddress = model.DropAddress,
            DropLat = model.DropLat,
            DropLng = model.DropLng,
            GoodsType = model.GoodsType,
            GoodsWeight = model.GoodsWeight,
            EstimatedFare = model.EstimatedFare,
            FinalFare = model.FinalFare,
            CT_BookingStatus = model.CT_BookingStatus,
            ScheduledTime = model.ScheduledTime,
            CreatedAt = model.CreatedAt,
            IsAvailable = model.IsAvailable
        };
        public void ModelMapTo(Booking model)
        {
            model.Id = Id;
            model.CustomerId = CustomerId;
            model.VehicleId = VehicleId;
            model.DriverId = DriverId;
            model.PickupAddress = PickupAddress;
            model.PickupLat = PickupLat;
            model.PickupLng = PickupLng;
            model.DropAddress = DropAddress;
            model.DropLat = DropLat;
            model.DropLng = DropLng;
            model.GoodsType = GoodsType;

            model.GoodsWeight = GoodsWeight;
            model.EstimatedFare = EstimatedFare;
            model.FinalFare = FinalFare;
            model.CT_BookingStatus = CT_BookingStatus;
            model.ScheduledTime = ScheduledTime;
            model.CreatedAt = CreatedAt;
            model.IsAvailable = IsAvailable;

        }
        public long Id { get; set; }

        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public Guid? VehicleId { get; set; }
        public string? VehicleNumber { get; set; }

        public Guid? DriverId { get; set; }
        public string? DriverName { get; set; }

        public string? PickupAddress { get; set; }

        public string? DropAddress { get; set; }

        public decimal? PickupLat { get; set; }

        public decimal? PickupLng { get; set; }

        public decimal? DropLat { get; set; }

        public decimal? DropLng { get; set; }

        public string? GoodsType { get; set; }

        public decimal? GoodsWeight { get; set; }

        public decimal? EstimatedFare { get; set; }

        public decimal? FinalFare { get; set; }

        public int? CT_BookingStatus { get; set; }

        public DateTime? ScheduledTime { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool? IsAvailable { get; set; }

        public bool? IsDeleted { get; set; }

        public string? Message { get; set; }
        public virtual ICollection<BookingBenefitsLog> BookingBenefitsLogs { get; set; } = new List<BookingBenefitsLog>();

        public virtual User Customer { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual BookingViewModel Vehicle { get; set; }
    }
}
