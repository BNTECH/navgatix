using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class HRPayPeriod
{
    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime? PayRollDate { get; set; }

    public DateTime? PayDate { get; set; }

    public bool IsPayRollStatus { get; set; }

    public DateTime? HRTimeOffCalcDate { get; set; }

    public int? HrsWorking { get; set; }

    public int? HrsHoliday { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public int ID { get; set; }

    public int HRPayCycleID { get; set; }
}
