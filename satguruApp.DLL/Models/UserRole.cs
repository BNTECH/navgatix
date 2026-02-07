using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class UserRole
{
    public string UserId { get; set; }

    public string RoleId { get; set; }

    public string ApplicationRoleId { get; set; }

    public string Discriminator { get; set; }

    public virtual User User { get; set; }
}
