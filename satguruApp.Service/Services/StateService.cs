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
    public class StateService : Repository<State>, IStateService
    {
        public StateService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<StatelistViewModel> GetById(int? id, string name = "")
        {
            return await (from state in _db.States
                          where state.Id == id.GetValueOrDefault() && state.Name.ToLower() == (string.IsNullOrEmpty(name) ? state.Name : name).ToLower() && state.IsDeleted == false
                          select new StatelistViewModel
                          {
                              Id = state.Id,
                              Name = state.Name,
                              StateCode = state.StateCode,
                              CountryId = state.CountryId,
                              IsDeleted = state.IsDeleted,
                              CreatedBy = state.CreatedBy,
                              CreatedDatetime = state.CreatedDatetime,
                              UpdatedBy = state.UpdatedBy,
                              UpdatedDatetime = state.UpdatedDatetime
                          }).FirstOrDefaultAsync();
        }
        public async Task<StatelistViewModel> SaveChangeAsync(StatelistViewModel stateView)
        {
            var states = await (from stat in _db.States
                                where ((stateView.Id > 0 && stat.Id != stateView.Id) || stateView.Id == 0) && stat.CountryId == stateView.CountryId && (stat.Name.ToLower().Trim() == stateView.Name.ToLower().Trim() && stat.IsDeleted == false)
                                select stat).FirstOrDefaultAsync();
            if (states != null)
            {
                stateView.Message = "State with same name already exists.";
                return stateView;
            }
            else
            {
                states = await (from stat in _db.States
                                where (stateView.Id > 0 && stat.Id == stateView.Id && (stat.Name.ToLower().Trim() == stateView.Name.ToLower().Trim()) && stat.IsDeleted == false)
                                select stat).FirstOrDefaultAsync();
            }
            if (states == null)
            {
                states = new State();
                stateView.ModelMapTo(states);
                _db.States.Add(states);
            }

            var stsCount = await _db.SaveChangesAsync();
            if (stsCount > 0)
            {
                stateView.Id = states.Id;

            }
            else
            {
                stateView.Message = "Error occurred while saving state details.";
            }
            return stateView;
        }
        public async Task<int> Delete(int id)
        {
            var stateDetail = await _db.States.Where(x => x.Id == id).FirstOrDefaultAsync();
            stateDetail.IsDeleted = !stateDetail.IsDeleted;
            return await _db.SaveChangesAsync();
        }
        public async Task<List<StatelistViewModel>> GetAll(string name = "", int countryId = 1)
        {
            return await (from cust in _db.States
                          where cust.IsDeleted == false && (string.IsNullOrEmpty(name) || cust.Name.ToLower().Contains(name.ToLower()) || (countryId > 0 && cust.CountryId == countryId))

                          select new StatelistViewModel
                          {
                              Id = cust.Id,
                              Name = cust.Name,
                              StateCode = cust.StateCode,
                              CreatedDatetime = cust.CreatedDatetime,
                              CreatedBy = cust.CreatedBy,
                              CountryId = cust.CountryId,
                              UpdatedDatetime = cust.UpdatedDatetime,
                              UpdatedBy = cust.UpdatedBy,
                              IsDeleted = cust.IsDeleted,

                          }).ToListAsync();
        }
    }
}
