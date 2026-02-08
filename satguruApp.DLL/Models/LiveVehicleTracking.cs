using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class LiveVehicleTracking
{
    public long Id { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid? VehicleId { get; set; }

    public decimal? LastLatitude { get; set; }

    public decimal? LastLongitude { get; set; }

    public DateTime? LastUpdated { get; set; }
    public string? DeviceId { get; set; }

    public virtual Vehicle Vehicle { get; set; }
}
