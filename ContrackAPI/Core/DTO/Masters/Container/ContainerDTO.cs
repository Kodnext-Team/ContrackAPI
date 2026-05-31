namespace ContrackAPI
{
    public class ContainerAvailableDTO
    {
        public string locationname { get; set; } = "";
        public string iso_code { get; set; } = "";
        public int planned_qty { get; set; } = 0;
        public int stock_qty { get; set; } = 0;
        public int short_qty { get; set; } = 0;
        public bool is_available { get; set; } = true;
    }
    public class ContainerDTO
    {
        public EncryptedData containerid { get; set; } = new EncryptedData();
        public string containeruuid { get; set; } = "";
        public string equipmentno { get; set; } = "";
        //public FormattedValue<DateTime> lastbookingdate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        //public FormattedValue<DateTime> manufacturedate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue); 
        public string model_iso_code { get; set; } = "";
        public string sizename { get; set; } = "";
        public string type_name { get; set; } = "";
        public string locationname { get; set; } = "";
        public string operatorname { get; set; }
        //public string bookingno { get; set; } = "";
        //public string bookinguuid { get; set; } = "";
       
        public string locationicon { get; set; } = "";
        public string locationtypename { get; set; } = "";
        public string lastmove { get; set; } = "";
        public string moveicon { get; set; } = "";
        public FormattedValue<DateTime> lastmovedatetime { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);

        public bool isdamaged { get; set; } = false;
        public FormattedValue<int> is_empty { get; set; } = new FormattedValue<int>();
        public FormattedValue<int> status_code { get; set; } = new FormattedValue<int>();

        public TableCounts rowcount { get; set; } = new TableCounts();
      
       
    }

    public class ContainerDetailDTO
    {
        public EncryptedData containerid { get; set; } = new EncryptedData();
        public string containeruuid { get; set; } = "";
        public string equipmentno { get; set; } = "";
        public string containermodeluuid { get; set; } = "";
        //public FormattedValue<DateTime> lastbookingdate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public FormattedValue<DateTime> manufacturedate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public string sizename { get; set; } = "";
        public string model_iso_code { get; set; } = "";

        public string type_name { get; set; } = "";
        public string locationname { get; set; } = "";
        public string operatorname { get; set; }
        public string bookingno { get; set; } = "";
        public string bookinguuid { get; set; } = "";
        public decimal ageinyears { get; set; } = 0;
        public string agetext { get; set; } = "";
        public string lastmove { get; set; } = "";
        public string moveicon { get; set; } = "";
        public FormattedValue<DateTime> lastmovedatetime { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);

        public FormattedValue<int> is_empty { get; set; } = new FormattedValue<int>();
        public FormattedValue<int> status_code { get; set; } = new FormattedValue<int>();
    }
}