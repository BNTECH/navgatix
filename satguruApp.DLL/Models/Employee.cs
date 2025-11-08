using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string Salutation { get; set; }

    public string Name { get; set; }

    public string FName { get; set; }

    public string LName { get; set; }

    public string MName { get; set; }

    public DateTime? BirthDate { get; set; }

    public string Email { get; set; }

    public string Email2 { get; set; }

    public string Mobile { get; set; }

    public string AlternateNumber { get; set; }

    public string PhoneHome { get; set; }

    public DateTime? JoiningDate { get; set; }

    public DateTime? LeavingDate { get; set; }

    public bool? IsDeleted { get; set; }

    public int? Version { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }

    public int? CompanyId { get; set; }

    public int? DivisionId { get; set; }

    public int? HRPayCycleId { get; set; }

    public decimal? EmpNumber { get; set; }

    public string EmpType { get; set; }

    public int? HRPositionId { get; set; }

    public string JobTitle { get; set; }

    public bool? IsExempt { get; set; }

    public bool? CanRehire { get; set; }

    public int? CTTerminationTypeId { get; set; }

    public int? CTMaritalStatusId { get; set; }

    public int? CTGenderId { get; set; }

    public int? CTEmployeeTypeId { get; set; }

    public int? SectionId { get; set; }

    public int? PayrollId { get; set; }

    public string TerminationComment { get; set; }

    public int? CTPreferenceEmailId { get; set; }

    public int? CTPreferencePhoneId { get; set; }

    public int? CTBestCommunicationId { get; set; }

    public bool? IsVeteranStatus { get; set; }

    public bool? IsDisability { get; set; }

    public string SuijunctionCode { get; set; }

    public string HomeDepartment { get; set; }

    public bool? IsOvertimeExempt { get; set; }

    public bool? IsCompTime { get; set; }

    public bool? IsClientDeliverable { get; set; }

    public bool? IsSundayAllowed { get; set; }

    public bool? IsShowHoursOnpayStub { get; set; }

    public int? CTFillingStatusId { get; set; }

    public string Exemptions { get; set; }

    public bool? IsMultipleJob { get; set; }

    public int? Dependants { get; set; }

    public int? IsLegalStatusId { get; set; }

    public string OtherIncome { get; set; }

    public bool? IsNonResidentAllien { get; set; }

    public decimal? AdditionalTaxAmount { get; set; }

    public bool? IsCalculateFederalIncomeTax { get; set; }

    public bool? IsCalculateFederalTaxable { get; set; }

    public bool? IsCanculateFutaTax { get; set; }

    public int? CTEthinicityId { get; set; }

    public int? ComapnyCodeDetailId { get; set; }

    public int? DepartmentId { get; set; }

    public decimal? TotalAllowances { get; set; }

    public decimal? AdditionWithHolding { get; set; }

    public decimal? StateWithHolding { get; set; }

    public string StateExemption { get; set; }

    public string OtherInfoReason { get; set; }

    public int? CTWorkCategoryId { get; set; }

    public bool? EnableNotification { get; set; }

    public bool? IsMilitarySpouse { get; set; }

    public int? CTBranchOfService { get; set; }

    public string DutyStation { get; set; }

    public int? CTSpouseMilitaryStatus { get; set; }

    public string BranchOfServiceIfDiffer { get; set; }

    public string MilitaryDependant { get; set; }

    public string SpouseContactInfo { get; set; }

    public bool? IsVerificationConsent { get; set; }

    public string SpouseContactPhoneInfo { get; set; }
}
