using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Notification
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public string Title { get; set; }

    public string Message { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; }
}
