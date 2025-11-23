using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Aspnet_SchemaVersions
{
    public string Feature { get; set; }

    public string CompatibleSchemaVersion { get; set; }

    public bool IsCurrentVersion { get; set; }
}
