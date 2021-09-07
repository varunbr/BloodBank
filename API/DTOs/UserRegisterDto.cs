using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserRegisterDto
    {
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
        [Required]
        [StringLength(12,MinimumLength = 4)]
        public string Password { get; set; }
    }
}
