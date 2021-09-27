using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class BankRegisterDto
    {
        [Required] public string Name { get; set; }
        [Required] public string Area { get; set; }
        [Required] public string City { get; set; }
        [Required] public string State { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string PostalCode { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required] public string Email { get; set; }
        public string Website { get; set; }
        [Required] public string BankAdmin { get; set; }
    }
}
