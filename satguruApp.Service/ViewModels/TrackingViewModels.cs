using System;

namespace satguruApp.Service.ViewModels
{
    public class RideTrackingSnapshotViewModel
    {
        public long BookingId { get; set; }
        public string? CustomerId { get; set; }
        public Guid? DriverId { get; set; }
        public Guid? VehicleId { get; set; }
        public string? RideStatus { get; set; }
        public string? PickupAddress { get; set; }
        public string? DropAddress { get; set; }
        public decimal? PickupLat { get; set; }
        public decimal? PickupLng { get; set; }
        public decimal? DropLat { get; set; }
        public decimal? DropLng { get; set; }
        public decimal? DriverLatitude { get; set; }
        public decimal? DriverLongitude { get; set; }
        public DateTime? LastUpdatedUtc { get; set; }
        public double? DistanceRemainingKm { get; set; }
        public int? EstimatedArrivalMinutes { get; set; }
    }
}
