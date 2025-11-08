using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Wallet
{
    public long Id { get; set; }

    public string UserId { get; set; }

    public decimal? Balance { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; }
}
