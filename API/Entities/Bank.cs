using System;
using System.Collections.Generic;
using System.Linq;
using API.Helpers;

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
        public ICollection<BloodGroup> BloodGroups { get; set; }
        public ICollection<Moderator> Moderators { get; set; }

        public static Bank Create()
        {
            var bank = new Bank
            {
                Photo = new Photo(),
                Moderators = new List<Moderator>(),
                BloodGroups = Util.GetBloodGroupList().Select(x => new BloodGroup { Group = x }).ToList()
            };

            return bank;
        }
    }
}
