using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class FAPayCategory
{
    public Guid ID { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public bool IsActive { get; set; }

    public int? CTPayTypeID { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public int? DivisionID { get; set; }

    public virtual Company Division { get; set; }

    public virtual ICollection<FAPayCategoryDetail> FAPayCategoryDetails { get; set; } = new List<FAPayCategoryDetail>();
}
