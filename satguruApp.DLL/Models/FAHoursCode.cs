using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class FAHoursCode
{
    public int ID { get; set; }

    public string Name { get; set; }

    public string RateCode { get; set; }

    public int? CTPeriodID { get; set; }

    public int? FAAccountTypeID { get; set; }

    public int? CTPayrollCategoryID { get; set; }

    public int? CTHoursTypeID { get; set; }

    public int? RateHourCodeID { get; set; }

    public int OrderBy { get; set; }

    public bool IsAvailableBillRates { get; set; }

    public bool IsAvailablePayRates { get; set; }

    public bool NotesMandatoryLevel1Approval { get; set; }

    public bool ApplyTimeOffHrsToAccrual { get; set; }

    public bool EnableManualTimeOffAdjustment { get; set; }

    public bool NotAllowSubmitHrsFutureDates { get; set; }

    public bool NotConsiderOTPayRateForADPReport { get; set; }

    public bool AlwaysAvailableinTSOnBasisOfPayCategory { get; set; }

    public int? AvailableHrsForTimeOffType { get; set; }

    public string Label { get; set; }

    public decimal? MaxPayoutHrs { get; set; }

    public bool WorkingDaysNotesMandatory { get; set; }

    public bool WeekendDaysNotesMandatory { get; set; }

    public bool HolidaysNotesMandatory { get; set; }

    public string Description { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public int? DivisionID { get; set; }

    public string ColorCode { get; set; }

    public virtual Company Division { get; set; }

    public virtual ICollection<FAPayCategoryDetail> FAPayCategoryDetails { get; set; } = new List<FAPayCategoryDetail>();
}
