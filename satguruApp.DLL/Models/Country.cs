using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string CountryCodeThree { get; set; }

    public string CountryCodeTwo { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDatetime { get; set; }

    public string RegionName { get; set; }

    public string CurrencySymbols { get; set; }

    public bool? IsDeleted { get; set; }
}
