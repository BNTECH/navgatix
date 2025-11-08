using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<CourseSubject> CourseSubjects { get; set; } = new List<CourseSubject>();

    public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}
