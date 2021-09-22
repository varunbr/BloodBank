using System.Collections.Generic;

namespace API.DTOs
{
    public class BloodGroupUpdateDto
    {
        public int BankId { get; set; }
        public ICollection<BloodGroupDto> Groups { get; set; }

    }
}
