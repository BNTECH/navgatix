using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class NavLinkAudit
{
    public int NavLinkAuditId { get; set; }

    public int NavLinkId { get; set; }

    public string Title { get; set; }

    public string Url { get; set; }

    public string LinkText { get; set; }

    public string IconClass { get; set; }

    public string IconUrl { get; set; }

    public int? LinkOrder { get; set; }

    public int? NavLinkParentId { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public bool IsDeleted { get; set; }

    public string LinkJson { get; set; }

    public string Source { get; set; }

    public int? AuditedBy { get; set; }

    public DateTime? AuditedDateTime { get; set; }
}
