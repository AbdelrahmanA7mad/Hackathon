using Microsoft.AspNetCore.Identity;
using System.Net;

namespace WaZuF.Models
{
    public class AppUser : IdentityUser
    {
        public string? UserType { get; set; }

        public string ? FirstName { get; set; }
        public string ? LastName { get; set; }
        public string ? CompanyName { get; set; }
        public string? RegistrationNumber { get; set; }
        public List<JobRequest> JobRequests { get; set; } = new();
    }

}
