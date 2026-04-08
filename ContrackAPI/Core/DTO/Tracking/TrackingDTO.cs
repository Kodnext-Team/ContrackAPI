using ContrackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class TrackingDTO
    {
        public EncryptedData TrackingId { get; set; } = new EncryptedData();
        public string TrackingUuid { get; set; } = "";
        public MovesDTO Moves { get; set; } = new MovesDTO();
        public EncryptedData LocationDetailId { get; set; } = new EncryptedData();
        public string RecordDateTime { get; set; } = "";
        public MovesDTO NextMoves { get; set; } = new MovesDTO();
        public EncryptedData NextLocationDetailId { get; set; } = new EncryptedData();
        public string NextDateTime { get; set; } = "";
        public bool IsDamaged { get; set; } = false;
        public int NoOfDamages { get; set; } = 0;
        public EncryptedData CurrentVoyageId { get; set; } = new EncryptedData();
        public string CurrentVoyageName { get; set; } = "";
        public EncryptedData NextVoyageId { get; set; } = new EncryptedData();
        public string NextVoyageName { get; set; } = "";
        public CreationInfo CreationInfo { get; set; } = new CreationInfo();
        public ContainerDTO Container { get; set; } = new ContainerDTO();
        public ContainerBookingSummaryDTO ContainerBooking { get; set; } = new ContainerBookingSummaryDTO();
        public string PickSelectionUuid { get; set; } = "";
        //public List<TrackingDamageDTO> TrackingDamages { get; set; } = new List<TrackingDamageDTO>();
    }
    public class TrackingListDTO
    {
        public TableCounts rowcount { get; set; } = new TableCounts();
        public EncryptedData trackingid { get; set; } = new EncryptedData();
        public string trackinguuid { get; set; } = "";
        public string location_countryflag { get; set; }
        public FormattedValue<DateTime> recorddatetime { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public string movesname { get; set; } = "";
        public string move_icon { get; set; } = "";
        public string in_out { get; set; } = "";
        public EncryptedData containerid { get; set; } = new EncryptedData();
        public EncryptedData bookingid { get; set; } = new EncryptedData();
        public string locationuuid { get; set; } = "";
        public string locationname { get; set; } = "";
        public string location_portcode { get; set; } = "";
        public string location_countryname { get; set; } = "";
        public string containeruuid { get; set; } = "";
        public string containerno { get; set; } = "";
        public string containersizetype { get; set; } = "";
        public bool isempty { get; set; }
        public bool isdamaged { get; set; }
        public int noofdamages { get; set; }
        public string bookingno { get; set; } = "";
        public string customername { get; set; } = "";
        public string nextmovename { get; set; } = "";
        public string nextlocationname { get; set; } = "";
        public FormattedValue<DateTime> nextdatetime { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public bool canedit { get; set; } = false;
    }
    public class TrackingDamageDTO
    {
        public EncryptedData TrackingdDamageId { get; set; } = new EncryptedData();
        public string TrackingDamageUuid { get; set; } = "";
        public EncryptedData DamageSide { get; set; } = new EncryptedData();
        public string DamageSideName { get; set; } = "";
        public EncryptedData DamageType { get; set; } = new EncryptedData();
        public string DamageTypeName { get; set; } = "";
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Comments { get; set; } = "";
        public CreationInfo CreationInfo { get; set; } = new CreationInfo();
    }
}