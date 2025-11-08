using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Aspnet_Applications
{
    public string ApplicationName { get; set; }

    public string LoweredApplicationName { get; set; }

    public Guid ApplicationId { get; set; }

    public string Description { get; set; }

    //public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();

    //public virtual ICollection<Aspnet_Membership> Aspnet_Membership { get; set; } = new List<Aspnet_Membership>();

    //public virtual ICollection<Aspnet_Paths> Aspnet_Paths { get; set; } = new List<Aspnet_Paths>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
