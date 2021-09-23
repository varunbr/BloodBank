using System.Collections.Generic;

namespace API.DTOs
{
    public class BankRoleUpdateDto
    {
        public int BankId { get; set; }
        public ICollection<ModeratorDto> Moderators { get; set; }
    }
}
