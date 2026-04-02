using System;
using System.Collections.Generic;

namespace satguruApp.Service.ViewModels
{
    public class TransporterDashboardSummaryViewModel
    {
        public int TotalFleet { get; set; }
        public int ActiveDrivers { get; set; }
        public int OngoingTrips { get; set; }
        public int PendingApprovals { get; set; }
        public decimal TotalEarnings { get; set; }
        public int TotalRides { get; set; }
    }
}
