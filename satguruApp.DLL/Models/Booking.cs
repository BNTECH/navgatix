using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Booking
{
    public string? CustomerId { get; set; }

    public Guid? VehicleId { get; set; }

    public Guid? DriverId { get; set; }

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

    public long Id { get; set; }

    public virtual User Customer { get; set; }

    public virtual Driver Driver { get; set; }

    public virtual Vehicle Vehicle { get; set; }
}
