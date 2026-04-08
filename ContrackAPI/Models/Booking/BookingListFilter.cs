using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class BookingListFilter
    {
        public string? sortby { get; set; } = "createdat";
        public string? sortdirection { get; set; } = "DESC";
        public int? startindex { get; set; } = 0;
        public int noofrows { get; set; } = 10;
        public string searchstr { get; set; } = "";
        public BookingFilters filters { get; set; } = new BookingFilters();
        public List<BookingStatusCountDTO> StatusCount { get; set; } = new List<BookingStatusCountDTO>();

        public int GetFilterCount()
        {
            int count = 0;

            if (!string.IsNullOrEmpty(filters.startdate) && !string.IsNullOrEmpty(filters.enddate))
                count++;
            if (!string.IsNullOrWhiteSpace(filters.bookingno))
            {
                count++;
            }
            if (filters.agencyuuids.Count > 0)
                count++;
            if (filters.clientuuids.Count > 0)
                count++;
            if (filters.status.Count > 0)
                count++;
            if (!string.IsNullOrEmpty(filters.polencrypt))
            {
                count++;
            }
            if (!string.IsNullOrEmpty(filters.podencrypt))
            {
                count++;
            }
            if (!string.IsNullOrWhiteSpace(filters.voyageno))
            {
                count++;
            }
            if (filters.vesseldetailid.Count > 0)
                count++;
            if (filters.createdby.Count > 0)
                count++;

            return count;
        }
    }


    public class BookingFilters
    {
        public string? startdate { get; set; } = "";
        public string? enddate { get; set; } = "";
        public List<string>? agencyuuids { get; set; } = new List<string>();
        public List<string>? clientuuids { get; set; } = new List<string>();
        public string? bookingno { get; set; } = "";
        public int? pol { get; set; } = 0;
        public string? polencrypt { get; set; } = "";
        public int? pod { get; set; } = 0;
        public string? podencrypt { get; set; } = "";
        public List<int>? status { get; set; } = new List<int>();
        public List<string>? vesseldetailid { get; set; } = new List<string>();
        public List<int>? vesselid { get; set; } = new List<int>();
        public string? voyageno { get; set; } = "";
        public List<int>? createdby { get; set; } = new List<int>();
    }
    public class BookingStatusCountDTO
    {
        public int StatusId { get; set; }
        public long StatusCount { get; set; }
    }


}