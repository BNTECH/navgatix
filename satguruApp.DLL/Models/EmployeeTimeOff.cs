using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class EmployeeTimeOff
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? TimeOffId { get; set; }

    public decimal? HoursEarnedTotal { get; set; }

    public DateTime? TimeOffStartdate { get; set; }

    public DateTime? TimeOffEndDate { get; set; }

    public decimal? NegativeCapHours { get; set; }

    public decimal? MaxUsageHours { get; set; }

    public int? PersonalActionId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }
}
