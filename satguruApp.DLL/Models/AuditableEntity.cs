using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.DLL.Models
{
    public class AuditableEntity : IAuditableEntity
    {
      public  int CreatedBy { get; set; }
      public  int LastModifyBy { get; set; }
      public  System.DateTime CreatedDT { get; set; }
        public System.DateTime? LastModifiedDT { get; set; }
    }
}
