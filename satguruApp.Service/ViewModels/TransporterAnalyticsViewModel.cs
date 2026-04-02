using System;

namespace satguruApp.Service.ViewModels
{
    public class TransporterAnalyticsViewModel
    {
        public int DailyTrips { get; set; }
        public int TotalTrips { get; set; }
        public decimal TotalDistanceKm { get; set; }
        public decimal DailyDistanceKm { get; set; }
        public decimal EstimatedFuelLiters { get; set; } 
        public decimal DailyEarnings { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal PerformanceScore { get; set; }
    }
}
