using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class JobSkill
{
    public int JobId { get; set; }

    public int SkillId { get; set; }

    public int Id { get; set; }

    public virtual Skill Skill { get; set; }
}
