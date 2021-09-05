namespace API.Entities
{
    public class BloodGroup
    {
        public int Id { get; set; }
        public int Ap { get; set; }
        public int An { get; set; }
        public int Bp { get; set; }
        public int Bn { get; set; }
        public int Op { get; set; }
        public int On { get; set; }
        public int ABp { get; set; }
        public int ABn { get; set; }
        public int BankId { get; set; }
        public Bank Bank { get; set; }
    }
}
