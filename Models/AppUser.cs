using Microsoft.AspNetCore.Identity;

namespace Clinc.Models
{
    public class AppUser: IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
