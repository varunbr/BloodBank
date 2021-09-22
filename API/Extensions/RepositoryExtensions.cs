using System.Linq;
using API.DTOs;

namespace API.Extensions
{
    public static class RepositoryExtensions
    {
        public static void UpdateRole(this BankModeratorDto bank, int userId)
        {
            bank.Role = bank.Moderators.FirstOrDefault(i => i.UserId == userId)?.Type;
            if (bank.Role != "BankAdmin") bank.Moderators = null;
        }
    }
}
