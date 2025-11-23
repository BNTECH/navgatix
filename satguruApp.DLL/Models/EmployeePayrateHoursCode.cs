using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class EmployeePayrateHoursCode
{
    public int Id { get; set; }

    public int? EmployeePayrateId { get; set; }

    public int? HoursCodeId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }
}
