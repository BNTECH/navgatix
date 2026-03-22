using System;

namespace satguruApp.Service.ViewModels;

public class DriverKYCViewModel
{
    public Guid? Id { get; set; }
    public Guid DriverId { get; set; }
    public string DocumentType { get; set; }
    public string DocumentUrl { get; set; }
    public string VerifiedStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}
