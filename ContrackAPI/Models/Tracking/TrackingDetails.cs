using ContrackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class TrackingDetails
    {
        public List<TrackingDetailsDTO> Trackingdetails { get; set; } = new List<TrackingDetailsDTO>();
        public TrackingBookingSummaryDTO booking { get; set; } = new TrackingBookingSummaryDTO();
    }
}