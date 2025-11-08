using System;
using System.Collections.Generic;

namespace satguruApp.DLL.Models;

public partial class UserInformation
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string MiddleName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public long? Mobile { get; set; }

    public int? AccountTypeId { get; set; }

    public int? CourseId { get; set; }

    public string UserId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string WhatsAppLink { get; set; }

    public string TwiterLink { get; set; }

    public string FacebookLink { get; set; }

    public string InstagramLink { get; set; }

    public string WebsiteLink { get; set; }

    public string Company { get; set; }

    public int? GenderId { get; set; }

    public DateTime? DOB { get; set; }

    public string Description { get; set; }

    public string ProfilePic { get; set; }
}
