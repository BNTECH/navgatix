using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class CandicateQualification
{
    public int Id { get; set; }

    public int? CandidateId { get; set; }

    public int? QualificationId { get; set; }
}
