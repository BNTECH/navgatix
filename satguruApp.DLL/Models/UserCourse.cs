using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class UserCourse
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public int? CourseId { get; set; }

    public int? SubjectId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Course Course { get; set; }

    public virtual Subject Subject { get; set; }
}
