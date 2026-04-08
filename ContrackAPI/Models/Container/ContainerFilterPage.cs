using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace ContrackAPI
{
    public class ContainerFilterPage
    {
        public int startindex { get; set; } = 0;
        public int noofrows { get; set; } = 20;
        public string sortby { get; set; } = "containercreatedat";
        public string sortdirection { get; set; } = "DESC";
        public string searchstr { get; set; } = "";

        public ContainerFilter filters { get; set; } = new ContainerFilter();
        public List<ContainerStatusCountDTO> StatusCount { get; set; } = new List<ContainerStatusCountDTO>();

        public int GetFilterCount()
        {
            int count = 0;
            if (!string.IsNullOrEmpty(filters.containertype_encry)) count++;
            if (!string.IsNullOrEmpty(filters.containersize_encry)) count++;
            if (!string.IsNullOrEmpty(filters.containermodel_encry)) count++;
            if (!string.IsNullOrEmpty(filters.location_encry)) count++;
            if (!string.IsNullOrEmpty(filters.pol_encry)) count++;
            if (!string.IsNullOrEmpty(filters.pod_encry)) count++;
            if (filters.status > 0) count++;
            return count;
        }
    }
    public class ContainerFilter
    {
        public string containertype_encry { get; set; }
        public string containersize_encry { get; set; }
        public string containermodel_encry { get; set; }
        public string location_encry { get; set; }
        public string pol_encry { get; set; }
        public string pod_encry { get; set; }
        [JsonProperty("status_selected")]
        public int status { get; set; }
        public List<long> containertypeids { get; set; } = new List<long>();
        public List<int> containersizeids { get; set; } = new List<int>();
        public List<string> containermodeluuids { get; set; } = new List<string>();
        public List<long> locationdetailids { get; set; } = new List<long>();
        public List<int> pols { get; set; } = new List<int>();
        public List<int> pods { get; set; } = new List<int>();
        [JsonProperty("status")]
        public List<int> status_list { get; set; } = new List<int>();
    }
    public class ContainerStatusCountDTO
    {
        public int status_code { get; set; }
        public string status_name { get; set; }
        public long status_count { get; set; }
    }
}