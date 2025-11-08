using Microsoft.EntityFrameworkCore;
using satguruApp.DLL.Models;
using satguruApp.Service.Services.Interfaces;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static satguruApp.Service.ViewModels.CompanyDetailViewModel;

namespace satguruApp.Service.Services
{
    public class CompanyService : Repository<Company>, ICompanyService
    {

        private readonly IAddressService _address;
        //private readonly IHDGroupService _hdGroupService;



        public CompanyService(SatguruDBContext context, IAddressService address) : base(context)
        {
            _address = address;
        }
        private SatguruDBContext _db => (SatguruDBContext)_context;



        public async Task<object> GetCompanyList(CompanyFilterViewModel filter)
        {
            int CttypeId = 0;
            if (filter.CompanyType != null && filter.CompanyType.ToLower() == "police")
            {
                CttypeId = await _db.CommonTypes.Where(x => x.Keys == "PL" && x.Name.ToLower() == "police").Select(x => x.Id).FirstOrDefaultAsync();
            }
            else if (filter.CompanyType != null && filter.CompanyType.ToLower() == "lab") // for Labs list
            {
                CttypeId = await _db.CommonTypes.Where(x => x.Keys == "LB" && x.Name.ToLower() == "lab").Select(x => x.Id).FirstOrDefaultAsync();
            }
            else
            {
                CttypeId = 0;



            }
            var type = await _db.CommonTypes.Where(x => x.Code == "CTYP").Select(x => x.Id).FirstOrDefaultAsync();
            var results = (from dbModel in _db.Companies
                           join addr in _db.Addresses.Where(a => !a.Is_Deleted.GetValueOrDefault() && a.Ct_Table_Id == type) on dbModel.ID equals addr.Table_Row_Id into temp
                           from addr in temp.DefaultIfEmpty()
                           select new CompanyViewModel
                           {
                               Name = dbModel.Name,
                               Descr = dbModel.Descr,
                               Id = dbModel.ID,
                               ShortName = dbModel.ShortName,
                               Email = dbModel.Email,
                               Email2 = dbModel.Email2,
                               PhoneCell = dbModel.PhoneCell,
                               PhoneCell2 = dbModel.PhoneCell2,
                               WebsiteUrl = dbModel.WebsiteUrl,
                               CttypeId = dbModel.CTTypeID,
                               Address = addr
                           });



            if (!string.IsNullOrEmpty(filter.Filter))
            {
                results = results.Where(x => x.Name.Contains(filter.Filter));
            }
            if (CttypeId != 0)
            {
                results = results.Where(x => x.CttypeId == CttypeId);
            }
            var count = results.Count();
            var rkComps = await results.Skip(filter.Skip).Take(filter.PageSize).ToListAsync();
            var rtn = new { rkComps, count };
            return rtn;
        }





        /// <summary>
               /// Add Company with Address
               /// </summary>
               /// <param name="model"></param>
               /// <returns></returns>
        public async Task<CompanyViewModel> AddCompany(CompanyViewModel VMModel)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (VMModel.Id == 0)
                {
                    var check = await _db.Companies.Where(x => !x.IsDeleted && x.Name.ToLower().Trim() == VMModel.Name.ToLower().Trim() &&
                    x.CTTypeID == VMModel.CttypeId).FirstOrDefaultAsync();
                    if (check == null)
                    {
                        var type = await _db.CommonTypes.Where(x => x.Code == "CTYP").Select(x => x.Id).FirstOrDefaultAsync();
                        Company _company = new Company();
                        VMModel.RKMapTo(_company);
                        _company.IsDeleted = false;
                        _company.CTTypeID = type;
                        _db.Companies.Add(_company);
                        _db.SaveChanges();
                        VMModel.Id = _company.ID;
                        if (VMModel.Address != null)
                        {
                            Address addr = new Address();
                            addr = VMModel.Address;
                            ////Lat Long 
                            string StateName = await _db.States.Where(x => x.Id == addr.State_Id).Select(x => x.Name).FirstOrDefaultAsync();
                            string address = addr.Address1 + addr.Zip_Code + addr.City_Id + StateName;
                            var res = _address.GetdtLatLong(address);



                            //Address Insert 
                            VMModel.RKUpateTo(addr);
                            addr.Table_Row_Id = VMModel.Id;
                            addr.Ct_Table_Id = type;
                            _db.Addresses.Add(addr);
                            await _db.SaveChangesAsync();
                        }
                        scope.Complete();
                    }
                }
                else if (VMModel.Id != 0)
                {
                    var dbModel = await _db.Companies.Where(x => x.ID == VMModel.Id).FirstOrDefaultAsync();
                    VMModel.RKMapTo(dbModel);
                    if (dbModel != null)
                    {
                        VMModel.RKMapTo(dbModel);
                        if (VMModel.Address != null)
                        {
                            Address newAddr = new Address();
                            newAddr = VMModel.Address;
                            var type = await _db.CommonTypes.Where(x => x.Code == "CTYP").Select(x => x.Id).FirstOrDefaultAsync();
                            string StateName = await _db.States.Where(x => x.Id == newAddr.State_Id).Select(x => x.Name).FirstOrDefaultAsync();
                            string address = newAddr.Address1 + newAddr.Zip_Code + newAddr.City_Id + StateName;
                            var res = _address.GetdtLatLong(address);
                            await _db.SaveChangesAsync();
                            var recAdd = await _db.Addresses.Where(x => x.Table_Row_Id == VMModel.Id && x.Ct_Table_Id == type && !x.Is_Deleted.GetValueOrDefault()).FirstOrDefaultAsync();
                            if (recAdd != null)
                            {
                                VMModel.RKUpateTo(recAdd);
                                recAdd.Ct_Table_Id = type;
                                recAdd.Updated_Datetime = DateTime.Now;
                                await _db.SaveChangesAsync();
                            }



                        }
                        scope.Complete();
                    }
                }
                else
                {
                    throw new Exception("Company already exist");
                }



            }
            return VMModel;
        }



        public async Task<List<PayrollPayCycleList>> GetPayRollCycles(int id)
        {
            return await (from a in _db.DivisionPayrollCycles
                          join com in _db.CommonTypes on a.CTPayrollCycleId equals com.Id
                          where !a.IsDeleted.GetValueOrDefault() && a.DivisionId == id
                          select new PayrollPayCycleList { id = com.Id, name = com.Name }).OrderBy(x => x.name).ToListAsync();
        }
        public async Task<List<PayrollPayCycleList>> GetCompanyCodes(int id)
        {
            return await (from a in _db.CompanyCodeDetails
                          join com in _db.CompanyCodes on a.CompanyCodeId equals com.Id
                          where !a.IsDeleted.GetValueOrDefault() && a.TableRowId == id
                          select new PayrollPayCycleList { id = com.Id, name = com.Name }).OrderBy(x => x.name).ToListAsync();
        }
        public async Task<CompanyEditViewModel> GetCompanyDetailsById(int companyId)
        {
            var record = new CompanyEditViewModel();



            record = await (from company in _db.Companies
                            join d in _db.CommonTypes on company.CTClientTypeID equals d.Id
                            into temp
                            from d in temp.DefaultIfEmpty()
                            join inc in _db.CommonTypes on company.CTIncorporationTypeID equals inc.Id
                            into temp1
                            from inc in temp1.DefaultIfEmpty()
                            where company.ID == companyId && !company.IsDeleted
                            orderby company.ID descending
                            select new CompanyEditViewModel
                            {
                                ID = company.ID,
                                CTCategoryID = company.CTCategoryID,
                                CTIndustryID = company.CTIndustryID,
                                CurrencyID = company.CurrencyID,
                                CTTypeID = company.CTTypeID,
                                Descr = company.Descr,
                                Email = company.Email,
                                Email2 = company.Email2,
                                Name = company.Name,
                                ParentCompanyID = company.ParentCompanyID,
                                PhoneCell = company.PhoneCell,
                                PhoneCell2 = company.PhoneCell2,
                                PhoneHome = company.PhoneHome,
                                PhoneHome2 = company.PhoneHome2,
                                PhoneOther = company.PhoneOther,
                                PhoneOther2 = company.PhoneOther2,
                                PhoneWork = company.PhoneWork,
                                PhoneWork2 = company.PhoneWork2,
                                Phone_Main = company.Phone_Main,
                                ShortName = company.ShortName,
                                WebsiteUrl = company.WebsiteUrl,
                                TIN = company.TIN,
                                PanNumber = company.PanNumber,
                                CINNo = company.CINNo,
                                StatusReportName = company.StatusReportName,
                                StatusReportEmail = company.StatusReportEmail,
                                AccountManagerEmailAddress = company.AccountManagerEmailAddress,
                                EINFederalID = company.EINFederalID,
                                CTClientTypeID = company.CTClientTypeID,
                                CTIncorporationTypeID = company.CTIncorporationTypeID,
                                ClientTypeValue = d.Name,
                                IncorporationName = inc.Name,
                                CTInvoiceCycleID = company.CTInvoiceCycleID,
                                CTPaymentTermsID = company.CTPaymentTermsID,
                                MailTemplateID = company.MailTemplateID,
                                IsApproverOTComment = company.IsApproverOTComment,
                                IsHolidayWorkingComment = company.IsHolidayWorkingComment,
                                IsNWHoursComment = company.IsNWHoursComment,
                                IsWeekendHourComment = company.IsWeekendHourComment,
                                IsZeroTimesheetApprove = company.IsZeroTimesheetApprove,
                                IsNotSubmitPrevTS = company.IsNotSubmitPrevTS,
                                IsApproverWeekendCommentRequired = company.IsApproverWeekendCommentRequired,
                                IsEmployeeOTComment = company.IsEmployeeOTComment,
                                FringeHourDayLimit = company.FringeHourDayLimit,
                                IsEmployeeCommentRequiredOnWorkingHolidayHourCode = company.IsEmployeeCommentRequiredOnWorkingHolidayHourCode,
                                IsFringeHoursAutoApproval = company.IsFringeHoursAutoApproval,
                                Explain = company.Explain,
                                Cage = company.Cage,
                                DUNS = company.DUNS,
                                UEI = company.UEI,
                                MaxFringeHoursInWeek = company.MaxHoursInWeek,
                                IsAllowHrsOneTaskPerDay = company.IsAllowHrsOneTaskPerDay
                            }).FirstOrDefaultAsync();
            record.PayrollCycles = await GetPayRollCycles(record.ID);
            record.CompanyCodes = await GetCompanyCodes(record.ID);
            record.PayrollCyclesName = string.Join(", ", (from a in _db.DivisionPayrollCycles
                                                          join b in _db.CommonTypes on a.CTPayrollCycleId equals b.Id
                                                          where a.DivisionId == record.ID && !a.IsDeleted.GetValueOrDefault()
                                                          select b.Name));
            record.CompanyCodesName = string.Join(", ", (from a in _db.CompanyCodeDetails
                                                         join b in _db.CompanyCodes on a.CompanyCodeId equals b.Id
                                                         where a.TableRowId == record.ID && !a.IsDeleted.GetValueOrDefault()
                                                         select b.Name));
            return record;
        }



        /// <summary>
        /// Update Company with Address
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<CompanyViewModel> UpdCompany(CompanyViewModel model)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var dbModel = await _db.Companies.Where(x => x.ID == model.Id).FirstOrDefaultAsync();
                    model.RKMapTo(dbModel);
                    if (dbModel != null)
                    {
                        model.RKMapTo(dbModel);
                        Address newAddr = new Address();
                        newAddr = model.Address;
                        var type = await _db.CommonTypes.Where(x => x.Code == "CTYP").Select(x => x.Id).FirstOrDefaultAsync();
                        string StateName = await _db.States.Where(x => x.Id == newAddr.State_Id).Select(x => x.Name).FirstOrDefaultAsync();
                        string address = newAddr.Address1 + newAddr.Zip_Code + newAddr.City_Id + StateName;
                        var res = _address.GetdtLatLong(address);
                        await _db.SaveChangesAsync();
                        var recAdd = await _db.Addresses.Where(x => x.Table_Row_Id == model.Id && x.Ct_Table_Id == type && !x.Is_Deleted.GetValueOrDefault()).FirstOrDefaultAsync();
                        if (recAdd != null)
                        {
                            model.RKUpateTo(recAdd);
                            recAdd.Updated_By = AppUserId;
                            recAdd.Updated_Datetime = DateTime.Now;
                            recAdd.Latitude = Convert.ToDouble(res.Rows[0]["lat"]);
                            recAdd.Longitude = Convert.ToDouble(res.Rows[0]["lng"]);
                            await _db.SaveChangesAsync();
                        }
                        scope.Complete();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return model;
        }



        public async Task<CompanyViewModel> GetCompany(int id)
        {



            var type = await _db.CommonTypes.Where(x => x.Code == "CTYP").Select(x => x.Id).FirstOrDefaultAsync();
            return await (from dbModel in _db.Companies.Where(b => b.ID == id)
                          join addr in _db.Addresses.Where(a => !a.Is_Deleted.GetValueOrDefault() && a.Ct_Table_Id == type) on dbModel.ID equals addr.Table_Row_Id into temp
                          from addr in temp.DefaultIfEmpty()
                          select new CompanyViewModel
                          {
                              Name = dbModel.Name,
                              Descr = dbModel.Descr,
                              Id = dbModel.ID,
                              ShortName = dbModel.ShortName,
                              Email = dbModel.Email,
                              Email2 = dbModel.Email2,
                              PhoneCell = dbModel.PhoneCell,
                              PhoneCell2 = dbModel.PhoneCell2,
                              WebsiteUrl = dbModel.WebsiteUrl,
                              CttypeId = dbModel.CTTypeID,
                              Address = addr
                          }).SingleOrDefaultAsync();
        }
        public async Task<CompanyViewModel> GetCompanyDetail(int id)
        {
            return await (from dbModel in _db.Companies
                          where dbModel.ID == id
                          select new CompanyViewModel
                          {
                              Id = dbModel.ID,
                              Name = dbModel.Name,
                              Descr = dbModel.Descr,
                              Email = dbModel.Email,
                              PhoneCell = dbModel.PhoneCell,
                              PhoneCell2 = dbModel.PhoneCell2,
                              PhoneMain = dbModel.Phone_Main,
                              ShortName = dbModel.ShortName,
                              CtindustryId = dbModel.CTIndustryID,
                              ParentCompanyId = dbModel.ParentCompanyID,
                              CtclientTypeId = dbModel.CTClientTypeID,
                              CttypeId = dbModel.CTTypeID,
                              StatusReportEmail = dbModel.StatusReportEmail,
                              StatusReportName = dbModel.StatusReportName,
                              EinfederalId = dbModel.EINFederalID,
                              PanNumber = dbModel.PanNumber
                          }).SingleOrDefaultAsync();
        }
        //public async Task<CompanyEditViewModel> SaveUpdateDivisionCompanyCodeDetailList(CompanyEditViewModel vmModel)
        //{
        //    var existingDivisionCompanyCode = await _db.CompanyCodeDetails.Where(x => !x.IsDeleted.GetValueOrDefault() && x.TableRowId == vmModel.ID).Select(x => x.CompanyCodeId.GetValueOrDefault()).ToListAsync();
        //    var newDCC = vmModel.CompanyCodes.Select(x => x.id).AsEnumerable().Except(existingDivisionCompanyCode).ToList();
        //    var deletedDCC = existingDivisionCompanyCode.Except(vmModel.CompanyCodes.Select(x => x.id)).ToList();



        //    if (deletedDCC.Count > 0)
        //    {
        //        foreach (var item in deletedDCC)
        //        {
        //            var rec = await _db.CompanyCodeDetails.Where(x => x.CompanyCodeId == item && x.TableRowId == vmModel.ID && !x.IsDeleted.GetValueOrDefault()).FirstOrDefaultAsync();
        //            rec.IsDeleted = true;
        //            rec.UpdatedBy = AppUserId;
        //            rec.UpdatedDatetime = DateTime.Now;
        //            await _db.SaveChangesAsync();
        //        }
        //    }
        //    foreach (var item in newDCC)
        //    {
        //        var model = new CompanyCodeDetail();
        //        model.TableRowId = vmModel.ID;
        //        model.CompanyCodeId = item;
        //        model.IsDeleted = false;
        //        model.CreatedBy = AppUserId;
        //        model.CreatedDatetime = DateTime.Now;
        //        _db.CompanyCodeDetails.Add(model);
        //        await _db.SaveChangesAsync();
        //    }



        //    return vmModel;
        //}



        //public async Task<CompanyEditViewModel> SaveUpdateDivisionPayCycleDetailList(CompanyEditViewModel vmModel)
        //{
        //    var existingDivisionPayCycle = await _db.DivisionPayrollCycles.Where(x => !x.IsDeleted.GetValueOrDefault() && x.DivisionId == vmModel.ID).Select(x => x.CTPayrollCycleId.GetValueOrDefault()).ToListAsync();
        //    var newDP = vmModel.PayrollCycles.Select(x => x.id).AsEnumerable().Except(existingDivisionPayCycle).ToList();
        //    var deletedDP = existingDivisionPayCycle.Except(vmModel.PayrollCycles.Select(x => x.id)).ToList();



        //    if (deletedDP.Count > 0)
        //    {
        //        foreach (var item in deletedDP)
        //        {
        //            var rec = await _db.DivisionPayrollCycles.Where(x => x.CTPayrollCycleId == item && x.DivisionId == vmModel.ID && !x.IsDeleted.GetValueOrDefault()).FirstOrDefaultAsync();
        //            rec.IsDeleted = true;
        //            rec.UpdatedBy = AppUserId;
        //            rec.UpdatedDatetime = DateTime.Now;
        //            await _db.SaveChangesAsync();
        //        }
        //    }
        //    foreach (var item in newDP)
        //    {
        //        var model = new DivisionPayrollCycle();
        //        model.DivisionId = vmModel.ID;
        //        model.CTPayrollCycleId = item;
        //        model.IsDeleted = false;
        //        model.CreatedBy = AppUserId;
        //        model.CreatedDatetime = DateTime.Now;
        //        _db.DivisionPayrollCycles.Add(model);
        //        await _db.SaveChangesAsync();
        //    }
        //    return vmModel;
        //}

    }
}
