using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Role
{
    public Guid ApplicationId { get; set; }

    public Guid RoleId { get; set; }

    public string RoleName { get; set; }

    public string LoweredRoleName { get; set; }

    public string Description { get; set; }

    public string AspNetUserId { get; set; }

    public virtual ICollection<UsersInRole> UsersInRoles { get; set; } = new List<UsersInRole>();
}
