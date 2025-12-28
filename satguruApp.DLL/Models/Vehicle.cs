using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Vehicle
{
    public Guid Id { get; set; }

    public long? TransporterId { get; set; }

    public string? VehicleNumber { get; set; }

    public string? VehicleName { get; set; }

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
    
    public int? CT_VehicleType { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }

    public int? CTBodyType { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<LiveVehicleTracking> LiveVehicleTrackings { get; set; } = new List<LiveVehicleTracking>();

    public virtual TransporterDetail Transporter { get; set; }
}
