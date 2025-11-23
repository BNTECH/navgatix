using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class EmployeePayrate
{
    public int Id { get; set; }

    public int? PlacementInfoId { get; set; }

    public decimal? Amount { get; set; }

    public int? CTPeriodId { get; set; }

    public decimal? MaxCapHours { get; set; }

    public bool? IsPorated { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsReccuring { get; set; }

    public bool? IsBillable { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }
}
