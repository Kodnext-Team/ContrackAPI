using ContrackAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContrackAPI
{
    public class VoyageExtendedDTO
    {
        public EncryptedData VoyageId { get; set; } = new EncryptedData();
        public string VoyageUuid { get; set; } = "";
        public string VoyageNumber { get; set; } = "";
        public string actualvoyagenumber { get; set; } = "";
        public string Vesselname { get; set; } = "";
        public EncryptedData VesseDetailId { get; set; } = new EncryptedData();
        public string Description { get; set; } = "";
        public VoyageDetailDTO VoyageDetails { get; set; } = new VoyageDetailDTO();

    }
    public class VoyageDTO
    {
        public EncryptedData VoyageId { get; set; } = new EncryptedData();
        public string VoyageUuid { get; set; } = "";
        public string VoyageNumber { get; set; } = "";
        public string ActualVoyageNumber { get; set; } = "";
        public EncryptedData OriginPort { get; set; } = new EncryptedData();
        public EncryptedData DestinationPort { get; set; } = new EncryptedData();
        public EncryptedData vesselassignmentid { get; set; } = new EncryptedData();
        public EncryptedData VesseDetailId { get; set; } = new EncryptedData();
        public string Vesselname { get; set; } = "";
        public bool IsLive { get; set; }
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
        public string Text { get; set; } = "";
        public bool IsNew { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public FormattedValue<DateTime> minDate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public FormattedValue<DateTime> maxDate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public int NoOfDays { get; set; } = 0;
        public List<VoyageDetailDTO> VoyageDetails { get; set; } = new List<VoyageDetailDTO>();
        public int totalnoofrows { get; set; } = 0;
        public int total_count { get; set; } = 0;

    }
    public class VoyageDetailDTO
    {
        public EncryptedData VoyageDetailId { get; set; } = new EncryptedData();
        public EncryptedData VoyageId { get; set; } = new EncryptedData();
        public EncryptedData PortId { get; set; } = new EncryptedData();
        public string portname { get; set; } = "";
        public string Terminal { get; set; } = "";
        public FormattedValue<DateTime> ETA { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public FormattedValue<DateTime> ETD { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public FormattedValue<DateTime> CutoffDeadline { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public FormattedValue<DateTime> ATA { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public FormattedValue<DateTime> ATD { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public string ArrivalCaptain { get; set; } = "";
        public string DepartureCaptain { get; set; } = "";
        public string portcode { get; set; } = "";
        public int PortCountryId { get; set; } = 0;
        public int CountryId { get; set; } = 0;
        public string CountryName { get; set; } = "";
        public string CountryFlag { get; set; } = "";
        public string PortStatus { get; set; } = "";
        public int SortOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    }



}