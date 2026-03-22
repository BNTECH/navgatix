using System;
using System.Collections.Generic;

namespace satguruApp.Service.ViewModels
{
    public class ComplaintReportViewModel
    {
        public long? RideId { get; set; }
        public string? IssueType { get; set; } // complaint | ride_issue | other
        public string? Description { get; set; }
        public int? CreatedBy { get; set; }
    }

    public class DisputeResultViewModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public long? ComplaintId { get; set; }
    }

    public class DisputeItemViewModel
    {
        public long Id { get; set; }
        public long? RideId { get; set; }
        public string? IssueType { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public string? Resolution { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
