namespace ContrackAPI
{

    public class UserFilter
    {
        public string UserType { get; set; } = "";
        public string Role { get; set; } = "";
        public List<string> EntityID { get; set; } = new List<string>();
        public string Search { get; set; } = "";
        public string sorting { get; set; } = "createdat";
        public string sortingorder { get; set; } = "desc";
        public int limit { get; set; } = 10;
        public int offset { get; set; } = 0;
    }
}