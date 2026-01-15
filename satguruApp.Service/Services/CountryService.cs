using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    public class CountryService : Repository<Country>, ICountryService
    {
        public CountryService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<CountryViewModel> GetById(int? id, string name = "")
        {
            return await (from country in _db.Countries
                          where country.Id == id.GetValueOrDefault() && country.Name.ToLower() == (string.IsNullOrEmpty(name) ? country.Name : name).ToLower() && country.IsDeleted == false
                          select new CountryViewModel
                          {
                              Id = country.Id,
                              Name = country.Name,
                              CountryCodeTwo = country.CountryCodeTwo,
                              CountryCodeThree = country.CountryCodeThree,
                              RegionName = country.RegionName,
                              IsDeleted = country.IsDeleted,
                              CreatedBy = country.CreatedBy,
                              CreatedDatetime = country.CreatedDatetime,
                              UpdatedBy = country.UpdatedBy,
                              UpdatedDatetime = country.UpdatedDatetime
                          }).FirstOrDefaultAsync();
        }
        public async Task<CountryViewModel> SaveChangeAsync(CountryViewModel countryView)
        {
            var country = await (from contry in _db.Countries
                                 where ((countryView.Id > 0 && contry.Id != countryView.Id) || (countryView.Id == 0) && (contry.Name.ToLower().Trim() == countryView.Name.ToLower().Trim()) && contry.IsDeleted == false)
                                 select contry).FirstOrDefaultAsync();
            if (country != null)
            {
                countryView.Message = "Country with same name already exists.";
                return countryView;
            }
            else
            {
                country = await (from cntry in _db.Countries
                                 where (countryView.Id > 0 && cntry.Id == countryView.Id && (cntry.Name.ToLower().Trim() == countryView.Name.ToLower().Trim()) && cntry.IsDeleted == false)
                                 select cntry).FirstOrDefaultAsync();
            }
            if (country == null)
            {
                country = new Country();
                countryView.ModelMapTo(country);
                _db.Countries.Add(country);
            }

            var stsCount = await _db.SaveChangesAsync();
            if (stsCount > 0)
            {
                countryView.Id = country.Id;

            }
            else
            {
                countryView.Message = "Error occurred while saving country details.";
            }
            return countryView;
        }
        public async Task<int> Delete(int id)
        {
            var countryDetail = await _db.Countries.Where(x => x.Id == id).FirstOrDefaultAsync();
            countryDetail.IsDeleted = !countryDetail.IsDeleted;
            return await _db.SaveChangesAsync();
        }
        public async Task<List<CountryViewModel>> GetAll(string name = "")
        {
            return await (from contry in _db.Countries
                          where contry.IsDeleted == false && (string.IsNullOrEmpty(name) || contry.Name.ToLower().Contains(name.ToLower()))

                          select new
                          CountryViewModel
                          {
                              Id = contry.Id,
                              Name = contry.Name,
                              CountryCodeTwo = contry.CountryCodeTwo,
                              CountryCodeThree = contry.CountryCodeThree,
                              RegionName = contry.RegionName,
                              IsDeleted = contry.IsDeleted,
                              CreatedBy = contry.CreatedBy,
                              CreatedDatetime = contry.CreatedDatetime,
                              UpdatedBy = contry.UpdatedBy,
                              UpdatedDatetime = contry.UpdatedDatetime
                          }).ToListAsync();
        }
    }
}
