using System;

namespace API.DTOs
{
    public class BankDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public DateTime LastUpdated { get; set; }
        public string PhotoUrl { get; set; }
        public BloodGroupDto BloodGroup { get; set; }
    }
}