using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class AppCustomer : Repository<CustomerDetail>, IAppCustormer
    {
        public AppCustomer(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<CustomerDetailViewModel> GetById(int? id, string userId = "")
        {
            return await (from cust in _db.CustomerDetails
                          where cust.Id == id.GetValueOrDefault() || cust.UserId == userId
                          select new CustomerDetailViewModel
                          {
                              Id = cust.Id,
                              Address = cust.Address,
                              City = cust.City,
                              State = cust.State,
                              UserId = cust.UserId,
                              CompanyName = cust.CompanyName,
                              GSTNumber = cust.GSTNumber
                          }).FirstOrDefaultAsync();
        }
        public async Task<int> SaveChangeAsync(CustomerDetailViewModel customerView)
        {
            var customer = await (from cust in _db.CustomerDetails
                                  where ((customerView.Id > 0 && cust.Id != customerView.Id) || 
                                  (customerView.Id == 0) && 
                                  (cust.CompanyName.ToLower().Trim() == customerView.CompanyName.ToLower().Trim() && !string.IsNullOrEmpty(customerView.CompanyName)) || 
                                  (cust.GSTNumber == customerView.GSTNumber && !string.IsNullOrEmpty( customerView.GSTNumber) && !string.IsNullOrEmpty(cust.GSTNumber)) && cust.IsDeleted == false)
                                  select cust).FirstOrDefaultAsync();
            if (customer != null)
            {
                return 0;
            }
            else
            {
                customer = await (from cust in _db.CustomerDetails
                                  where (customerView.Id > 0 && cust.Id == customerView.Id && (cust.CompanyName.ToLower().Trim() == customerView.CompanyName.ToLower().Trim() || string.IsNullOrEmpty(customerView.CompanyName) || cust.GSTNumber == customerView.GSTNumber || string.IsNullOrEmpty(customerView.GSTNumber)) && cust.IsDeleted == false)
                                  select cust).FirstOrDefaultAsync();
            }
            if (customer == null)
            {
                customer = new CustomerDetail();
                customerView.ModelMapTo(customer);
                _db.CustomerDetails.Add(customer);
            }
           
            await _db.SaveChangesAsync();
            return 1;
        }
        public async Task<int> Delete(int id)
        {
            var accountType = await _db.CustomerDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
            accountType.IsDeleted = !accountType.IsDeleted;
            return await _db.SaveChangesAsync();
        }
        public async Task<List<CustomerDetailViewModel>> GetAll(string name = "")
        {
            return await (from cust in _db.CustomerDetails
                          where cust.IsDeleted == false && (string.IsNullOrEmpty(name) || cust.CompanyName.ToLower().Contains(name.ToLower()))
                         
                          select new                           
                          CustomerDetailViewModel
                          {
                              Id = cust.Id,
                              Address = cust.Address,
                              City = cust.City,
                              State = cust.State,
                              UserId = cust.UserId,
                              CompanyName = cust.CompanyName,
                              GSTNumber = cust.GSTNumber
                          }).ToListAsync();
        }
    }
}
