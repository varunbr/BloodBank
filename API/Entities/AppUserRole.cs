using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public Bank Bank { get; set; }
        public AppRole Role { get; set; }
    }
}
