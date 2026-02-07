using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Company
{
    public int ID { get; set; }

    public string Name { get; set; }

    public string ShortName { get; set; }

    public string Descr { get; set; }

    public int? CTIndustryID { get; set; }

    public bool IsDeleted { get; set; }

    public int? ParentCompanyID { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public string Email { get; set; }

    public string Email2 { get; set; }

    public string PhoneCell { get; set; }

    public string PhoneCell2 { get; set; }

    public string PhoneWork { get; set; }

    public string PhoneWork2 { get; set; }

    public string PhoneHome { get; set; }

    public string PhoneHome2 { get; set; }

    public string Phone_Main { get; set; }

    public string PhoneOther { get; set; }

    public string PhoneOther2 { get; set; }

    public int? CurrencyID { get; set; }

    public int? CTTypeID { get; set; }

    public int? CTCategoryID { get; set; }

    public string WebsiteUrl { get; set; }

    public string TIN { get; set; }

    public string PanNumber { get; set; }

    public string CINNo { get; set; }

    public string StatusReportEmail { get; set; }

    public string StatusReportName { get; set; }

    public string AccountManagerEmailAddress { get; set; }

    public int? CTClientTypeID { get; set; }

    public string EINFederalID { get; set; }

    public int? CTIncorporationTypeID { get; set; }

    public int? CTInvoiceCycleID { get; set; }

    public int? CTPaymentTermsID { get; set; }

    public int? MailTemplateID { get; set; }

    public int? MSIID { get; set; }

    public bool? IsApproverOTComment { get; set; }

    public bool? IsNWHoursComment { get; set; }

    public bool? IsWeekendHourComment { get; set; }

    public bool? IsHolidayWorkingComment { get; set; }

    public bool? IsZeroTimesheetApprove { get; set; }

    public bool? IsNotSubmitPrevTS { get; set; }

    public bool? IsFringeHoursAutoApproval { get; set; }

    public int FringeHourDayLimit { get; set; }

    public bool IsApproverWeekendCommentRequired { get; set; }

    public bool IsEmployeeOTComment { get; set; }

    public bool IsEmployeeCommentRequiredOnWorkingHolidayHourCode { get; set; }

    public string Explain { get; set; }

    public string Cage { get; set; }

    public int? DUNS { get; set; }

    public string UEI { get; set; }

    public decimal? MaxHoursInWeek { get; set; }

    public bool? IsAllowHrsOneTaskPerDay { get; set; }

    public string AccountingName { get; set; }

    public virtual CommonType CTCategory { get; set; }

    public virtual CommonType CTClientType { get; set; }

    public virtual CommonType CTIncorporationType { get; set; }

    public virtual CommonType CTIndustry { get; set; }

    public virtual CommonType CTType { get; set; }

    public virtual ICollection<FAHoursCode> FAHoursCodes { get; set; } = new List<FAHoursCode>();

    public virtual ICollection<FAPayCategory> FAPayCategories { get; set; } = new List<FAPayCategory>();

    public virtual ICollection<Company> InverseParentCompany { get; set; } = new List<Company>();

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual Company ParentCompany { get; set; }
}
