using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class AspNetUserClaim
{
    public int Id { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public string UserId { get; set; }

    public string AspNetUserId { get; set; }

    public virtual User User { get; set; }
}
