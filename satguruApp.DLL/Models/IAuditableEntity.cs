using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.DLL.Models
{
    public interface  IAuditableEntity
    {
        int CreatedBy { get; set; }
        int LastModifyBy { get; set; }
        System.DateTime CreatedDT { get; set; }
        System.DateTime? LastModifiedDT { get; set; }
    }
}
