namespace API.Helpers
{
    public class AdminRoleParams
    {
        private const int MaxSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 12;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxSize ? MaxSize : value;
        }

        public string Role { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
