using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class EmployeePolicyClassfication
{
    public int Id { get; set; }

    public int? PolicyClassificationId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Comment { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }
}
