using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string BloodGroup { get; set; }
        [Required] public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string Area { get; set; }
        [Required] public string City { get; set; }
        [Required] public string State { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string PostalCode { get; set; }
        public string PhotoUrl { get; set; }
    }
}
