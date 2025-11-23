using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Candidate
{
    public long Id { get; set; }

    public long? CandidateNumber { get; set; }

    public string Salutation { get; set; }

    public string FisrtName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string EmailId { get; set; }

    public string EmailId2 { get; set; }

    public string Mobile { get; set; }

    public string AlternateNumber { get; set; }

    public string PhoneNumber { get; set; }

    public decimal? Experience { get; set; }

    public string UserId { get; set; }

    public string Skills { get; set; }

    public int? CTResumeSourceId { get; set; }

    public string ResumePath { get; set; }

    public string OptedOut { get; set; }

    public DateTime? OptedOutDate { get; set; }

    public int? CTCandidateStatusId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }

    public string ResumeTitle { get; set; }

    public string CurrentLocation { get; set; }

    public string PreferredLocation { get; set; }

    public string CurrentDesignation { get; set; }

    public string CurrentEmployer { get; set; }

    public int? CompanyId { get; set; }

    public string UGCourse { get; set; }

    public string PGCourse { get; set; }

    public bool? IsSendIntroEmail { get; set; }

    public bool? IsApplied { get; set; }

    public int? CTGenderId { get; set; }

    public int? CTVeteranStatusId { get; set; }

    public bool? IsVeteranStatus { get; set; }

    public DateTime? StatusDatetime { get; set; }

    public int? CTFeedbackStatusId { get; set; }

    public string SalaryType { get; set; }

    public DateTime? ResumeUpdatedDatetime { get; set; }

    public int? DivisionId { get; set; }

    public int? TeamSize { get; set; }

    public int? CompanySize { get; set; }

    public bool? CheckW9Verfication { get; set; }

    public bool? CheckSecuritybackground { get; set; }

    public bool? CheckDoeRetireeStatus { get; set; }

    public bool? CheckFormerDoeStatus { get; set; }

    public bool? IsPAProcessed { get; set; }

    public DateTime? PAProcessedDate { get; set; }

    public bool? Disability { get; set; }

    public int? CTMaritalId { get; set; }

    public DateTime? Birthdate { get; set; }
}
