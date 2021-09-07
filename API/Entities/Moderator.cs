namespace API.Entities
{
    public class Moderator
    {
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int BankId { get; set; }
        public Bank Bank { get; set; }
        public string Type { get; set; }
    }
}
