namespace API.Entities
{
    public class BloodGroup
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public int Value { get; set; }
        public int BankId { get; set; }
        public Bank Bank { get; set; }
    }
}
