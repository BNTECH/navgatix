using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Education
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public DateTime? UpdatedDatetime { get; set; }
}
