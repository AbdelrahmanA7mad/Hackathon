using Microsoft.AspNetCore.Identity;
using System.Net;

namespace WaZuF.Models
{
    public class Company : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public List<JobRequest> JobRequests { get; set; } = new();
    }

}
