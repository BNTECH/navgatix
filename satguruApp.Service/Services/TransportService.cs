using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class TransportService : Repository<Driver>, ITransportService
    {
        public TransportService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;

        public async Task<int> SaveDriverAsync(DriverViewModel driverInfo)
        {
            var gender = await _db.Genders.Where(x => x.Name == driverInfo.Gender && !string.IsNullOrEmpty( driverInfo.Gender) || (x.Id == driverInfo.GenderId)).FirstOrDefaultAsync();
            if (gender == null && !string.IsNullOrEmpty(driverInfo.Gender))
            {
                gender = new Gender();
                gender.Name = driverInfo.Gender;
                gender.IsDeleted = false;
                _db.Genders.Add(gender);
                await _db.SaveChangesAsync();
            }
            if (driverInfo.UserId != null)
            {
                if (driverInfo.Id == null || driverInfo.Id == Guid.Empty)
                {
                    var driver = new Driver();
                    driver.Id = Guid.NewGuid();
                    driver.TransporterId = driverInfo.TransporterId;

                    // Resolve TransporterId from TransporterUserId if not provided
                    if ((driver.TransporterId == null || driver.TransporterId == 0) && !string.IsNullOrEmpty(driverInfo.TransporterUserId))
                    {
                        var transporter = await _db.TransporterDetails.FirstOrDefaultAsync(t => t.UserId == driverInfo.TransporterUserId);
                        if (transporter != null)
                        {
                            driver.TransporterId = transporter.Id;
                        }
                    }

                    driver.UserId = driverInfo.UserId;
                    driver.Name = driverInfo.FirstName + " " + driverInfo.LastName;
                    driver.Phone = Convert.ToString(driverInfo.Mobile);
                    driver.LicenseNumber = driverInfo.LicenseNumber;
                    driver.LicenseExpiry = driverInfo.LicenseExpiry;
                    driver.PhotoUrl = driverInfo.ProfilePic;
                    driver.IsDeleted = false;
                    _db.Drivers.Add(driver);
                }
                else
                {
                    Driver driver = await _db.Drivers.Where(x => x.Id == driverInfo.Id).FirstOrDefaultAsync();
                    driver.Name = driverInfo.FirstName + " " + driverInfo.LastName;
                    driver.Phone = Convert.ToString(driverInfo.Mobile);
                    driver.LicenseNumber = driverInfo.LicenseNumber;
                    driver.LicenseExpiry = driverInfo.LicenseExpiry;
                    driver.PhotoUrl = driverInfo.ProfilePic;
                    driver.IsDeleted = false;
                }
                return await _db.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<int> SaveTransporterAsync(TransporterViewModel transportInfo)
        {
            var gender = await _db.Genders.Where(x => x.Name == transportInfo.Gender || x.Id == transportInfo.GenderId).FirstOrDefaultAsync();
            if (gender == null && !string.IsNullOrEmpty(transportInfo.Gender))
            {
                gender = new Gender();
                gender.Name = transportInfo.Gender;
                gender.IsDeleted = false;
                _db.Genders.Add(gender);
                await _db.SaveChangesAsync();
            }
            if (transportInfo.UserId != null)
            {
                var transporter = new TransporterDetail();
                if (transportInfo.CustTransId == null || transportInfo.CustTransId == 0)
                {
                    
                    transporter.CompanyName = transportInfo.FirstName + " " + transportInfo.LastName;
                    transporter.BankAccountNumber = transportInfo.BankAccountNumber;
                    transporter.GSTNumber = transportInfo.GSTNumber;
                    transporter.IFSCCode = transportInfo.IFSCCode;
                    transporter.ProfileVerified = transportInfo.ProfileVerified;
                    transporter.IsDeleted = false;
                    transporter.UserId = transportInfo.UserId;
                    _db.TransporterDetails.Add(transporter);
                }
                else
                {
                     transporter = await _db.TransporterDetails.Where(x => x.Id == transportInfo.CustTransId).FirstOrDefaultAsync();
                    if (transporter != null)
                    {
                        transporter.CompanyName = transportInfo.FirstName + " " + transportInfo.LastName;
                        transporter.BankAccountNumber = transportInfo.BankAccountNumber;
                        transporter.GSTNumber = transportInfo.GSTNumber;
                        transporter.IFSCCode = transportInfo.IFSCCode;
                        transporter.UserId = transportInfo.UserId;
                        transporter.ProfileVerified = transportInfo.ProfileVerified;
                        transporter.IsDeleted = false;
                    }
                }
                return await _db.SaveChangesAsync();
            }
            return 0;

        }

        public async Task<DriverViewModel> GetDriverDetails(string userId)
        {
            return await (from drv in _db.Drivers
                          join trans in _db.TransporterDetails on drv.TransporterId equals trans.Id
                          join userInfo in _db.UserInformations on drv.UserId equals userInfo.UserId
                          where drv.IsDeleted != true && drv.UserId == userId
                          select new DriverViewModel
                          {
                              Id = drv.Id,
                              Name = drv.Name,
                              TransporterId = drv.TransporterId,
                              UserId = drv.UserId,
                              LicenseExpiry = drv.LicenseExpiry,
                              LicenseNumber = drv.LicenseNumber,
                              IsDeleted = drv.IsDeleted
                          }).FirstOrDefaultAsync();
        }
        public async Task<TransporterViewModel> GetTransporterDetails(string userId) {
            return await (from transport in _db.TransporterDetails
                          join userInfo in _db.UserInformations on transport.UserId equals userInfo.UserId
                          where transport.IsDeleted != true && transport.UserId == userId
                          select new TransporterViewModel
                          {
                              CustTransId = transport.Id,
                              Name = transport.CompanyName,
                              UserId = transport.UserId,
                              BankAccountNumber = !string.IsNullOrEmpty(transport.BankAccountNumber) && transport.BankAccountNumber.Length >= 4 
                                  ? transport.BankAccountNumber.Substring(transport.BankAccountNumber.Length - 4) 
                                  : (transport.BankAccountNumber ?? ""),
                              IFSCCode = transport.IFSCCode,
                              IsDeleted= transport.IsDeleted,
                              GSTNumber = transport.GSTNumber,
                               ProfileVerified = transport.ProfileVerified
                          }).FirstOrDefaultAsync();
        }

        public async Task<int> SaveDriverKYCAsync(DriverKYCViewModel kycInfo)
        {
            var kyc = await _db.DriverKYCs.FirstOrDefaultAsync(x => x.DriverId == kycInfo.DriverId && x.DocumentType == kycInfo.DocumentType);
            if (kyc == null)
            {
                kyc = new DriverKYC
                {
                    Id = Guid.NewGuid(),
                    DriverId = kycInfo.DriverId,
                    CreatedAt = DateTime.UtcNow
                };
                _db.DriverKYCs.Add(kyc);
            }

            kyc.DocumentType = kycInfo.DocumentType;
            kyc.DocumentUrl = kycInfo.DocumentUrl;
            kyc.VerifiedStatus = "Pending";

            return await _db.SaveChangesAsync();
        }

        public async Task<List<DriverKYCViewModel>> GetDriverKYCAsync(Guid driverId)
        {
            return await _db.DriverKYCs
                .Where(x => x.DriverId == driverId)
                .Select(x => new DriverKYCViewModel
                {
                    Id = x.Id,
                    DriverId = x.DriverId,
                    DocumentType = x.DocumentType,
                    DocumentUrl = x.DocumentUrl,
                    VerifiedStatus = x.VerifiedStatus,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();
        }

        public async Task<int> UpdateProfileStatusAsync(Guid driverId, string status)
        {
            var driver = await _db.Drivers.FirstOrDefaultAsync(x => x.Id == driverId);
            if (driver != null)
            {
                driver.ProfileStatus = status;
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<TransporterDashboardSummaryViewModel> GetDashboardSummary(string userId)
        {
            var transporter = await _db.TransporterDetails.FirstOrDefaultAsync(t => t.UserId == userId);
            if (transporter == null) return new TransporterDashboardSummaryViewModel();

            var fleetCount = await _db.Vehicles.CountAsync(v => v.TransporterId == transporter.Id && v.IsDeleted != true);
            var driverCount = await _db.Drivers.CountAsync(d => d.TransporterId == transporter.Id && d.IsDeleted != true);
            
            // Assuming bookings with status "Driver Assigned" or "Ride Started" are ongoing
            // You might need to adjust these state IDs based on your CommonType table
            var ongoingTrips = await (from b in _db.Bookings
                                      join v in _db.Vehicles on b.VehicleId equals v.Id
                                      where v.TransporterId == transporter.Id && (b.CT_BookingStatus == 2 || b.CT_BookingStatus == 3) // Example IDs
                                      select b).CountAsync();

            var totalRides = await (from b in _db.Bookings
                                    join v in _db.Vehicles on b.VehicleId equals v.Id
                                    where v.TransporterId == transporter.Id && b.CT_BookingStatus == 4 // Example Completed ID
                                    select b).CountAsync();

            var totalEarnings = await (from b in _db.Bookings
                                       join v in _db.Vehicles on b.VehicleId equals v.Id
                                       where v.TransporterId == transporter.Id && b.CT_BookingStatus == 4
                                       select b.FinalFare).SumAsync() ?? 0;

            return new TransporterDashboardSummaryViewModel
            {
                TotalFleet = fleetCount,
                ActiveDrivers = driverCount,
                OngoingTrips = ongoingTrips,
                TotalRides = totalRides,
                TotalEarnings = totalEarnings,
                PendingApprovals = 0 // Update if you have an approval queue
            };
        }

        public async Task<TransporterAnalyticsViewModel> GetTransporterAnalytics(string userId)
        {
            var transporter = await _db.TransporterDetails.FirstOrDefaultAsync(t => t.UserId == userId);
            if (transporter == null) return new TransporterAnalyticsViewModel();

            var vehicles = await _db.Vehicles.Where(v => v.TransporterId == transporter.Id && v.IsDeleted != true).Select(v => v.Id).ToListAsync();
            
            var allBookings = await _db.Bookings
                .Where(b => b.VehicleId.HasValue && vehicles.Contains(b.VehicleId.Value))
                .ToListAsync();

            var completedBookings = allBookings.Where(b => b.CT_BookingStatus == RideStatus.RideCompleted).ToList();
            var cancelledBookings = allBookings.Where(b => b.CT_BookingStatus == RideStatus.Cancelled).ToList();

            var today = DateTime.UtcNow.Date;
            var dailyBookings = completedBookings.Where(b => b.CreatedAt.HasValue && b.CreatedAt.Value.Date == today).ToList();

            decimal totalDist = 0;
            decimal dailyDist = 0;

            foreach (var b in completedBookings)
            {
                var dist = (decimal)CalculateDistanceKm(b.PickupLat, b.PickupLng, b.DropLat, b.DropLng);
                totalDist += dist;
                if (b.CreatedAt.HasValue && b.CreatedAt.Value.Date == today)
                {
                    dailyDist += dist;
                }
            }

            decimal performance = 100m;
            int closedRides = completedBookings.Count + cancelledBookings.Count;
            if (closedRides > 0)
            {
                performance = Math.Round((decimal)completedBookings.Count / closedRides * 100m, 1);
            }

            return new TransporterAnalyticsViewModel
            {
                DailyTrips = dailyBookings.Count,
                TotalTrips = completedBookings.Count,
                TotalDistanceKm = Math.Round(totalDist, 2),
                DailyDistanceKm = Math.Round(dailyDist, 2),
                EstimatedFuelLiters = Math.Round(totalDist / 4m, 2), // Rough estimate: 4km per liter for heavy fleet
                DailyEarnings = dailyBookings.Sum(b => b.FinalFare ?? 0),
                TotalEarnings = completedBookings.Sum(b => b.FinalFare ?? 0),
                PerformanceScore = performance
            };
        }

        private double CalculateDistanceKm(decimal? lat1, decimal? lon1, decimal? lat2, decimal? lon2)
        {
            if (!lat1.HasValue || !lon1.HasValue || !lat2.HasValue || !lon2.HasValue) return 0;
            var R = 6371; // Earth radius km
            var dLat = ToRadians((double)(lat2.Value - lat1.Value));
            var dLon = ToRadians((double)(lon2.Value - lon1.Value));
            
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians((double)lat1.Value)) * Math.Cos(ToRadians((double)lat2.Value)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRadians(double angle) => angle * Math.PI / 180.0;

        public async Task<List<TransporterFleetItemViewModel>> GetFleetOverview(string userId)
        {
            var transporter = await _db.TransporterDetails.FirstOrDefaultAsync(t => t.UserId == userId);
            if (transporter == null) return new List<TransporterFleetItemViewModel>();

            var vehicles = await _db.Vehicles
                .Where(v => v.TransporterId == transporter.Id && v.IsDeleted != true)
                .OrderBy(v => v.VehicleNumber)
                .ToListAsync();

            if (!vehicles.Any()) return new List<TransporterFleetItemViewModel>();

            var vehicleIds = vehicles.Select(v => v.Id).ToList();
            var bookings = await _db.Bookings
                .Where(b => b.VehicleId.HasValue && vehicleIds.Contains(b.VehicleId.Value) && b.IsDeleted != true)
                .ToListAsync();

            var driverIds = bookings.Where(b => b.DriverId.HasValue).Select(b => b.DriverId.Value).Distinct().ToList();
            var drivers = await _db.Drivers.Where(d => driverIds.Contains(d.Id) && d.IsDeleted != true).ToListAsync();
            var driverLookup = drivers.ToDictionary(d => d.Id, d => d);

            var bookingsByVehicle = bookings
                .Where(b => b.VehicleId.HasValue)
                .GroupBy(b => b.VehicleId.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var bookingsByDriver = bookings
                .Where(b => b.DriverId.HasValue)
                .GroupBy(b => b.DriverId.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var liveTrackings = await _db.LiveVehicleTrackings
                .Where(x => x.VehicleId.HasValue && vehicleIds.Contains(x.VehicleId.Value) && x.IsDeleted != true)
                .ToListAsync();

            var latestTracking = liveTrackings
                .GroupBy(x => x.VehicleId.Value)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.LastUpdated).First());

            var typeIds = vehicles.Where(v => v.CT_VehicleType.HasValue).Select(v => v.CT_VehicleType.Value).Distinct().ToList();
            var commonTypeNames = await _db.CommonTypes
                .Where(ct => typeIds.Contains(ct.Id))
                .ToDictionaryAsync(ct => ct.Id, ct => ct.Name);

            var nowUtc = DateTime.UtcNow;
            var threshold = TimeSpan.FromMinutes(10);

            var fleet = new List<TransporterFleetItemViewModel>();

            foreach (var vehicle in vehicles)
            {
                bookingsByVehicle.TryGetValue(vehicle.Id, out var vehicleBookings);
                var vehicleBookingList = vehicleBookings ?? new List<Booking>();
                var completedVehicleBookings = vehicleBookingList.Where(b => b.CT_BookingStatus == RideStatus.RideCompleted).ToList();
                var activeBooking = vehicleBookingList
                    .Where(b => b.CT_BookingStatus != RideStatus.RideCompleted && b.CT_BookingStatus != RideStatus.Cancelled)
                    .OrderByDescending(b => b.CreatedAt ?? DateTime.MinValue)
                    .FirstOrDefault();

                Guid? driverId = activeBooking?.DriverId;
                var driver = driverId.HasValue && driverLookup.TryGetValue(driverId.Value, out var driverDetail) ? driverDetail : null;
                List<Booking> driverBookings = null;
                if (driverId.HasValue)
                {
                    bookingsByDriver.TryGetValue(driverId.Value, out driverBookings);
                }
                var driverCompletedBookings = (driverBookings ?? new List<Booking>())
                    .Where(b => b.CT_BookingStatus == RideStatus.RideCompleted)
                    .ToList();

                var driverCompletedRides = driverCompletedBookings.Count;
                var driverEarnings = driverCompletedBookings.Sum(b => b.FinalFare ?? 0);

                var vehicleCompletedRides = completedVehicleBookings.Count;
                var vehicleEarnings = completedVehicleBookings.Sum(b => b.FinalFare ?? 0);

                latestTracking.TryGetValue(vehicle.Id, out var liveEntry);
                var latitude = liveEntry?.LastLatitude ?? vehicle.CurrentLatitude;
                var longitude = liveEntry?.LastLongitude ?? vehicle.CurrentLongitude;
                var liveStatus = "No Signal";
                if (liveEntry != null)
                {
                    if (liveEntry.LastUpdated.HasValue && (nowUtc - liveEntry.LastUpdated.Value) <= threshold)
                    {
                        liveStatus = "Live";
                    }
                    else
                    {
                        liveStatus = "Stale";
                    }
                }

                var routeSummary = activeBooking != null
                    ? $"{(string.IsNullOrWhiteSpace(activeBooking.PickupAddress) ? "Pickup" : activeBooking.PickupAddress)} -> {(string.IsNullOrWhiteSpace(activeBooking.DropAddress) ? "Drop" : activeBooking.DropAddress)}"
                    : "Idle";

                var rawStatus = activeBooking != null
                    ? RideStatus.ToName(activeBooking.CT_BookingStatus)
                    : null;
                var friendlyStatus = FormatFriendlyStatus(rawStatus);

                var vehicleTypeName = vehicle.CT_VehicleType.HasValue && commonTypeNames.TryGetValue(vehicle.CT_VehicleType.Value, out var typeName)
                    ? typeName
                    : string.Empty;

                fleet.Add(new TransporterFleetItemViewModel
                {
                    VehicleId = vehicle.Id,
                    VehicleNumber = vehicle.VehicleNumber,
                    VehicleName = string.IsNullOrWhiteSpace(vehicle.VehicleName) ? vehicle.VehicleNumber : vehicle.VehicleName,
                    VehicleTypeName = vehicleTypeName,
                    DriverId = driver?.Id,
                    DriverName = driver?.Name ?? "Unassigned",
                    DriverPhone = driver?.Phone,
                    DriverUserId = driver?.UserId,
                    ActiveBookingId = activeBooking?.Id,
                    RideStatus = friendlyStatus,
                    RouteSummary = activeBooking != null ? routeSummary : "Idle",
                    VehicleCompletedRides = vehicleCompletedRides,
                    VehicleEarnings = vehicleEarnings,
                    DriverCompletedRides = driverCompletedRides,
                    DriverEarnings = driverEarnings,
                    Latitude = latitude,
                    Longitude = longitude,
                    LiveUpdatedAt = liveEntry?.LastUpdated,
                    LiveStatus = liveStatus,
                });
            }

            return fleet;
        }

        private static string FormatFriendlyStatus(string rawStatus)
        {
            if (string.IsNullOrWhiteSpace(rawStatus))
            {
                return "Available";
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rawStatus.Replace('_', ' '));
        }
    }
}

