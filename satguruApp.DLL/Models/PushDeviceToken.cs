using System;

namespace satguruApp.DLL.Models;

public partial class PushDeviceToken
{
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public string DeviceToken { get; set; }

    public string Platform { get; set; }

    public string DeviceId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastSeenAt { get; set; }

    public virtual User User { get; set; }
}
