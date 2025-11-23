using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class Skill
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
}
