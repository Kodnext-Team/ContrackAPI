using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class AgencyExtra
    {
        public string agencyshortcode { get; set; } = "";
        public string agencycolor { get; set; } = "";
        public string agencybgcolor { get; set; } = "";
    }

    public class AgencyDTO
    {
        public EncryptedData agencyid { get; set; } = new EncryptedData();
        public int agencydetailid { get; set; } = 0;
        public string uuid { get; set; } = "";
        public string agencyname { get; set; } = "";
        public string agencyshortname { get; set; } = "";
        public AgencyExtra extras { get; set; } = new AgencyExtra();
        public string imono { get; set; } = "";
        public string address { get; set; } = "";
        public string billingaddress { get; set; } = "";
        public string email { get; set; } = "";
        public string accountsemail { get; set; } = "";
        public string phone { get; set; } = "";
        public string ModifiedBy { get; set; } = "";
        public List<string> countryList { get; set; } = new List<string>();
        public List<string> portList { get; set; } = new List<string>();
        public string websites { get; set; } = "";
        public DateTime AgencyModifiedAt { get; set; } = new DateTime();
        public DateTime hcreatedat { get; set; } = new DateTime();
        public List<KeyValuePair> CustomAttributes { get; set; } = new List<KeyValuePair>();
        public List<CustomBankInfo> BankInfo { get; set; } = new List<CustomBankInfo>();

    }
}