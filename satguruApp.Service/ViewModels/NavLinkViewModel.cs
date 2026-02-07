using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.ViewModels
{
    public class NavLinkViewModel
    {
        public int NavLinkId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string LinkText { get; set; }
        public string IconClass { get; set; }
        public string IconUrl { get; set; }
        public int? LinkOrder { get; set; }
        public int? NavLinkParentId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string ParentName { get; set; }
        public string LinkJson { get; set; }
        public string Source { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
    public class NavTreeViewModel
    {
        public int NavTreeId { get; set; }
        public int NavParentId { get; set; }
        public int[] NavChildId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string Parentname { get; set; }
        public string Childname { get; set; }
        public int ChildId { get; set; }
        public string LinkJson { get; set; }
        public string Source { get; set; }
    }
    public class RoleNavigationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
    public class RoleNavigationMapping
    {
        public int NavLinkRoleId { get; set; }
        public int NavLinkId { get; set; }
        public int[] Navlinkmap { get; set; }
        public string Rolename { get; set; }
        public string LinkText { get; set; }
        public string Linkname { get; set; }
        public string ParentName { get; set; }



        public string RoleId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string Source { get; set; }
    }
}
