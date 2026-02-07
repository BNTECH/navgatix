using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class HRPayCycle
{
    public string Title { get; set; }

    public int? PayCycleTypeCT { get; set; }

    public int? PayRollDayAfter { get; set; }

    public int? PayDayAfter { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public decimal? TotalPayPeriods { get; set; }

    public string ExcludedDays { get; set; }

    public decimal? HoursPerPayPeriod { get; set; }

    public int? CTWeekdaysID { get; set; }

    public int ID { get; set; }
}
