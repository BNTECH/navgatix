using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class PromoCode
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public int? DiscountPercent { get; set; }

    public decimal? MaxDiscountAmount { get; set; }

    public decimal? MinBookingAmount { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTill { get; set; }

    public int? MaxUsagePerUser { get; set; }

    public int? TotalUsageLimit { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }
}
