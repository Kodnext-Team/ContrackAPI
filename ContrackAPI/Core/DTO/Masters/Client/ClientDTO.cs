namespace ContrackAPI
{
    public class ClientExtra
    {
        public string clientshortcode { get; set; } = "";
        public string clientcolor { get; set; } = "";
        public string clientbgcolor { get; set; } = "";
    }

    public class CustomerAnalyticsDTO
    {
        public int totalenquiries { get; set; } = 0;
        public int converted { get; set; } = 0;
        public decimal conversionrate { get; set; } = 0;
        public int totalinvoice { get; set; } = 0;
        public int unpaidtotalinvoice { get; set; } = 0;
    }

    public class ClientDTO
    {
        //public EncryptedData clientid { get; set; } = new EncryptedData();
        public EncryptedData clientdetailid { get; set; } = new EncryptedData();
        public string clientname { get; set; } = "";
        public string address { get; set; } = "";
        public string email { get; set; } = "";

        public string phone { get; set; } = "";
       
    }
}