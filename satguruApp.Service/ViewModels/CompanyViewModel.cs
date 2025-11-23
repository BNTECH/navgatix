using satguruApp.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Descr { get; set; }
        public int? CtindustryId { get; set; }
        public bool IsDeleted { get; set; }
        public int? ParentCompanyId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string PhoneCell { get; set; }
        public string PhoneCell2 { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneWork2 { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneHome2 { get; set; }
        public string PhoneMain { get; set; }
        public string PhoneOther { get; set; }
        public string PhoneOther2 { get; set; }
        public int? CttypeId { get; set; }
        public int? CtcategoryId { get; set; }
        public string Explain { get; set; }



        public string WebsiteUrl { get; set; }
        public string Tin { get; set; }
        public string PanNumber { get; set; }
        public Nullable<decimal> MaxFringeHoursInWeek { get; set; }
        public Nullable<bool> IsAllowHrsOneTaskPerDay { get; set; }
        public string Cinno { get; set; }
        public string StatusReportEmail { get; set; }
        public string StatusReportName { get; set; }
        public string AccountManagerEmailAddress { get; set; }
        public int? CtclientTypeId { get; set; }
        public string UEI { get; set; }
        public string EinfederalId { get; set; }
        public Address Address { get; set; }
        public Nullable<int> CTCategoryID { get; set; }
        public Nullable<int> CTIncorporationTypeID { get; set; }
        public Nullable<bool> IsFringeHoursAutoApproval { get; set; }
        public string Cage { get; set; }
        public Nullable<int> DUNS { get; set; }
        public Address RKUpateTo(Address dbModel)
        {
            dbModel.Address1 = Address.Address1;
            //dbModel.Unit = Address.Unit;
            dbModel.City_Id = Address.City_Id;
            dbModel.State_Id = Address.State_Id;
            dbModel.Zip_Code = Address.Zip_Code;
            dbModel.Latitude = Address.Latitude;
            dbModel.Longitude = Address.Longitude;
            return dbModel;
        }





        public Company RKMapTo(Company dbModel)
        {
            dbModel.ID = Id;
            dbModel.ParentCompanyID = ParentCompanyId;
            dbModel.Name = Name;
            dbModel.ShortName = ShortName;
            dbModel.CTIndustryID = CtindustryId;
            dbModel.Descr = Descr;
            dbModel.Email = Email;
            dbModel.AccountManagerEmailAddress = AccountManagerEmailAddress;
            dbModel.PhoneCell = PhoneCell;
            dbModel.PhoneCell2 = PhoneCell2;
            return dbModel;
        }
    }



    public class PayrollPayCycleList
    {
        public int id { get; set; }
        public string name { get; set; }
    }



    public class CompanyDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Descr { get; set; }
        public int? CtindustryId { get; set; }
        public bool IsDeleted { get; set; }
        public int? ParentCompanyId { get; set; }
        public string IncorporationName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string PhoneCell { get; set; }
        public string PhoneCell2 { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneWork2 { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneHome2 { get; set; }
        public string PhoneMain { get; set; }
        public string PhoneOther { get; set; }
        public string PhoneOther2 { get; set; }
        public int? CttypeId { get; set; }
        public int? CtcategoryId { get; set; }
        public string WebsiteUrl { get; set; }
        public string Tin { get; set; }
        public string PanNumber { get; set; }
        public string Cinno { get; set; }
        public string StatusReportEmail { get; set; }
        public string StatusReportName { get; set; }
        public string AccountManagerEmailAddress { get; set; }
        public int? CtclientTypeId { get; set; }
        public string EinfederalId { get; set; }
        public Dictionary<int, string> CompanyCodes { get; set; }
        public Dictionary<int, string> PayrollCycles { get; set; }



        public Company RKUpateTo(Company dbModel)
        {
            dbModel.ID = Id;
            dbModel.ParentCompanyID = ParentCompanyId;
            dbModel.Name = Name;
            dbModel.ShortName = ShortName;
            dbModel.CTIndustryID = CtindustryId;
            dbModel.Descr = Descr;
            dbModel.Email = Email;
            dbModel.AccountManagerEmailAddress = AccountManagerEmailAddress;
            dbModel.PhoneCell = PhoneCell;
            dbModel.PhoneCell2 = PhoneCell2;
            return dbModel;
        }
        public class CompanyFilterViewModel
        {
            public int TotalCount { get; set; }
            public bool IsDeleted { get; set; }
            public string CompanyName { get; set; }
            public string ShortName { get; set; }
            public string ParentCompanyName { get; set; }
            public bool isClientTypeDivision { get; set; }
            public bool isClientTypeAgencyClient { get; set; }
            public bool isClientTypeVendorSubVendor { get; set; }



            // paging
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int Skip { get; set; }



            //sorting
            public string SortBy { get; set; }
            public string SortDir { get; set; }
            public string CompanyType { get; set; }
            //filters
            public string Filter { get; set; }
            public int[] NavIds { get; set; }
            public int Id { get; set; }
        }
    }


    public class CompanyListViewModel
    {
        public static Expression<Func<Company, Company, CommonType, CompanyListViewModel>> RKMapFrom = (company, parentCompany, commonType) => new CompanyListViewModel
        {
            Name = company.Name,
            ID = company.ID,
            ShortName = company.ShortName,
            ParentCompanyName = parentCompany.Name,
            Type = commonType.Name,
            Email = company.Email,
            PhoneCell = company.PhoneCell,
            ParentCompanyID = parentCompany.ID,
            CTClientTypeID = company.CTClientTypeID
        };



        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Descr { get; set; }
        public Nullable<int> CTIndustryID { get; set; }
        public bool IsDeleted { get; set; }
        public string Type { get; set; }
        public Nullable<int> ParentCompanyID { get; set; }
        public string ParentCompanyName { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDateTime { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string PhoneCell { get; set; }
        public string PhoneWork { get; set; }
        public string PhoneCell2 { get; set; }
        public string PhoneWork2 { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneHome2 { get; set; }
        public string Phone_Main { get; set; }
        public string PhoneOther { get; set; }
        public string PhoneOther2 { get; set; }
        public Nullable<int> CTTypeID { get; set; }
        public Nullable<int> CTCategoryID { get; set; }
        public string WebsiteUrl { get; set; }
        public string Error { get; set; }
        public string StatusReportName { get; set; }
        public string StatusReportEmail { get; set; }
        public Nullable<int> CTClientTypeID { get; set; }
    }
    public class CompanyEditViewModel
    {
        public static Expression<Func<Company, CommonType, CommonType, CompanyEditViewModel>> RKMapFrom = (company, ct, inc) => new CompanyEditViewModel
        {
            ID = company.ID,
            CTCategoryID = company.CTCategoryID,
            CTIndustryID = company.CTIndustryID,
            CTTypeID = company.CTTypeID,
            CurrencyID = company.CurrencyID,
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
            ClientTypeValue = ct.Name,
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
        };



        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Descr { get; set; }
        public Nullable<Decimal> NegativeCapHrs { get; set; }
        public Nullable<int> CTIndustryID { get; set; }
        public string IndustryName { get; set; }
        public bool IsDeleted { get; set; }
        public string IncorporationName { get; set; }
        public Nullable<decimal> MaxFringeHoursInWeek { get; set; }
        public Nullable<int> ParentCompanyID { get; set; }
        public string ParentCompanyName { get; set; }
        public Nullable<bool> IsAllowHrsOneTaskPerDay { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDateTime { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public int FringeHourDayLimit { get; set; }
        public bool IsApproverWeekendCommentRequired { get; set; }
        public bool IsEmployeeOTComment { get; set; }
        public string PhoneCell { get; set; }
        public string PhoneWork { get; set; }
        public string UEI { get; set; }
        public string Explain { get; set; }
        public string PhoneCell2 { get; set; }
        public string PhoneWork2 { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneHome2 { get; set; }
        public string Phone_Main { get; set; }
        public string PhoneOther { get; set; }
        public string PhoneOther2 { get; set; }
        public Nullable<bool> IsApproverOTComment { get; set; }
        public Nullable<bool> IsNWHoursComment { get; set; }
        public Nullable<bool> IsWeekendHourComment { get; set; }
        public Nullable<bool> IsHolidayWorkingComment { get; set; }
        public Nullable<bool> IsZeroTimesheetApprove { get; set; }
        public bool IsEmployeeCommentRequiredOnWorkingHolidayHourCode { get; set; }
        public Nullable<int> CTTypeID { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<bool> IsNotSubmitPrevTS { get; set; }
        public string NotSubmitPrevTS { get; set; }
        public Nullable<bool> IsMaxFringHours { get; set; }
        public string MaxFringHours { get; set; }
        public Nullable<int> CTCategoryID { get; set; }
        public Nullable<int> CTClientTypeID { get; set; }
        public Nullable<int> CTIncorporationTypeID { get; set; }
        public Nullable<int> CTInvoiceCycleID { get; set; }
        public Nullable<int> CTPaymentTermsID { get; set; }
        public Nullable<int> MailTemplateID { get; set; }
        public string WebsiteUrl { get; set; }
        public string EINFederalID { get; set; }
        public string Error { get; set; }
        public bool IsExist { get; set; }
        public string TIN { get; set; }
        public string PanNumber { get; set; }
        public string CINNo { get; set; }
        public string StatusReportName { get; set; }
        public string StatusReportEmail { get; set; }
        public string AccountManagerEmailAddress { get; set; }
        public string ClientTypeValue { get; set; }
        public List<PayrollPayCycleList> PayrollCycles { get; set; }
        public string PayrollCyclesName { get; set; }
        public string CompanyCodesName { get; set; }
        public List<PayrollPayCycleList> CompanyCodes { get; set; }
        public Nullable<bool> IsFringeHoursAutoApproval { get; set; }
        public string Cage { get; set; }
        public Nullable<int> DUNS { get; set; }
        public void RKMapTo(Company company)
        {
            company.CTCategoryID = CTCategoryID;
            company.CTIndustryID = CTIndustryID;
            company.CTTypeID = CTTypeID;
            company.Descr = Descr;
            company.CurrencyID = CurrencyID;
            company.Email = Email;
            company.Email2 = Email2;
            company.Name = Name;
            company.ParentCompanyID = ParentCompanyID;
            company.PhoneCell = PhoneCell;
            company.PhoneCell2 = PhoneCell2;
            company.PhoneHome = PhoneHome;
            company.PhoneHome2 = PhoneHome2;
            company.PhoneOther = PhoneOther;
            company.PhoneOther2 = PhoneOther2;
            company.PhoneWork = PhoneWork;
            company.PhoneWork2 = PhoneWork2;
            company.Phone_Main = Phone_Main;
            company.ShortName = ShortName;
            company.WebsiteUrl = WebsiteUrl;
            company.TIN = TIN;
            company.FringeHourDayLimit = FringeHourDayLimit;
            company.IsApproverWeekendCommentRequired = IsApproverWeekendCommentRequired;
            company.IsEmployeeOTComment = IsEmployeeOTComment;
            company.PanNumber = PanNumber;
            company.CINNo = CINNo;
            company.StatusReportName = StatusReportName;
            company.StatusReportEmail = StatusReportEmail;
            company.AccountManagerEmailAddress = AccountManagerEmailAddress;
            company.EINFederalID = EINFederalID;
            company.CTClientTypeID = CTClientTypeID;
            company.CTIncorporationTypeID = CTIncorporationTypeID;
            company.CTInvoiceCycleID = CTInvoiceCycleID;
            company.CTPaymentTermsID = CTPaymentTermsID;
            company.IsZeroTimesheetApprove = IsZeroTimesheetApprove;
            company.IsNWHoursComment = IsNWHoursComment;
            company.IsHolidayWorkingComment = IsHolidayWorkingComment;
            company.IsWeekendHourComment = IsWeekendHourComment;
            company.IsApproverOTComment = IsApproverOTComment;
            company.MailTemplateID = MailTemplateID;
            company.IsNotSubmitPrevTS = IsNotSubmitPrevTS;
            company.IsFringeHoursAutoApproval = IsFringeHoursAutoApproval;
            company.IsEmployeeCommentRequiredOnWorkingHolidayHourCode = IsEmployeeCommentRequiredOnWorkingHolidayHourCode;
            company.Explain = Explain;
            company.Cage = Cage;
            company.DUNS = DUNS;
            company.UEI = UEI;
            company.MaxHoursInWeek = MaxFringeHoursInWeek;
            company.IsAllowHrsOneTaskPerDay = IsAllowHrsOneTaskPerDay;
        }
    }

}
