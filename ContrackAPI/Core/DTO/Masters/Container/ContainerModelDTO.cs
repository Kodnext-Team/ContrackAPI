using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContrackAPI
{
    public class ContainerModelDTO
    {
        public EncryptedData modelid { get; set; } = new EncryptedData();
        public string modeluuid { get; set; } = "";
        public EncryptedData sizeid { get; set; } = new EncryptedData();
        public EncryptedData typeid { get; set; } = new EncryptedData();
        public string iso_code { get; set; } = "";
        public string description { get; set; } = "";
        public DateTime createdat { get; set; } = new DateTime();
        public int AvailableCount { get; set; } = 0;
        public int BookedCount { get; set; } = 0;
        public int RepairCount { get; set; } = 0;
        public int TotalCount { get; set; } = 0;
    }

    public class ContainerModelExtendedDTO
    {
        public EncryptedData modelid { get; set; } = new EncryptedData();
        public string modeluuid { get; set; } = "";
        public EncryptedData sizeid { get; set; } = new EncryptedData();
        public string iso_code { get; set; } = "";
        public string description { get; set; } = "";
        public EncryptedData typeid { get; set; } = new EncryptedData();
        public DateTime createdat { get; set; } = new DateTime();
        public int AvailableCount { get; set; } = 0;
        public int BlockedCount { get; set; } = 0;
        public int BookedCount { get; set; } = 0;
        public int RepairCount { get; set; } = 0;
        public int TotalCount { get; set; } = 0;
        public string sizename { get; set; } = "";
        public string length { get; set; } = "";
        public string width { get; set; } = "";
        public string height { get; set; } = "";
        public int sizeorderby { get; set; } = 0;
        public EncryptedData containertypeid { get; set; } = new EncryptedData();
        public string typeuuid { get; set; } = "";
        public string typename { get; set; } = "";
        public string typeshortname { get; set; } = "";
        public EncryptedData iconid { get; set; } = new EncryptedData();
        public string icon { get; set; } = "";
    }

}