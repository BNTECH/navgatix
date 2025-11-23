using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class User
{
    public string Id { get; set; }

    public int AccessFailedCount { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string Email { get; set; }

    public int AppUserId { get; set; }

    public bool EmailConfirmed { get; set; }

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public string NormalizedEmail { get; set; }

    public string NormalizedUserName { get; set; }

    public string PasswordHash { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public string SecurityStamp { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime? DOB { get; set; }

    public string Discriminator { get; set; }

    public bool? IsActive { get; set; }

    public string Configuration { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public int? DefaultDivisionID { get; set; }

    public string FullName { get; set; }

    public bool? IsEnabled { get; set; }

    public string JobTitle { get; set; }

    public string LoginName { get; set; }

    public string OtpSecret { get; set; }

    public string Pin { get; set; }

    public string UserType { get; set; }

    public DateTime? LockoutEndDateUtc { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CustomerDetail> CustomerDetails { get; set; } = new List<CustomerDetail>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<TransporterDetail> TransporterDetails { get; set; } = new List<TransporterDetail>();

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}
