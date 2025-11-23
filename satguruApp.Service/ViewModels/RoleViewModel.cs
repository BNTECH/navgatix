using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NormalizedName { get; set; }
        public string Descriminator { get; set; }

    }
    public class RoleFilterViewModel
    {
        public string Filter { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalCount { get; set; }
    }
}
