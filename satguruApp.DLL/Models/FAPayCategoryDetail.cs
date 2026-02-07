using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class FAPayCategoryDetail
{
    public Guid ID { get; set; }

    public Guid FAPayCategoryID { get; set; }

    public int? CTParentTypeID { get; set; }

    public int? CTPayCategoryTypesID { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }

    public int? FAHourCodeID { get; set; }

    public int? CTPeriodID { get; set; }

    public virtual CommonType CTParentType { get; set; }

    public virtual CommonType CTPayCategoryTypes { get; set; }

    public virtual FAHoursCode FAHourCode { get; set; }

    public virtual FAPayCategory FAPayCategory { get; set; }
}
