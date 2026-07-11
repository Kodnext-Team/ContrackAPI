using System;

namespace ContrackAPI
{
    public class BookedContainerDTO
    {
        public int RowIndex { get; set; } = 0;
        public long TotalCount { get; set; } = 0;
        public EncryptedData ContainerId { get; set; } = new EncryptedData();
        public bool IsChecked { get; set; } = false;
        public string ContainerUuid { get; set; } = "";
        public string ContainerNo { get; set; } = "";
        public string IsoCode { get; set; } = "";
        public string ContainerTypeName { get; set; } = "";
        public string ContainerSizeName { get; set; } = "";
        public string operatorname { get; set; }
        public bool IsDamaged { get; set; } = false;
        public string VoyageUuid { get; set; } = "";
        public string VoyageNumber { get; set; } = "";
        public string PolPortCode { get; set; } = "";
        public string PolPortName { get; set; } = "";
        public string PolCountryCode { get; set; } = "";
        public string PolCountryName { get; set; } = "";
        public string PolCountryFlag { get; set; } = "";

        public string PodPortCode { get; set; } = "";
        public string PodPortName { get; set; } = "";
        public string PodCountryCode { get; set; } = "";
        public string PodCountryName { get; set; } = "";
        public string PodCountryFlag { get; set; } = "";
        public string CurrentLocationName { get; set; } = "";
        public string CurrentPortName { get; set; } = "";
        public string CurrentCountryName { get; set; } = "";
        public string CurrentCountryFlag { get; set; } = "";
        public string MoveName { get; set; } = "";
        public int MoveIconID { get; set; } = 0;
        public FormattedValue<int> IsEmpty { get; set; } = new FormattedValue<int>();
        public FormattedValue<DateTime> lastmovedatetime { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);


        //public int ContainerTypeID { get; set; } = 0;
        //public int ContainerSizeID { get; set; } = 0;

        //public string Stamp { get; set; } = "";
        //public decimal Weight { get; set; } = 0;
        //public string Comments { get; set; } = "";
        //public string deliveryfrom { get; set; } = "";
        //public string deliveryto { get; set; } = "";
        //public int full_empty { get; set; } = 0;
        //public decimal yardage { get; set; } = 0;

        /*-------- For Transport Order ------------*/
        //public string dropoff { get; set; } = "";
        //public string transportcomments { get; set; } = "";
        /*-------- For Transport Order ------------*/
    }
}
