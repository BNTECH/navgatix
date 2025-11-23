using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class CustomerDetail
{
    public long Id { get; set; }

    public string? UserId { get; set; }

    public string? CompanyName { get; set; }

    public string? GSTNumber { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Pincode { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual User User { get; set; }
}
