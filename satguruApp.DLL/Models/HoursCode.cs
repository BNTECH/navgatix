using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class HoursCode
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string RateCode { get; set; }

    public int? CTPeriodId { get; set; }

    public int? AccountTypeId { get; set; }

    public int? CTPayrollCategoryId { get; set; }

    public int? CTHoursCodeId { get; set; }

    public int? RateCodeId { get; set; }

    public int? OrderBy { get; set; }

    public bool? IsAvailableBillRates { get; set; }

    public bool? IsAvailablePayRates { get; set; }

    public bool? NotesMandatoryLevel1Approval { get; set; }

    public bool? ApplyTimeOffHrsToAccrual { get; set; }

    public bool? EnableManualTimeOffAdjustment { get; set; }

    public bool? NotAllowedSubmitHrsFutureDates { get; set; }

    public bool? NotConsiderITPayrateforADPReport { get; set; }

    public bool? AlwaysAvailableTSOnBasisOfPayCategory { get; set; }

    public int? AvailableHrsForTimeOffType { get; set; }

    public string Label { get; set; }

    public decimal? MaxPayoutHrs { get; set; }

    public bool? WorkingDaysNotesMandatory { get; set; }

    public bool? WeekendDaysNotesMandatory { get; set; }

    public string Description { get; set; }

    public string ColorCode { get; set; }

    public int? DivisionId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }
}
