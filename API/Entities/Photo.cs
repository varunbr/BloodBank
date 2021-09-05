namespace API.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public string Url { get; set; }
        public int? UserId { get; set; }
        public AppUser User { get; set; }
        public int? BankId { get; set; }
        public Bank Bank { get; set; }
    }
}
