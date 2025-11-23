using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class AspNetRole
{
    public string Id { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public string Discriminator { get; set; }

    public string AspNetUserId { get; set; }

    public virtual ICollection<IdentityRoleClaim<string>> AspNetRoleClaims { get; set; } = new List<IdentityRoleClaim<string>>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
