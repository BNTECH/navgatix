using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class CommonTypeWithKeyViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Keys { get; set; }
        public string? Code { get; set; }
        public string? ValueStr { get; set; }
        public int? ValueInt { get; set; }
        public string? ValueDesc { get; set; }
        public string? CodeValue { get; set; }
        public int?   OrderBy { get; set; }
        public bool? IsDisabled { get; set; }
    }
}
