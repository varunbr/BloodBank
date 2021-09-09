using System;

namespace API.DTOs
{
    public class MemberDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhotoUrl { get; set; }
        public string BloodGroup { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime LastActive { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
