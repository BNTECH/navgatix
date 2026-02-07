using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class NavTreeAudit
{
    public int NavTreeAuditId { get; set; }

    public int NavTreeId { get; set; }

    public int NavParentId { get; set; }

    public int NavChildId { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public bool IsDeleted { get; set; }

    public int AuditedBy { get; set; }

    public DateTime? AuditedDateTime { get; set; }
}
