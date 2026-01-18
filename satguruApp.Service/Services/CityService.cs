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
    public class CityService : Repository<City>, ICityService
    {
        public CityService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<CityViewModel> GetById(int? id, string name = "")
        {
            return await (from cty in _db.Cities
                          where cty.Id == id.GetValueOrDefault() && cty.CityName.ToLower() == (string.IsNullOrEmpty(name) ? cty.CityName : name).ToLower() && cty.IsDeleted == false
                          select new CityViewModel
                          {
                              Id = cty.Id,
                              CityName = cty.CityName,
                              StateId = cty.StateId,
                              IsDeleted = cty.IsDeleted,
                              CreatedBy = cty.CreatedBy,
                              CreatedDatetime = cty.CreatedDatetime,
                              UpdatedBy = cty.UpdatedBy,
                              UpdatedDatetime = cty.UpdatedDatetime
                          }).FirstOrDefaultAsync();
        }
        public async Task<CityViewModel> SaveChangeAsync(CityViewModel cityView)
        {
            var cities = await (from cty in _db.Cities
                                where ((cityView.Id > 0 && cty.Id != cityView.Id) || cityView.Id == 0) && cty.StateId == cityView.StateId && (cty.CityName.ToLower().Trim() == cityView.CityName.ToLower().Trim() && cty.IsDeleted == false)
                                select cty).FirstOrDefaultAsync();
            if (cities != null)
            {
                cityView.Message = "State with same name already exists.";
                return cityView;
            }
            else
            {
                cities = await (from cty in _db.Cities
                                where (cityView.Id > 0 && cty.Id == cityView.Id && (cty.CityName.ToLower().Trim() == cityView.CityName.ToLower().Trim()) && cty.IsDeleted == false)
                                select cty).FirstOrDefaultAsync();
            }
            if (cities == null)
            {
                cities = new City();
                cityView.ModelMapTo(cities);
                _db.Cities.Add(cities);
            }

            var stsCount = await _db.SaveChangesAsync();
            if (stsCount > 0)
            {
                cityView.Id = cities.Id;

            }
            else
            {
                cityView.Message = "Error occurred while saving cty details.";
            }
            return cityView;
        }
        public async Task<int> Delete(int id)
        {
            var ctyDetail = await _db.Cities.Where(x => x.Id == id).FirstOrDefaultAsync();
            ctyDetail.IsDeleted = !ctyDetail.IsDeleted;
            return await _db.SaveChangesAsync();
        }
        public async Task<List<CityViewModel>> GetAll(string name = "", int countryId = 1)
        {
            return await (from cty in _db.Cities
                          where cty.IsDeleted == false && (string.IsNullOrEmpty(name) || cty.CityName.ToLower().Contains(name.ToLower()) || (countryId > 0 && cty.StateId == countryId))

                          select new CityViewModel
                          {
                              Id = cty.Id,
                              CityName = cty.CityName,
                              CreatedDatetime = cty.CreatedDatetime,
                              CreatedBy = cty.CreatedBy,
                              StateId = cty.StateId,
                              UpdatedDatetime = cty.UpdatedDatetime,
                              UpdatedBy = cty.UpdatedBy,
                              IsDeleted = cty.IsDeleted,
                          }).ToListAsync();
        }
    }
}
