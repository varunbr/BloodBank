namespace API.Helpers
{
    public class BaseParams
    {
        private const int MaxSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxSize ? MaxSize : value;
        }
        public string BloodGroup { get; set; }
        public string Address { get; set; }
        public string OrderBy { get; set; }
    }
}
