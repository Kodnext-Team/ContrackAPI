using ContrackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class ContainerModal
    {
        public ContainerDTO container { get; set; } = new ContainerDTO();
        public List<TrackingListDTO> trackinglist = new List<TrackingListDTO>();
        public int MakeMonth { get; set; }
        public int MakeYear { get; set; }
    }
}