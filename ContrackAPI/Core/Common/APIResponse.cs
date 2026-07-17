namespace ContrackAPI
{
    public class APIResponse
    {
        public Result Result { get; set; }  = new Result();
        public object Data { get; set; }
    }

    public class APIResponseType
    {
        public Result Result { get; set; } = new Result();
        public int MatchType { get; set; }
        public object Data { get; set; }

    }

}
