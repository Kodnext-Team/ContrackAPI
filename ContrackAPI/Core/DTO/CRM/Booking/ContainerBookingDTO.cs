namespace ContrackAPI
{
    public class ContainerBookingDTO
    {
        public EncryptedData bookingid { get; set; } = new EncryptedData();
        public string bookinguuid { get; set; } = "";
        public string bookingno { get; set; } = "";
        public DateTime createdat { get; set; } = new DateTime();
        public string createdusername { get; set; } = "";
        public BookingCustomerDTO customer { get; set; } = new BookingCustomerDTO();
        public BookingLocationDTO location { get; set; } = new BookingLocationDTO();
        public List<ContainerBookingDetailDTO> details { get; set; } = new List<ContainerBookingDetailDTO>();
        // using for pol
        public List<BookingAdditionalServicesDTO> additionalservices { get; set; } = new List<BookingAdditionalServicesDTO>();
        public List<BookingAdditionalServicesDTO> PODadditionalservices { get; set; } = new List<BookingAdditionalServicesDTO>();
    }

    public class BookingCustomerDTO
    {
        public EncryptedData agencydetailid { get; set; } = new EncryptedData();
        public string agencyname { get; set; } = "";
        public string bookingdate { get; set; } = Common.ToDateTimeString(DateTime.Now, "yyyy-MM-dd HH:mm");
        public EncryptedData customertype { get; set; } = new EncryptedData() { EncryptedValue = Common.Encrypt(1), NumericValue = 1 };
        public EncryptedData mode { get; set; } = new EncryptedData();
        public FormattedValue<int> status { get; set; } = new FormattedValue<int>();
        public string fullempty { get; set; } = "";
        public ClientDTO client { get; set; } = new ClientDTO();
        public CustomerAnalyticsDTO analytics { get; set; } = new CustomerAnalyticsDTO();
        public bool isconfirmed { get; set; } = false;

    }

    public class BookingLocationDTO
    {
        public EncryptedData pol { get; set; } = new EncryptedData();
        public string pol_portname { get; set; } = "";
        public string pol_portcode { get; set; } = "";
        public int pol_countryid { get; set; }
        public string pol_countryname { get; set; } = "";
        public string pol_countrycode { get; set; } = "";
        public string pol_countryflag { get; set; } = "";
        public EncryptedData pod { get; set; } = new EncryptedData();
        public string pod_portname { get; set; } = "";
        public string pod_portcode { get; set; } = "";
        public int pod_countryid { get; set; }
        public string pod_countryname { get; set; } = "";
        public string pod_countrycode { get; set; } = "";
        public string pod_countryflag { get; set; } = "";
        public EncryptedData shipperdetailid { get; set; } = new EncryptedData();
        public string shippername { get; set; } = "";
        public EncryptedData shipperpic { get; set; } = new EncryptedData();
        public string shipperpiccustom { get; set; } = "";
        public string shipperpic_name { get; set; } = "";
        public string shipperpic_email { get; set; } = "";
        public string shipperpic_phone { get; set; } = "";
        public string shipperemail { get; set; } = "";
        public string shipperemailtemp { get; set; } = "";
        public string shipperphone { get; set; } = "";
        public string shipperphonetemp { get; set; } = "";
        public string shipperaddress { get; set; } = "";
        public EncryptedData consigneedetailid { get; set; } = new EncryptedData();
        public string consigneename { get; set; } = "";
        public EncryptedData consigneepic { get; set; } = new EncryptedData();
        public string consigneepiccustom { get; set; } = "";
        public string consigneepic_name { get; set; } = "";
        public string consigneepic_email { get; set; } = "";
        public string consigneepic_phone { get; set; } = "";
        public string consigneeemail { get; set; } = "";
        public string consigneeemailtemp { get; set; } = "";
        public string consigneephone { get; set; } = "";
        public string consigneephonetemp { get; set; } = "";
        public string consigneeaddress { get; set; } = "";
        public string voyageuuid { get; set; } = "";
    }

    public class ContainerBookingDetailDTO
    {
        public EncryptedData bookingdetailid { get; set; } = new EncryptedData();
        public string bookingdetailuuid { get; set; } = "";
        public EncryptedData bookingid { get; set; } = new EncryptedData();
        public string bookinguuid { get; set; } = "";
        public string containermodeluuid { get; set; } = "";
        public EncryptedData containertypeid { get; set; } = new EncryptedData();
        public EncryptedData sizeid { get; set; } = new EncryptedData();
        public int ownership { get; set; }
        public int qty { get; set; } = 0;
        public string commodity { get; set; } = "";
        public decimal grossweight { get; set; }
        public decimal volumeweight { get; set; }
        public string hscode { get; set; } = "";
        public decimal cargovalue { get; set; }
        public EncryptedData packagetype { get; set; } = new EncryptedData();
        public FormattedValue<DateTime> expectedstuffingdate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public string stuffinglocation { get; set; } = "";
        public string pickuplocation { get; set; } = "";
        public bool isdg { get; set; }
        public bool isreefer { get; set; }
        public string sizename { get; set; } = "";
        public string length { get; set; } = "";
        public string width { get; set; } = "";
        public string height { get; set; } = "";
        public string isocode { get; set; } = "";
        public string modeldescription { get; set; } = "";
        public string containertypeuuid { get; set; } = "";
        public string containertypename { get; set; } = "";
        public string containertypeshortname { get; set; } = "";
        public EncryptedData iconid { get; set; } = new EncryptedData();
        public string icon { get; set; } = "";
        public string empty_full { get; set; } = "";
        public List<ContainerBookingDetailServicesDTO> services { get; set; } = new List<ContainerBookingDetailServicesDTO>();
    }
    public class ContainerBookingDetailServicesDTO
    {
        public EncryptedData bookingdetailserviceid { get; set; } = new EncryptedData();
        public EncryptedData bookingdetailid { get; set; } = new EncryptedData();
        public EncryptedData serviceid { get; set; } = new EncryptedData();
        public EncryptedData servicetype { get; set; } = new EncryptedData();
        public string servicename { get; set; } = "";
        public int serviceorderby { get; set; }
    }

    public class BookingAdditionalServicesDTO
    {
        public EncryptedData bookingadditionalserviceid { get; set; } = new EncryptedData();
        public EncryptedData bookingid { get; set; } = new EncryptedData();
        public EncryptedData additionalserviceid { get; set; } = new EncryptedData();
        public EncryptedData locationtypeid { get; set; } = new EncryptedData();
        public string additionalserviceuuid { get; set; } = "";
        public decimal quantity { get; set; } = 0;
        public string servicename { get; set; } = "";
        public string description { get; set; } = "";
        public int order { get; set; } = 0;
        public string uom { get; set; } = "";
        public int type { get; set; } = 0;
    }

    public class QuotationHeaderDTO
    {
        public EncryptedData quotationid { get; set; } = new EncryptedData();
        public string quotationuuid { get; set; } = "";
        public string quotationno { get; set; } = "";
        public string currency { get; set; } = "USD";
        public FormattedValue<int> status { get; set; } = new FormattedValue<int>();
        public FormattedValue<DateTime> expirydate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public EncryptedData agencydetailid { get; set; } = new EncryptedData();
        public string agencyuuid { get; set; } = "";
        public string agencyname { get; set; } = "";
        public string agencyemail { get; set; } = "";
        public string agencyphone { get; set; } = "";
        public FormattedValue<DateTime> quotedate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public string billto { get; set; } = "";
        public string termsandconditions { get; set; } = "";
        public string remarks { get; set; } = "";
        public string bookinguuid { get; set; }
        public DateTime createdat { get; set; }
        public EncryptedData quotationfor { get; set; } = new EncryptedData();
        public string quotationforname { get; set; } = "";
        public List<QuotationDetailDTO> details { get; set; } = new List<QuotationDetailDTO>();
    }
    public class QuotationDetailDTO
    {
        public EncryptedData quotationdetailid { get; set; } = new EncryptedData();
        public string quotationdetailuuid { get; set; } = "";
        public EncryptedData quotationid { get; set; } = new EncryptedData();
        public string description { get; set; } = "";
        public string remarks { get; set; } = "";
        public EncryptedData containertypeid { get; set; } = new EncryptedData();
        public EncryptedData containersizeid { get; set; } = new EncryptedData();
        public string TypeSizeCombinedText { get; set; } = "";
        public string TypeSizeCombinedValue { get; set; } = "";
        public string TypeSizeName { get; set; } = "";
        public string fullempty { get; set; } = "";
        public string containertypename { get; set; } = "";
        public string containersizename { get; set; } = "";
        public string uom { get; set; } = "";
        public decimal qty { get; set; } = 0;
        public decimal amount { get; set; } = 0;
        public decimal templateprice { get; set; } = 0;
        public DateTime createdat { get; set; }
        public bool isdeleted { get; set; }
        public string jobtypedetailuuid { get; set; } = "";
        public decimal tax { get; set; } = 0;
    }

    public class QuotationList
    {
        public EncryptedData quotationid { get; set; } = new EncryptedData();
        public string quotationuuid { get; set; } = "";
        public string quotationno { get; set; } = "";
        public string bookinguuid { get; set; } = "";
        public string bookingno { get; set; } = "";
        public string createdBy { get; set; } = "";
        public string currency { get; set; } = "";
        public FormattedValue<DateTime> quotedate { get; set; } = new FormattedValue<DateTime>();
        public string quotedateOnly { get; set; } = "";
        public string expirydateOnly { get; set; } = "";
        public FormattedValue<DateTime> expirydate { get; set; } = new FormattedValue<DateTime>();
        public string timeago { get; set; } = "";
        public string billto { get; set; } = "";
        public int ETADays { get; set; } = 0;
        public string totalamountformatted { get; set; }

        public FormattedValue<int> status { get; set; } = new FormattedValue<int>();
        public int approvalstatus { get; set; }
        public int approvedby { get; set; }
        public FormattedValue<DateTime> approveddate { get; set; } = new FormattedValue<DateTime>();
        public FormattedValue<DateTime> createdat { get; set; } = new FormattedValue<DateTime>();
        public EncryptedData agencydetailid { get; set; } = new EncryptedData();
        public string agencyuuid { get; set; } = "";
        public string agencyname { get; set; } = "";
        public string customername { get; set; } = "";
        public int customertype { get; set; } = 0;
        public string customertypename { get; set; } = "";
        public string agencyemail { get; set; } = "";
        public string agencyphone { get; set; } = "";
        public decimal totalamount { get; set; }
        public int totalitems { get; set; }
        public string statusname { get; set; } = "";
        public string approvername { get; set; } = "";
        public int row_index { get; set; }
        public int total_count { get; set; }
        public EncryptedData quotationfor { get; set; } = new EncryptedData();


    }

    public class BookingSummaryDTO
    {
        public EncryptedData summaryid { get; set; } = new EncryptedData();
        public string summaryuuid { get; set; } = "";
        public string bookinguuid { get; set; } = "";
        public bool intramodal_transport { get; set; } = false;
        public string stuffing_unstuffing_units { get; set; } = "";
        public string pickupandreturn_empty_rentalunits { get; set; } = "";
        public string dropoff { get; set; } = "";
        public string currency { get; set; } = "";
        public FormattedValue<DateTime> cutoffdatetime { get; set; } = new FormattedValue<DateTime>();
      //  [AllowHtml]
        public string additionalterms { get; set; } = "";
        public int freetimepol { get; set; } = 0;
        public int freetimepod { get; set; } = 0;
        public decimal freightchargeamount { get; set; } = 0;
        public string freightchargecomments { get; set; } = "";
        public decimal totalothercharges { get; set; } = 0;
        public List<OtherFeesDTO> otherfees { get; set; } = new List<OtherFeesDTO>();
    }

    public class OtherFeesDTO
    {
        public string otherfeeuuid { get; set; } = "";
        public string chargename { get; set; } = "";
        public decimal amount { get; set; } = 0;
        public string uom { get; set; } = "";
        public string remarks { get; set; } = "";
    }
}