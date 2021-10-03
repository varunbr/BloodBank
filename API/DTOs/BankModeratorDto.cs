using System.Collections.Generic;

namespace API.DTOs
{
    public class BankModeratorDto : BankDto
    {
        public string Role { get; set; }
        public ICollection<RoleDto> Moderators { get; set; }
    }
}
