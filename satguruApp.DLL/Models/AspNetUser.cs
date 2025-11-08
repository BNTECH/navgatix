using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class AspNetUser
{
    public Guid ApplicationId { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; }

    public string LoweredUserName { get; set; }

    public string MobileAlias { get; set; }

    public bool IsAnonymous { get; set; }

    public DateTime LastActivityDate { get; set; }

  //  public virtual Aspnet_Applications Application { get; set; }

   // public virtual Aspnet_Membership Aspnet_Membership { get; set; }

   // public virtual ICollection<Aspnet_PersonalizationPerUser> Aspnet_PersonalizationPerUser { get; set; } = new List<Aspnet_PersonalizationPerUser>();

    //public virtual Aspnet_Profile Aspnet_Profile { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
