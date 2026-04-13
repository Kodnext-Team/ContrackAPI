namespace ContrackAPI
{
    public class LoginResponse 
    {
        public Result Result { get; set; }
        public string Token { get; set; }
        public object Data { get; set; }
    }
}
