using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using Newtonsoft;
using System.Threading;
namespace satguruApp.Service.Services
{
    public class SystemConfigurationService : Repository<SystemConfiguration>, ISystemConfigurationService
    {
        public SystemConfigurationService(SatguruDBContext context) : base(context)
        { }
        private SatguruDBContext _db => (SatguruDBContext)_context;
        public async Task<SystemConfigurationViewModel> GetById(int? id, string name = "")
        {
            return await (from conig in _db.SystemConfigurations
                          where conig.Id == id && conig.IsDeleted == false
                          select new SystemConfigurationViewModel
                          {
                              Id = conig.Id,
                              Field = conig.Field,
                              Value = conig.Value,
                              IsDeleted = conig.IsDeleted,
                              FieldText = conig.FieldText,
                              CTTableId = conig.CTTableId,
                              IsJson = conig.IsJson,

                          }).FirstOrDefaultAsync();
        }
        public async Task<int> SaveChangeAsync(SystemConfigurationViewModel model)
        {
            var data = await _db.SystemConfigurations.Where(x => x.Field.ToLower() == model.Field.ToLower() || x.FieldText.ToLower() == model.FieldText.ToLower()).FirstOrDefaultAsync();
            if (data != null && data.Id != model.Id)
            {
                return 0;
            }
            else if (data != null && data.Id == model.Id)
            {
                data.Value = model.Value;
                data.Field = model.Field;
                data.FieldText = model.FieldText;
                data.CTTableId = model.CTTableId;
                data.IsJson = model.IsJson;
            }
            else
            {
                data = new SystemConfiguration();
                model.ModelMapTo(data);
                _db.SystemConfigurations.Add(data);
            }
            return await _db.SaveChangesAsync();

        }
        public async Task<int> Delete(int id)
        {
            var data = await _db.SystemConfigurations.Where(x => x.Id == id).FirstOrDefaultAsync();
            data.IsDeleted = !data.IsDeleted;
            return await _db.SaveChangesAsync();
        }
        public async Task<List<SystemConfigurationViewModel>> GetAll(string name = "")
        {
            return await (from conig in _db.SystemConfigurations
                          where ((conig.Field.ToLower().Contains(name.ToLower()) || conig.FieldText.ToLower().Contains(name.ToLower())) || string.IsNullOrEmpty(name)) && conig.IsDeleted == false
                          select new SystemConfigurationViewModel
                          {
                              Id = conig.Id,
                              Field = conig.Field,
                              Value = conig.Value,
                              IsDeleted = conig.IsDeleted,
                              FieldText = conig.FieldText,
                              CTTableId = conig.CTTableId,
                              IsJson = conig.IsJson,

                          }).ToListAsync();
        }
        public async Task<SystemConfigurationViewModel> GetConfiguration(string name = "")
        {
            return await (from conig in _db.SystemConfigurations
                          where (conig.Field.ToLower()==name.ToLower() || conig.FieldText.ToLower()==name.ToLower()) && conig.IsDeleted == false
                          select new SystemConfigurationViewModel
                          {
                              Id = conig.Id,
                              Field = conig.Field,
                              Value = conig.Value,
                              IsDeleted = conig.IsDeleted,
                              FieldText = conig.FieldText,
                              CTTableId = conig.CTTableId,
                              IsJson = conig.IsJson,
                          }).FirstOrDefaultAsync();
        }
        public async Task<T> GetConfigurationDetails<T>(string keyField = "") {
            var config =  await(from congf in _db.SystemConfigurations where congf.IsDeleted == false && congf.FieldText.ToLower() == keyField.ToLower() select new { KeyName = congf.FieldText, Value = congf.Value }).FirstOrDefaultAsync();
            dynamic result;
            if (config != null && config.Value != null)
            {
                var isNumeric = 0;
                if (int.TryParse(config.Value, out isNumeric))
                    result = config.Value;
                else
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject(config.Value);
            }
            else
            { result = default(T); }
            return (T)result;
        } 

    }
}
