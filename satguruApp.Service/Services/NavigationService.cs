using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using satguruApp.DLL.Models;
using satguruApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Services
{
    //public class NavigationService:Repository<NavLink>, INavigation
    //{
    //public NavigationService(SatguruDBContext context) : base(context)
    //    { }
    //    private SatguruDBContext _db => (SatguruDBContext)_context;



    //    public int AddNavLink(NavLinkViewModel nav)
    //    {
    //        nav.NavLinkParentId = nav.ParentName != null ? _db.NavLinks.Where(x => x.Title == nav.ParentName.ToLower().Trim() && !x.IsDeleted).Select(x => x.NavLinkId).FirstOrDefault() : nav.NavLinkParentId;



    //        NavLink com = new NavLink();
    //        com.Title = nav.Title;
    //        com.LinkText = nav.LinkText;
    //        com.LinkOrder = nav.LinkOrder;
    //        com.IconUrl = nav.IconUrl;
    //        com.IconClass = nav.IconClass;
    //        com.Url = nav.Url;
    //        com.CreatedBy = AppUserId;
    //        com.CreatedDateTime = DateTime.UtcNow;
    //        com.NavLinkParentId = nav.NavLinkParentId;
    //        com.LinkJson = nav.LinkJson;
    //        com.Source = "LCP";





    //        try
    //        {
    //            _db.NavLinks.Add(com);
    //            _db.SaveChanges();
    //        }



    //        catch (Exception)
    //        {
    //            return 0;
    //        }



    //        return 1;
    //    }



    //    public List<NavLinkViewModel> GetaParentNavLink(NavLinkViewModel vmModel)
    //    {
    //        List<NavLinkViewModel> obj = new List<NavLinkViewModel>();
    //        try
    //        {
    //            obj = (from com in _db.NavLinks
    //                   join c in _db.NavLinks on com.NavLinkParentId equals c.NavLinkId
    //                   into temp
    //                   from c in temp.DefaultIfEmpty()
    //                   where (vmModel.Source == null || com.Source == vmModel.Source || (vmModel.Source == "ERP" && com.Source == null))
    //                   && (!com.IsDeleted)
    //                   && (com.NavLinkParentId == null)
    //                   select new NavLinkViewModel
    //                   {
    //                       Title = com.Title,
    //                       Url = com.Url,
    //                       IconClass = com.IconClass,
    //                       IconUrl = com.IconUrl,
    //                       NavLinkId = com.NavLinkId,
    //                       NavLinkParentId = com.NavLinkParentId,
    //                       LinkOrder = com.LinkOrder,
    //                       LinkText = com.LinkText,
    //                       IsDeleted = com.IsDeleted,
    //                       LinkJson = com.LinkJson,
    //                       ParentName = c.Title,
    //                       Source = com.Source == null ? "ERP" : com.Source
    //                   }).OrderBy(x => x.Title).ToList();



    //            return obj;



    //        }



    //        catch (Exception)



    //        {
    //            throw;
    //        }



    //    }
    //    public List<NavLinkViewModel> GetallNavLinks(NavLinkViewModel vmModel)
    //    {
    //        int skipedRecord = (vmModel.Page - 1) * vmModel.PageSize;
    //        List<NavLinkViewModel> obj = new List<NavLinkViewModel>();
    //        try
    //        {
    //            obj = (from com in _db.NavLinks
    //                   join c in _db.NavLinks on com.NavLinkParentId equals c.NavLinkId
    //                   into temp
    //                   from c in temp.DefaultIfEmpty()
    //                   where (vmModel.Source == null || com.Source == vmModel.Source || (vmModel.Source == "ERP" && com.Source == null))
    //                   && (com.IsDeleted == vmModel.IsDeleted)
    //                   select new NavLinkViewModel
    //                   {
    //                       Title = com.Title,
    //                       Url = com.Url,
    //                       IconClass = com.IconClass,
    //                       IconUrl = com.IconUrl,
    //                       NavLinkId = com.NavLinkId,
    //                       NavLinkParentId = com.NavLinkParentId,
    //                       LinkOrder = com.LinkOrder,
    //                       LinkText = com.LinkText,
    //                       IsDeleted = com.IsDeleted,
    //                       LinkJson = com.LinkJson,
    //                       ParentName = c.Title,
    //                       Source = com.Source == null ? "ERP" : com.Source,
    //                       CreatedDateTime = com.CreatedDateTime
    //                   }).AsEnumerable().OrderByDescending(x => x.CreatedDateTime).ToList();
    //            if (!String.IsNullOrWhiteSpace(vmModel.Title))
    //            {
    //                obj = obj.Where(x => x.Title.Trim().ToLower().Contains(vmModel.Title.Trim().ToLower())).ToList();
    //            }
    //            if (!String.IsNullOrWhiteSpace(vmModel.ParentName))
    //            {
    //                obj = obj.Where(x => x.ParentName != null && x.ParentName.Trim().ToLower() == (vmModel.ParentName.Trim().ToLower())).ToList();
    //            }
    //            vmModel.TotalCount = obj.Count;
    //            return obj.Skip(skipedRecord).Take(vmModel.PageSize).OrderByDescending(x => x.NavLinkId).ToList();



    //        }



    //        catch (Exception)



    //        {
    //            throw;
    //        }



    //    }
    //    public List<NavLinkViewModel> GetAllParentNavLink(NavLinkViewModel vmModel)
    //    {
    //        List<NavLinkViewModel> obj = new List<NavLinkViewModel>();
    //        try
    //        {
    //            obj = (from com in _db.NavLinks
    //                   join c in _db.NavLinks on com.NavLinkParentId equals c.NavLinkId
    //                   into temp
    //                   from c in temp.DefaultIfEmpty()
    //                   where (vmModel.Source == null || com.Source == vmModel.Source || (vmModel.Source == "ERP" && com.Source == null))
    //                   && (com.IsDeleted == vmModel.IsDeleted)
    //                   select new NavLinkViewModel
    //                   {
    //                       Title = com.Title,
    //                       Url = com.Url,
    //                       IconClass = com.IconClass,
    //                       IconUrl = com.IconUrl,
    //                       NavLinkId = com.NavLinkId,
    //                       NavLinkParentId = com.NavLinkParentId,
    //                       LinkOrder = com.LinkOrder,
    //                       LinkText = com.LinkText,
    //                       IsDeleted = com.IsDeleted,
    //                       LinkJson = com.LinkJson,
    //                       ParentName = c.Title,
    //                       Source = com.Source == null ? "ERP" : com.Source
    //                   }).OrderBy(x => x.Title).ToList();
    //            if (!String.IsNullOrWhiteSpace(vmModel.Title))
    //            {
    //                obj = obj.Where(x => x.Title.Trim().ToLower().Contains(vmModel.Title.Trim().ToLower())).ToList();
    //            }
    //            if (!String.IsNullOrWhiteSpace(vmModel.ParentName))
    //            {
    //                obj = obj.Where(x => x.ParentName != null && x.ParentName.Trim().ToLower().Contains(vmModel.ParentName.Trim().ToLower())).ToList();
    //            }
    //            return obj.OrderByDescending(x => x.NavLinkId).ToList();
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }



    //    public int UpdateNavLink(NavLinkViewModel nav)
    //    {
    //        NavLink com = new NavLink();
    //        com.NavLinkId = nav.NavLinkId;
    //        try
    //        {
    //            var col1 = _db.NavLinks.Where(w => w.NavLinkId == com.NavLinkId);
    //            foreach (var item in col1)
    //            {
    //                item.Title = nav.Title;
    //                item.LinkOrder = nav.LinkOrder;
    //                item.Url = nav.Url;
    //                item.IconClass = nav.IconClass;
    //                item.IconUrl = nav.IconUrl;
    //                item.UpdatedBy = nav.UpdatedBy;
    //                item.UpdatedDateTime = DateTime.UtcNow;
    //                item.LinkText = nav.LinkText;
    //                item.IsDeleted = nav.IsDeleted;
    //                item.LinkJson = nav.LinkJson;
    //                item.NavLinkParentId = nav.NavLinkParentId;
    //            }



    //            _db.SaveChanges();
    //        }
    //        catch (Exception)
    //        {
    //            return 0;
    //        }



    //        return 1;
    //    }
    //    public List<NavTreeViewModel> GetallNavTree()
    //    {
    //        try



    //        {
    //            var record = (from com in _db.NavTrees
    //                          join d in _db.NavLinks on com.NavParentId equals d.NavLinkId into temp
    //                          from d in temp.DefaultIfEmpty()
    //                          join s in _db.NavLinks on com.NavChildId equals s.NavLinkId into temp1
    //                          from s in temp1.DefaultIfEmpty()
    //                          select new NavTreeViewModel
    //                          {
    //                              ChildId = com.NavChildId,
    //                              NavParentId = com.NavParentId,
    //                              NavTreeId = com.NavTreeId,
    //                              Childname = s.Title,
    //                              Parentname = d.Title,
    //                              IsDeleted = com.IsDeleted,
    //                              Source = d.Source
    //                          }).AsQueryable();
    //            return record.ToList();
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    public int AddNavTree(NavTreeViewModel nav)
    //    {



    //        try
    //        {
    //            if (nav != null)
    //            {



    //                foreach (var item in nav.NavChildId)
    //                {
    //                    var check = _db.NavTrees.Where(x => x.NavParentId == nav.NavParentId && !x.IsDeleted && x.NavChildId == item).FirstOrDefault();
    //                    if (check == null)
    //                    {
    //                        NavTree tree = new NavTree();
    //                        tree.IsDeleted = false;
    //                        tree.CreatedBy = AppUserId;
    //                        tree.CreatedDateTime = DateTime.UtcNow;
    //                        tree.NavParentId = nav.NavParentId;
    //                        tree.NavChildId = item;
    //                        _db.NavTrees.Add(tree);
    //                        _db.SaveChanges();
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            return 0;
    //        }
    //        return 1;
    //    }



    //    public int UpdateNavTree(NavTreeViewModel nav)
    //    {
    //        NavTree com = new NavTree();
    //        com.NavParentId = nav.NavParentId;
    //        try
    //        {
    //            if (nav.ChildId == 0)
    //            {
    //                List<int> ExitingSubMenu = _db.NavTrees.Where(x => x.NavParentId == nav.NavParentId && !x.IsDeleted).Select(x => x.NavChildId).ToList();
    //                List<int> AllSubMenu = nav.NavChildId.Select(x => x).ToList();



    //                var newSubmenu = AllSubMenu.Except(ExitingSubMenu).ToList();
    //                var deletedSubMenu = ExitingSubMenu.Except(AllSubMenu).ToList();
    //                if (newSubmenu.Count > 0)
    //                {
    //                    foreach (var yy in newSubmenu)
    //                    {
    //                        var check = _db.NavTrees.Where(x => x.NavParentId == nav.NavParentId && !x.IsDeleted && x.NavChildId == yy).FirstOrDefault();
    //                        if (check == null)
    //                        {
    //                            NavTree tree = new NavTree();
    //                            tree.IsDeleted = false;
    //                            tree.NavParentId = nav.NavParentId;
    //                            tree.NavChildId = yy;
    //                            tree.IsDeleted = nav.IsDeleted;
    //                            _db.NavTrees.Add(tree);
    //                            var col1 = _db.NavLinks.Where(w => w.NavLinkId == yy);
    //                            foreach (var item in col1)
    //                            {
    //                                item.NavLinkParentId = nav.NavParentId;
    //                            }
    //                            _db.SaveChanges();
    //                        }
    //                        else
    //                        {
    //                            var delrecord = _db.NavTrees.Where(x => x.NavParentId == nav.NavParentId && x.IsDeleted && x.NavChildId == yy).FirstOrDefault();
    //                            delrecord.IsDeleted = false;
    //                            _db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                if (deletedSubMenu.Count > 0)
    //                {
    //                    foreach (var delClient in deletedSubMenu)
    //                    {
    //                        var delrecord = _db.NavTrees.Where(x => x.NavParentId == nav.NavParentId && !x.IsDeleted && x.NavChildId == delClient).FirstOrDefault();
    //                        delrecord.IsDeleted = true;
    //                        _db.SaveChanges();
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                var delrecord = _db.NavTrees.Where(x => x.NavTreeId == nav.NavTreeId).FirstOrDefault();
    //                if (nav.IsDeleted)
    //                {
    //                    delrecord.IsDeleted = true;
    //                }
    //                else
    //                {
    //                    delrecord.IsDeleted = false;
    //                }
    //                _db.SaveChanges();
    //            }



    //        }
    //        catch (Exception)
    //        {
    //            return 0;
    //        }
    //        return 1;
    //    }



    //    public List<RoleNavigationModel> GetallNavRoles()
    //    {
    //        List<RoleNavigationModel> obj = new List<RoleNavigationModel>();
    //        try
    //        {
    //            obj = (from com in _db.Roles
    //                   select new RoleNavigationModel { Id = com.Id, Name = com.Name, NormalizedName = com.NormalizedName }).ToList();
    //            return obj;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }



    //    public IEnumerable<RoleNavigationMapping> GetallNavRolesMapping(RoleNavigationMapping vmModel)
    //    {
    //        try
    //        {
    //            var obj = (from com in _db.NavLinkRoles
    //                       join rol in _db.Roles on com.RoleId equals rol.Id
    //                       join lin in _db.NavLinks on com.NavLinkId equals lin.NavLinkId
    //                       join linp in _db.NavLinks on lin.NavLinkParentId equals linp.NavLinkId into temp
    //                       from linp in temp.DefaultIfEmpty()
    //                       where (vmModel.Source == null || lin.Source == vmModel.Source || (vmModel.Source == "ERP" && lin.Source == null)
    //                       )
    //                       select new RoleNavigationMapping
    //                       {
    //                           IsDeleted = com.IsDeleted,
    //                           NavLinkId = com.NavLinkId,
    //                           NavLinkRoleId = com.NavLinkRoleId,
    //                           RoleId = com.RoleId,
    //                           Rolename = rol.Name,
    //                           Linkname = lin.Title,
    //                           LinkText = lin.LinkText,
    //                           ParentName = linp.Title,
    //                           Source = lin.Source == null ? "ERP" : lin.Source,
    //                           CreatedDateTime = com.CreatedDateTime
    //                       }).AsEnumerable().OrderByDescending(x => x.CreatedDateTime).ToList();
    //            if (!String.IsNullOrWhiteSpace(vmModel.Linkname))
    //            {
    //                obj = obj.Where(x => x.Linkname.Trim().ToLower().Contains(vmModel.Linkname.Trim().ToLower())).ToList();
    //            }
    //            if (!String.IsNullOrWhiteSpace(vmModel.Rolename))
    //            {
    //                obj = obj.Where(x => x.Rolename.Trim().ToLower().Contains(vmModel.Rolename.Trim().ToLower())).ToList();
    //            }
    //            return obj;
    //        }
    //        catch (Exception)



    //        {
    //            throw;
    //        }
    //    }



    //    public int AddNavRole(RoleNavigationMapping nav)
    //    {
    //        try
    //        {
    //            for (int i = 0; i < nav.Navlinkmap.Length; i++)
    //            {
    //                NavLinkRole com = new NavLinkRole();
    //                com.RoleId = nav.RoleId;
    //                com.CreatedBy = AppUserId;
    //                com.NavLinkId = nav.Navlinkmap[i];
    //                com.CreatedDateTime = DateTime.UtcNow;
    //                com.IsDeleted = nav.IsDeleted;
    //                _db.NavLinkRoles.Add(com);
    //                _db.SaveChanges();
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            return 0;
    //        }
    //        return 1;
    //    }



    //    public int UpdateNavRole(RoleNavigationMapping nav)
    //    {
    //        try
    //        {
    //            if (nav.NavLinkId == 0)
    //            {
    //                List<int> ExitingNavlink = _db.NavLinkRoles.Where(x => x.RoleId == nav.RoleId && !x.IsDeleted).Select(x => x.NavLinkId).ToList();
    //                List<int> Allnavlinks = nav.Navlinkmap.Select(x => x).ToList();
    //                var newNavLink = Allnavlinks.Except(ExitingNavlink).ToList();
    //                var deletedNavLink = ExitingNavlink.Except(Allnavlinks).ToList();
    //                if (newNavLink.Count > 0)
    //                {
    //                    foreach (var yy in newNavLink)
    //                    {
    //                        var check = _db.NavLinkRoles.Where(x => x.RoleId == nav.RoleId && x.IsDeleted && x.NavLinkId == yy).FirstOrDefault();
    //                        if (check == null)
    //                        {
    //                            NavLinkRole tree = new NavLinkRole();
    //                            tree.IsDeleted = false;
    //                            tree.RoleId = nav.RoleId;
    //                            tree.NavLinkId = yy;
    //                            tree.IsDeleted = nav.IsDeleted;
    //                            tree.CreatedBy = AppUserId;
    //                            tree.CreatedDateTime = DateTime.UtcNow;
    //                            _db.NavLinkRoles.Add(tree);
    //                            _db.SaveChanges();
    //                        }
    //                        else
    //                        {
    //                            var delrecord = _db.NavLinkRoles.Where(x => x.RoleId == nav.RoleId && x.IsDeleted && x.NavLinkId == yy).FirstOrDefault();
    //                            delrecord.IsDeleted = false;
    //                            delrecord.UpdatedBy = AppUserId;
    //                            delrecord.UpdatedDateTime = DateTime.UtcNow;
    //                            _db.SaveChanges();
    //                        }
    //                    }
    //                }
    //                if (deletedNavLink.Count > 0)
    //                {
    //                    foreach (var delClient in deletedNavLink)
    //                    {



    //                        var delrecord = _db.NavLinkRoles.Where(x => x.RoleId == nav.RoleId && !x.IsDeleted && x.NavLinkId == delClient).FirstOrDefault();
    //                        delrecord.IsDeleted = true;
    //                        delrecord.UpdatedBy = AppUserId;
    //                        delrecord.UpdatedDateTime = DateTime.UtcNow;
    //                        _db.SaveChanges();
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                var delrecord = _db.NavLinkRoles.Where(x => x.NavLinkRoleId == nav.NavLinkRoleId).FirstOrDefault();
    //                if (nav.IsDeleted)
    //                {
    //                    delrecord.IsDeleted = true;
    //                    delrecord.UpdatedBy = AppUserId;
    //                    delrecord.UpdatedDateTime = DateTime.UtcNow;
    //                }
    //                else
    //                {
    //                    delrecord.IsDeleted = false;
    //                    delrecord.UpdatedBy = AppUserId;
    //                    delrecord.UpdatedDateTime = DateTime.UtcNow;
    //                }
    //                _db.SaveChanges();
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            return 0;
    //        }
    //        return 1;
    //    }
    //    public async Task<List<NavLinkViewModel>> GetMenuItems(string userId)
    //    {
    //        List<NavLinkViewModel> obj = new List<NavLinkViewModel>();
    //        try
    //        {
    //            var RoleIDs = await _db.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync();
    //            obj = await (from nlinks in _db.NavLinks
    //                         join nlroles in _db.NavLinkRoles.Where(x => RoleIDs.Contains(x.RoleId))
    //                         on nlinks.NavLinkId equals nlroles.NavLinkId
    //                         join uroles in _db.UserRoles on nlroles.RoleId equals uroles.RoleId
    //                         where !nlroles.IsDeleted && !nlinks.IsDeleted && uroles.UserId == userId
    //                         && nlinks.Source == "LCP"
    //                         select new NavLinkViewModel
    //                         {
    //                             Title = nlinks.Title,
    //                             Url = nlinks.Url,
    //                             IconClass = nlinks.IconClass,
    //                             IconUrl = nlinks.IconUrl,
    //                             NavLinkId = nlinks.NavLinkId,
    //                             NavLinkParentId = nlinks.NavLinkParentId != null ? nlinks.NavLinkParentId : 0,
    //                             LinkOrder = nlinks.LinkOrder,
    //                             LinkText = nlinks.LinkText,
    //                             LinkJson = nlinks.LinkJson,
    //                             IsDeleted = nlinks.IsDeleted
    //                         }).Distinct().OrderBy(x => x.LinkOrder).ToListAsync();
    //            return obj;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //}
}
