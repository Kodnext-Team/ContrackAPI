using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class ContainerBookingSummaryDTO
    {
        public EncryptedData bookingid { get; set; } = new EncryptedData();
        public string bookinguuid { get; set; } = "";
        public string bookingno { get; set; } = "";
        public DateTime createdat { get; set; } = new DateTime();
        public string createdusername { get; set; } = "";
    }
}