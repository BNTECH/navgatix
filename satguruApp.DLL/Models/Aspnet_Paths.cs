using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Aspnet_Paths
{
    public Guid ApplicationId { get; set; }

    public Guid PathId { get; set; }

    public string Path { get; set; }

    public string LoweredPath { get; set; }

    //public virtual Aspnet_Applications Application { get; set; }

  //public virtual Aspnet_PersonalizationAllUsers Aspnet_PersonalizationAllUsers { get; set; }

    //public virtual ICollection<Aspnet_PersonalizationPerUser> Aspnet_PersonalizationPerUser { get; set; } = new List<Aspnet_PersonalizationPerUser>();
}
