using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.DLL.Models
{
    public class ApplicationUser : IdentityUser//, IAuditableEntity

    {

        public virtual string FriendlyName

        {

            get

            {

                string friendlyName = string.IsNullOrWhiteSpace(FullName) ? UserName : FullName;

                if (!string.IsNullOrWhiteSpace(JobTitle))

                    friendlyName = $"{JobTitle} {friendlyName}";

                return friendlyName;

            }

        }

        public int AppUserId { get; set; }

        public string? LoginName { get; set; }

        public int? DefaultDivisionID { get; set; }

        public string? UserType { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? JobTitle { get; set; }

        public bool IsEnabled { get; set; }

        public string? FullName { get; set; }

        public string? Configuration { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public bool? IsActive { get; set; }

        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;

        public string? OtpSecret { get; set; }

        public string? Pin { get; set; }

        /// <summary>

        /// Navigation property for the roles this user belongs to.

        /// </summary>

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        /// <summary>

        /// Navigation property for the claims this user possesses.

        /// </summary>

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

    }
    
}
