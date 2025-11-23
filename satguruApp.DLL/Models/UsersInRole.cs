using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class UsersInRole
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public int Id { get; set; }

    public virtual Role Role { get; set; }
}
