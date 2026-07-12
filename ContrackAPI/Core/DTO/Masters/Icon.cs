using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class Icon
    {
        public EncryptedData iconid { get; set; } = new EncryptedData();
        public string icon { get; set; } = "";
        public EncryptedData type { get; set; } = new EncryptedData();
    }
}