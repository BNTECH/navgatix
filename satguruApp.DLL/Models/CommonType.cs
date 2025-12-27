using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class CommonType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CTID { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDatetime { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public int? UpdatedBy { get; set; }

    public string? Keys { get; set; }

    public string? Code { get; set; }

    public bool? IsSystem { get; set; }

    public int? ValueInt { get; set; }

    public string? ValueStr { get; set; }

    public DateTime? ValueDT { get; set; }

    public string? ValueDesc { get; set; }

    public string? Source { get; set; }

    public int? OrderBy { get; set; }

    public virtual ICollection<Company> CompanyCTCategories { get; set; } = new List<Company>();

    public virtual ICollection<Company> CompanyCTClientTypes { get; set; } = new List<Company>();

    public virtual ICollection<Company> CompanyCTIncorporationTypes { get; set; } = new List<Company>();

    public virtual ICollection<Company> CompanyCTIndustries { get; set; } = new List<Company>();

    public virtual ICollection<Company> CompanyCTTypes { get; set; } = new List<Company>();
}
