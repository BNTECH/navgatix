using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
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
                    driver.UserId = driverInfo.UserId;
                    driver.Name = driverInfo.FirstName + " " + driverInfo.LastName;
                    driver.Phone = Convert.ToString(driverInfo.Mobile);
                    driver.LicenseNumber = driverInfo.LicenseNumber;
                    driver.LicenseExpiry = driverInfo.LicenseExpiry;
                    driver.PhotoUrl = driverInfo.ProfilePic;
                    driver.IsDeleted = false;
                    _db.GetDrivers().Add(driver);
                }
                else
                {
                    Driver driver = await _db.GetDrivers().Where(x => x.Id == driverInfo.Id).FirstOrDefaultAsync();
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
                if (transportInfo.CustTransId == null || transportInfo.CustTransId == 0)
                {
                    TransporterDetail transporter = new TransporterDetail();
                    transporter.CompanyName = transportInfo.FirstName + " " + transportInfo.LastName;
                    transporter.BankAccountNumber = transportInfo.BankAccountNumber;
                    transporter.GSTNumber = transportInfo.GSTNumber;
                    transporter.PANNumber = transportInfo.PANNumber;
                    transporter.IFSCCode = transportInfo.IFSCCode;
                    transporter.ProfileVerified = transportInfo.ProfileVerified;
                    transporter.IsDeleted = false;
                    transporter.UserId = transporter.UserId;
                    _db.TransporterDetails.Add(transporter);
                }
                else
                {
                    TransporterDetail transporter = await _db.TransporterDetails.Where(x => x.Id == transportInfo.CustTransId).FirstOrDefaultAsync();
                    transporter.CompanyName = transportInfo.FirstName + " " + transportInfo.LastName;
                    transporter.BankAccountNumber = transportInfo.BankAccountNumber;
                    transporter.GSTNumber = transportInfo.GSTNumber;
                    transporter.PANNumber = transportInfo.PANNumber;
                    transporter.IFSCCode = transportInfo.IFSCCode;
                    transporter.UserId = transporter.UserId;
                    transporter.ProfileVerified = transportInfo.ProfileVerified;
                    transporter.IsDeleted = false;
                }
                return await _db.SaveChangesAsync();
            }
            return 0;

        }

        public async Task<DriverViewModel> GetDriverDetails(string userId)
        {
            return await (from drv in _db.GetDrivers()
                          join trans in _db.TransporterDetails on drv.TransporterId equals trans.Id
                          join userInfo in _db.UserInformations on drv.UserId.ToLower() equals userInfo.Id.ToString().ToLower()
                          where !drv.IsDeleted.GetValueOrDefault() && drv.UserId == userId
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
                          join userInfo in _db.UserInformations on transport.UserId.ToLower() equals userInfo.Id.ToString().ToLower()
                          where !transport.IsDeleted.GetValueOrDefault() && transport.UserId == userId
                          select new TransporterViewModel
                          {
                              CustTransId = transport.Id,
                              Name = transport.CompanyName,
                              UserId = transport.UserId,
                              BankAccountNumber = transport.BankAccountNumber.Substring(transport.BankAccountNumber.Length-4, transport.BankAccountNumber.Length),
                              IFSCCode = transport.IFSCCode,
                              IsDeleted= transport.IsDeleted,
                              GSTNumber = transport.GSTNumber,
                              PANNumber = transport.PANNumber   ,
                               ProfileVerified = transport.ProfileVerified
                          }).FirstOrDefaultAsync();
        }
    }
}
