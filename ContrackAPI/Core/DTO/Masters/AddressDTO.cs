namespace ContrackAPI
{
    public class AddressDTO
    {
        public EncryptedData addressid { get; set; } = new EncryptedData();
        public string addressidinc { get; set; } = "";
        public EncryptedData clientid { get; set; } = new EncryptedData();
        public int idreftypegroup { get; set; } = 0;
        public EncryptedData addresstypeid { get; set; } = new EncryptedData();
        public string address { get; set; } = "";
        public string addressname { get; set; } = "";
        public string typevalue { get; set; } = "";
        public bool isdefault { get; set; } = false;
    }
}