using ContrackAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ContrackAPI
{
    public class TrackingFilterPage
    {
        public int startindex { get; set; } = 0;
        public int noofrows { get; set; } = 20;
        public string searchstr { get; set; } = "";
        public string sortby { get; set; } = "recorddatetime";
        public string sortdirection { get; set; } = "DESC";
        public string ContainerUUID { get; set; } = "";
        public TrackingFilter filters { get; set; } = new TrackingFilter();
       // [System.Text.Json.Serialization.JsonIgnore]
       // public ContainerDTO ContainerInfo { get; set; } = new ContainerDTO();
        public int GetFilterCount()
        {
            int count = 0;
            if (filters == null) return 0;
            if (!string.IsNullOrEmpty(filters.startdate)) count++;
            if (!string.IsNullOrEmpty(filters.enddate)) count++;
            if (filters.activityid > 0) count++;
            if (filters.isdamaged > -1) count++;
            if (filters.isfull > -1) count++;
            return count;
        }
    }
    public class TrackingFilter
    {
        [JsonIgnore]
        public string activity_encrypted { get; set; }

        [JsonProperty("startdate")]
        public string startdate { get; set; }

        [JsonProperty("enddate")]
        public string enddate { get; set; }

        [JsonProperty("activityid")]
        public int activityid { get; set; } = 0;

        [JsonProperty("isdamaged")]
        public int isdamaged { get; set; } = -1;

        [JsonProperty("isfull")]
        public int isfull { get; set; } = -1;
    }
}