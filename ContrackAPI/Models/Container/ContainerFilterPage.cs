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
        [System.Text.Json.Serialization.JsonIgnore]
        public List<ContainerStatusCountDTO> StatusCount { get; set; } = new List<ContainerStatusCountDTO>();

        //public int GetFilterCount()
        //{
        //    int count = 0;
        //    //if (!string.IsNullOrEmpty(filters.containertype_encry)) count++;
        //    //if (!string.IsNullOrEmpty(filters.containersize_encry)) count++;
        //    //if (!string.IsNullOrEmpty(filters.containermodel_encry)) count++;
        //    //if (!string.IsNullOrEmpty(filters.location_encry)) count++;
        //    if (!string.IsNullOrEmpty(filters.pol_encry)) count++;
        //    if (!string.IsNullOrEmpty(filters.pod_encry)) count++;
        //    if (!string.IsNullOrEmpty(filters.voyage_encry)) count++;
        //    if (!string.IsNullOrEmpty(filters.move_encry)) count++;
        //   // if (!string.IsNullOrEmpty(filters.operator_encry)) count++;
        //    if (!string.IsNullOrEmpty(filters.bookingno)) count++;
        //  //  if (filters.status > 0) count++;
        //    return count;
        //}
    }
    public class ContainerFilter
    {
        // Multiple Select
        public List<string> containertypeencids { get; set; } = new();
        public List<string> containersizeencids { get; set; } = new();
        public List<string> containermodeluuids { get; set; } = new();
        public List<string> locationuuids { get; set; } = new();

        // Multiple Select Operator
        public List<string> operatorencids { get; set; } = new();

        // Single Select
        public string pol_encry { get; set; }
        public string pod_encry { get; set; }
        public string voyage_encry { get; set; }
        public string move_encry { get; set; }

        public string bookingno { get; set; }

        // Multiple Status Selection
        public List<int> status { get; set; } = new();

        // Internal fields
        [System.Text.Json.Serialization.JsonIgnore]
        public List<long> containertypeids { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore]
        public List<int> containersizeids { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore]
        public List<int> operatorids { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore]
        public List<int> pols { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore]
        public List<int> pods { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore]
        public List<int> voyageids { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore]
        public List<int> moveids { get; set; } = new();
    }
    public class ContainerStatusCountDTO
    {
        public int status_code { get; set; }
        public string status_name { get; set; }
        public long status_count { get; set; }
    }
}