using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class CreationInfo
    {
        public DateTime Timestamp { get; set; } = new DateTime();
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
    }
}