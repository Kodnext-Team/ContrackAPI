using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class Login
    {
        public LoginDTO LoginInfo { get; set; } = new LoginDTO();
        //public Result result { get; set; }
        public HubDTO HubInfo { get; set; } = new HubDTO();
    }
}