namespace ContrackAPI
{
    public class ClientContact
    {
        public EncryptedData picid { get; set; } = new EncryptedData();
        public EncryptedData clientid { get; set; } = new EncryptedData();
        public string fullname { get; set; } = "";
        public string email { get; set; } = "";
        public string phone { get; set; } = "";
        public string position { get; set; } = "";
        public bool isprimary { get; set; } = false;
    }
}