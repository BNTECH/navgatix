using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Aspnet_PersonalizationPerUser
{
    public Guid Id { get; set; }

    public Guid? PathId { get; set; }

    public Guid? UserId { get; set; }

    public byte[] PageSettings { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    //public virtual Aspnet_Paths Path { get; set; }

    //public virtual AspNetUser User { get; set; }
}
