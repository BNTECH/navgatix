using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class DisputeService : IDisputeService
    {
        private readonly SatguruDBContext _db;

        public DisputeService(SatguruDBContext db)
        {
            _db = db;
        }

        public Task<DisputeResultViewModel> ReportComplaintAsync(ComplaintReportViewModel model)
        {
            var payload = new ComplaintReportViewModel
            {
                RideId = model.RideId,
                IssueType = string.IsNullOrWhiteSpace(model.IssueType) ? "complaint" : model.IssueType,
                Description = model.Description,
                CreatedBy = model.CreatedBy,
            };
            return SaveComplaintAsync(payload);
        }

        public Task<DisputeResultViewModel> ReportRideIssueAsync(ComplaintReportViewModel model)
        {
            var payload = new ComplaintReportViewModel
            {
                RideId = model.RideId,
                IssueType = "ride_issue",
                Description = model.Description,
                CreatedBy = model.CreatedBy,
            };
            return SaveComplaintAsync(payload);
        }

        public async Task<List<DisputeItemViewModel>> GetByRideAsync(long rideId)
        {
            return await _db.Complaints
                .Where(x => x.Booking_Id == rideId && x.IsDeleted != true)
                .OrderByDescending(x => x.CreatedDateTime)
                .Select(x => new DisputeItemViewModel
                {
                    Id = x.Id,
                    RideId = x.Booking_Id,
                    IssueType = x.Issue_Type,
                    Description = x.Description,
                    Status = x.Status,
                    Resolution = x.Resolution,
                    CreatedDateTime = x.CreatedDateTime,
                })
                .ToListAsync();
        }

        private async Task<DisputeResultViewModel> SaveComplaintAsync(ComplaintReportViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                return new DisputeResultViewModel { Success = false, Message = "Description is required." };
            }

            var complaint = new Complaint
            {
                Booking_Id = model.RideId,
                Issue_Type = string.IsNullOrWhiteSpace(model.IssueType) ? "complaint" : model.IssueType,
                Description = model.Description,
                Status = 0, // open
                Resolution = string.Empty,
                IsDeleted = false,
                CreatedBy = model.CreatedBy,
                CreatedDateTime = DateTime.UtcNow,
            };

            _db.Complaints.Add(complaint);
            await _db.SaveChangesAsync();

            return new DisputeResultViewModel
            {
                Success = true,
                Message = "Dispute reported successfully.",
                ComplaintId = complaint.Id,
            };
        }
    }
}

