using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Gender
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; }
}
