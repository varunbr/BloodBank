namespace API.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public int? UserId { get; set; }
        public AppUser User { get; set; }
        public int? BankId { get; set; }
        public Bank Bank { get; set; }
    }
}
