using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class BookingBenefitsLog
{
    public string PromoCode { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? CashbackAmount { get; set; }

    public bool? WalletCredited { get; set; }

    public bool? IsDeleted { get; set; }

    public long? BookingId { get; set; }

    public long Id { get; set; }
}
