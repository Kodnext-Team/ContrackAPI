using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class IconDTO
    {
        public string icon { get; set; } = "";
        public int IconId { get; set; } = 0;
        public int type { get; set; } = 0;
        public string iconselected { get; set; } = "";
        public bool iscss { get; set; } = false;
    }
}