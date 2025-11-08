using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class FAAccountType
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public int? CTTypeId { get; set; }

    public int? CTTaxId { get; set; }

    public string Description { get; set; }

    public bool? IsExpenseClaim { get; set; }

    public bool? IsDashboardWatch { get; set; }

    public bool? IsEnablePaymentToAccount { get; set; }

    public int? DivisionId { get; set; }

    public int? AccountTypeParenId { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetine { get; set; }
}
