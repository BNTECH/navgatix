using System;

namespace satguruApp.Service.ViewModels
{
    public class FirebaseAuthRequestViewModel
    {
        public string FirebaseIdToken { get; set; } = string.Empty;
        public string? RoleName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Company { get; set; }
        public string? GSTNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string? Provider { get; set; }
        public bool? IsOnline { get; set; }
        public long ? TransporterId { get; set; }
        public string? Message { get; set; }
    }
}
