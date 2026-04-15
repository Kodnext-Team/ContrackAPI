namespace ContrackAPI
{
    public class ContainerBookingListDTO
    {
        public int row_index { get; set; }
        public int total_count { get; set; }
        public EncryptedData bookingid { get; set; } = new EncryptedData();
        public string bookinguuid { get; set; } = "";
        public EncryptedData agencydetailid { get; set; } = new EncryptedData();
        public string bookingno { get; set; } = "";
        public DateTime bookingdate { get; set; } = DateTime.Now;
        public EncryptedData clientdetailid { get; set; } = new EncryptedData();
        public string clientuuid { get; set; } = "";
        public string agencyuuid { get; set; } = "";
        public int customertype { get; set; } = 0;
        public string customertypename { get; set; } = "";
        public EncryptedData pol { get; set; } = new EncryptedData();
        public string pol_portname { get; set; } = "";
        public string pol_portcode { get; set; } = "";
        public string pol_countryname { get; set; } = "";
        public string pol_countrycode { get; set; } = "";
        public string pol_countryflag { get; set; } = "";
        public EncryptedData pod { get; set; } = new EncryptedData();
        public string pod_portname { get; set; } = "";
        public string pod_portcode { get; set; } = "";
        public string pod_countryname { get; set; } = "";
        public string pod_countrycode { get; set; } = "";
        public string pod_countryflag { get; set; } = "";
        public EncryptedData shipperdetailid { get; set; } = new EncryptedData();
        public EncryptedData consigneedetailid { get; set; } = new EncryptedData();
        public string voyageuuid { get; set; } = "";
        public string voyagenumber { get; set; } = "";
        public string vesselname { get; set; } = "";
        public FormattedValue<int> status { get; set; } = new FormattedValue<int>();
        public FormattedValue<DateTime> createdat { get; set; } = new FormattedValue<DateTime>();
        public int createdby { get; set; } = 0;
        public int noofcontainers { get; set; } = 0;
        public int sumofqty { get; set; } = 0;
        public string createdbyusername { get; set; } = "";
        public string agencyname { get; set; } = "";
        public string customername { get; set; } = "";
        public string booking_details { get; set; } = "";
    }
}
