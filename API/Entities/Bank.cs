using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public DateTime LastUpdated { get; set; }
        public Photo Photo { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public ICollection<Moderator> Moderators { get; set; }
    }
}
