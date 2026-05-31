namespace ContrackAPI
{
    public class ContainerBookingListDTO
    {
        public int row_index { get; set; }
        public int total_count { get; set; }
        public EncryptedData bookingid { get; set; } = new EncryptedData();
        public string bookinguuid { get; set; } = "";
        public string bookingno { get; set; } = "";
        public FormattedValue<DateTime> bookingdate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public EncryptedData pol { get; set; } = new EncryptedData();
        public string pol_portname { get; set; } = "";
        public string pol_portcode { get; set; } = "";
        public string pol_countryname { get; set; } = "";
        public string pol_countryflag { get; set; } = "";
        public EncryptedData pod { get; set; } = new EncryptedData();
        public string pod_portname { get; set; } = "";
        public string pod_portcode { get; set; } = "";
        public string pod_countryname { get; set; } = "";
        public string pod_countryflag { get; set; } = "";
        public string voyagenumber { get; set; } = "";
        public string vesselname { get; set; } = "";
        public FormattedValue<int> status { get; set; } = new FormattedValue<int>();
        public string agencyname { get; set; } = "";
        public string customername { get; set; } = "";

    }
}
