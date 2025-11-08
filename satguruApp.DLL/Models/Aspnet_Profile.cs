using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Aspnet_Profile
{
    public Guid UserId { get; set; }

    public string PropertyNames { get; set; }

    public string PropertyValuesString { get; set; }

    public byte[] PropertyValuesBinary { get; set; }

    public DateTime LastUpdatedDate { get; set; }

    //public virtual AspNetUser User { get; set; }
}
