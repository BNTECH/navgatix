//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.DLL.Models
{
    public partial class SatguruDBContext: IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        protected void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationRole>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationRole>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ApplicationUser>().Property(p => p.LockoutEnd).HasColumnName("LockoutEndDateUtc");
            modelBuilder.Entity<ApplicationUser>().Property(p => p.AppUserId).ValueGeneratedOnAdd();
            modelBuilder.Entity<ApplicationUser>().Property(p => p.AppUserId).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().ToTable("UserRoles").ToTable(x => x.HasTrigger("UserRoles_U"));
           // modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<string>>().HasKey(e => new { e.RoleId, e.UserId });
            modelBuilder.Entity<UserRole>().HasKey(e => new { e.RoleId, e.UserId });
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<UserLogin>().HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId });
            modelBuilder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
           // modelBuilder.Entity<UserDevice>().HasKey(c => new { c.AppUserId, c.Device });
            modelBuilder.Entity<UserRoles>().ToTable("UserRoles");
        }
    }
}
