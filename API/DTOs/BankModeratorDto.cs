using System.Collections.Generic;

namespace API.DTOs
{
    public class BankModeratorDto : BankDto
    {
        public string Role { get; set; }
        public ICollection<ModeratorDto> Moderators { get; set; }
    }
}
