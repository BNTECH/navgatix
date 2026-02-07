using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class ContactU
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public string PhoneNumber { get; set; }

    public string EmailId { get; set; }

    public string Description { get; set; }

    public bool? IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDatetime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDatetime { get; set; }
}
