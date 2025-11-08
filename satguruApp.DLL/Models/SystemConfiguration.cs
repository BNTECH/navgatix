using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class SystemConfiguration
{
    public int Id { get; set; }

    public string Field { get; set; }

    public string Value { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedDatetime { get; set; }

    public DateTime? UpdateDatetime { get; set; }

    public string FieldText { get; set; }

    public int? CTTableId { get; set; }

    public bool? IsJson { get; set; }
}
