using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Complaint
{
    public long Id { get; set; }

    public long? Booking_Id { get; set; }

    public string Issue_Type { get; set; }

    public string Description { get; set; }

    public int? Status { get; set; }

    public string Resolution { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }
}
