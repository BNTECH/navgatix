using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Aspnet_PersonalizationAllUsers
{
    public Guid PathId { get; set; }

    public byte[] PageSettings { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    public virtual Aspnet_Paths Path { get; set; }
}
