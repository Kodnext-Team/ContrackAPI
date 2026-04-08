using ContrackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public EncryptedData operatorid { get; set; } = new EncryptedData();
        public EncryptedData currentlocationid { get; set; } = new EncryptedData();
        public string containermodeluuid { get; set; } = "";
        public FormattedValue<DateTime> lastbookingdate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public FormattedValue<DateTime> manufacturedate { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public FormattedValue<DateTime> containercreatedat { get; set; } = FormatConvertor.ToDateTimeFormat(DateTime.MinValue);
        public string model_iso_code { get; set; } = "";
        public string sizename { get; set; } = "";
        public string type_name { get; set; } = "";
        public string locationname { get; set; } = "";
        public string portname { get; set; } = "";
        public string countryflag { get; set; } = "";
        public string operatorname { get; set; }
        public EncryptedData bookingid { get; set; } = new EncryptedData();
        public string bookingno { get; set; } = "";
        public string bookinguuid { get; set; } = "";
        public string customername { get; set; } = "";
        public string polname { get; set; } = "";
        public string podname { get; set; } = "";
        public string pol_flag { get; set; } = "";
        public string pod_flag { get; set; } = "";
        public bool islive { get; set; } = true;
        public bool isdamaged { get; set; } = false;
        public bool isblocked { get; set; }
        public string locationicon { get; set; } = "";
        public string locationtypename { get; set; } = "";
        public decimal ageinyears { get; set; } = 0;
        public string lastmove { get; set; } = "";
        public string moveicon { get; set; } = "";
        public string lastmovedatetime { get; set; } = "";
        public TableCounts rowcount { get; set; } = new TableCounts();
        public string polcode { get; set; } = ""; // Added
        public string podcode { get; set; } = ""; // Added
        public bool? is_empty { get; set; }
        public int status_code { get; set; } = 0;
    }
}