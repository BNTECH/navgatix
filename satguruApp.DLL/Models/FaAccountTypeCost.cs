using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class FaAccountTypeCost
{
    public int Id { get; set; }

    public int? CTCompanyTypeId { get; set; }

    public int? CTEmployeeTypeId { get; set; }

    public int? FaHoursCodeId { get; set; }

    public int? RevFaAccountTypeId { get; set; }

    public int? CostFaAccountTypeId { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }
}
