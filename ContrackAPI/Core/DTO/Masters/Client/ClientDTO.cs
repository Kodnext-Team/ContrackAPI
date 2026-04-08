using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        public EncryptedData clientid { get; set; } = new EncryptedData();
        public EncryptedData clientdetailid { get; set; } = new EncryptedData();
        public ClientExtra extras { get; set; } = new ClientExtra();
        public string clientuuid { get; set; } = "";
        public string picname { get; set; } = "";
        public string picshort { get; set; } = "";
        public string picemail { get; set; } = "";
        public string picdesignation { get; set; } = "";
        public string TypeEncrypted { get; set; } = "";
        public int hubid { get; set; } = 0;
        public string clientname { get; set; } = "";
        public string imono { get; set; } = "";
        public string address { get; set; } = "";
        public string billingaddress { get; set; } = "";
        public string email { get; set; } = "";
        public string accountsemail { get; set; } = "";
        public string emailtemp { get; set; } = "";
        public string accountsemailtemp { get; set; } = "";
        public string phone { get; set; } = "";
        public string ModifiedBy { get; set; } = "";
        public string preferredcurrency { get; set; } = "USD";
        public int paymentterms { get; set; } = 30;
        public string standardinstructions { get; set; } = "";

        public DateTime AgencyModifiedAt { get; set; } = new DateTime();
        public DateTime hcreatedat { get; set; } = new DateTime();
        public AgencyDTO agency { get; set; } = new AgencyDTO();
        public List<KeyValuePair> CustomAttributes { get; set; } = new List<KeyValuePair>();
        public List<CustomBankInfo> BankInfo { get; set; } = new List<CustomBankInfo>();
        public List<AddressDTO> Addresses { get; set; } = new List<AddressDTO>();
        public List<ClientContact> Contacts { get; set; } = new List<ClientContact>();
        public int row_index { get; set; } = 0;
        public int totalnoofrows { get; set; } = 0;
    }
}