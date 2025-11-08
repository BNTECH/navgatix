using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class UserRating
{
    public long Id { get; set; }

    public Guid? User_Id { get; set; }

    public Guid? Target_User_Id { get; set; }

    public long? Booking_Id { get; set; }

    public decimal? Score { get; set; }

    public string Comment { get; set; }

    public bool? IsDeleted { get; set; }
}
