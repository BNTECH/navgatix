using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Payment
{
    public Guid Id { get; set; }

    public Guid? BookingId { get; set; }

    public decimal? Amount { get; set; }

    public string PaymentMode { get; set; }

    public string PaymentStatus { get; set; }

    public string TransactionReference { get; set; }

    public DateTime? PaidAt { get; set; }

    public bool? IsDeleted { get; set; }
}
