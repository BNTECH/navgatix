using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace satguruApp.DLL.Models;

public partial class DriverKYC
{
    [Key]
    public Guid Id { get; set; }

    public Guid DriverId { get; set; }

    [Required]
    [MaxLength(50)]
    public string DocumentType { get; set; } // e.g., "Aadhaar", "DrivingLicense"

    [Required]
    public string DocumentUrl { get; set; }

    [MaxLength(20)]
    public string VerifiedStatus { get; set; } = "Pending"; // Pending, Approved, Rejected

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("DriverId")]
    public virtual Driver Driver { get; set; }
}
