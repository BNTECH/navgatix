using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class NgxTableColumn
{
    public Guid ID { get; set; }

    public string UserEmail { get; set; }

    public Guid UserID { get; set; }

    public string ScreenName { get; set; }

    public string Columns { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }
}
