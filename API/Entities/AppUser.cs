using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime LastActive { get; set; }
        public string BloodGroup { get; set; }
        public Photo Photo { get; set; }
        public Address Address { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
