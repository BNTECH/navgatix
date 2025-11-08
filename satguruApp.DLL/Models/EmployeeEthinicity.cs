using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class EmployeeEthinicity
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public int? CTEthinicityId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreaatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }
}
