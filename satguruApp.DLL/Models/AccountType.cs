using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class AccountType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
