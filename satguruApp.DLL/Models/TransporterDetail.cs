using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class TransporterDetail
{
    public long Id { get; set; }

    public string UserId { get; set; }

    public string CompanyName { get; set; }

    public string GSTNumber { get; set; }

    public string PANNumber { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Pincode { get; set; }

    public string BankAccountNumber { get; set; }

    public string IFSCCode { get; set; }

    public bool? ProfileVerified { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public virtual User User { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
