namespace API.Helpers
{
    public class UserParams : BaseParams
    {
        public string CurrentUserName { get; set; }
        public string Gender { get; set; }
        public int MaxAge { get; set; }
        public int MinAge { get; set; }

    }
}
