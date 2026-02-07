using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class NavLinkRole
{
    public int NavLinkRoleId { get; set; }

    public int NavLinkId { get; set; }

    public string RoleId { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public bool IsDeleted { get; set; }
}
