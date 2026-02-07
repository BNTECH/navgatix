using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class NavPosition
{
    public int NavPositionId { get; set; }

    public int NavParentId { get; set; }

    public int PositionId { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public bool IsDeleted { get; set; }
}
