using Newtonsoft.Json;
using System.Data;

namespace ContrackAPI
{
    public class ContainerRepository : CustomException, IContainerRepository
    {
        public List<ContainerDTO> GetContainerList(ContainerFilterPage filter)
        {
            List<ContainerDTO> list = new List<ContainerDTO>();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    string jsonFilters = JsonConvert.SerializeObject(filter);

                    string query = "SELECT * FROM masters.container_equip_list(" +
                                   "p_hubid := " + Common.HubID + "," +
                                   "p_filters := '" + Common.Escape(jsonFilters) + "'::jsonb," +
                                   "p_userid := " + Common.UserID +");";
                    DataTable tbl = Db.GetDataTable(query);
                    if (tbl != null)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            list.Add(ParseContainerList(dr));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return list;
        }
        private ContainerDTO ParseContainerList(DataRow dr)
        {
            var formattedAge = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(dr["manufacturedate"]));
            if (!string.IsNullOrEmpty(formattedAge.SubText))
            {
                formattedAge.SubText = formattedAge.SubText.Replace("ago", "old");
            }
            var formattedLastBooking = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(dr["lastbookingdate"]));
            if (!string.IsNullOrEmpty(formattedLastBooking.SubText))
            {
                formattedLastBooking.SubText = formattedLastBooking.SubText.Replace("ago", "");
            }
            return new ContainerDTO()
            {
                rowcount = new TableCounts
                {
                    row_index = Common.ToInt(dr["row_index"]),
                    totalnoofrows = Common.ToInt(dr["total_count"])
                },
                containerid = new EncryptedData()
                {
                    NumericValue = Common.ToInt(dr["containerid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["containerid"]))
                },
                containeruuid = Common.ToString(dr["containeruuid"]),
                equipmentno = Common.ToString(dr["equipmentno"]),
                model_iso_code = Common.ToString(dr["model_iso_code"]),
                sizename = Common.ToString(dr["sizename"]),
                type_name = Common.ToString(dr["typename"]),
                operatorname = Common.GetOperatorName(Common.ToInt(dr["operatorid"])),
                locationname = Common.ToString(dr["locationname"]),
                portname = Common.ToString(dr["portname"]),
                countryflag = !string.IsNullOrEmpty(Common.ToString(dr["countryflag"])) ? Common.FlagFolder + Common.ToString(dr["countryflag"]) : "",
                bookingid = new EncryptedData()
                {
                    NumericValue = Common.ToInt(dr["bookingid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["bookingid"]))
                },
                bookingno = Common.ToString(dr["bookingno"]),
                bookinguuid = Common.ToString(dr["bookinguuid"]),
                customername = Common.ToString(dr["customername"]),
                islive = Common.ToBool(dr["islive"]),
                isdamaged = dr.Table.Columns.Contains("isdamaged") ? Common.ToBool(dr["isdamaged"]) : false,
                isblocked = dr.Table.Columns.Contains("isblocked") ? Common.ToBool(dr["isblocked"]) : false,
                polname = Common.ToString(dr["pol_portname"]),
                polcode = Common.ToString(dr["pol_portcode"]),
                pol_flag = !string.IsNullOrEmpty(Common.ToString(dr["pol_flag"])) ? Common.FlagFolder + Common.ToString(dr["pol_flag"]) : "",
                podcode = Common.ToString(dr["pod_portcode"]),
                podname = Common.ToString(dr["pod_portname"]),
                pod_flag = !string.IsNullOrEmpty(Common.ToString(dr["pod_flag"])) ? Common.FlagFolder + Common.ToString(dr["pod_flag"]) : "",
                manufacturedate = formattedAge,
                lastbookingdate = formattedLastBooking,
                locationtypename = Common.ToString(dr["locationtypename"]),
                //locationicon = Common.GetIconPath(Common.ToInt(dr["locationtypeiconid"])),
                //moveicon = Common.GetSelectedIconPath(Common.ToInt(dr["moveiconid"])),
                lastmove = Common.ToString(dr["movesname"]),
                is_empty = Common.ToBool(dr["is_empty"]),
                status_code = Common.ToInt(dr["status_code"]),
                lastmovedatetime = Common.ToDateTimeString(Common.ToDateTime(dr["lastmovedatetime"]), Common.HumanDateTimeformat),
                containercreatedat = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(dr["containercreatedat"]))
            };
        }
        public ContainerDTO GetContainerByUUID(string containeruuid)
        {
            ContainerDTO model = new ContainerDTO();
            try
            {
                if (string.IsNullOrEmpty(containeruuid)) return model;
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable("SELECT * FROM masters.container_equip_get_byuuid('" + containeruuid + "'," + Common.HubID + ");");
                    if (tbl != null && tbl.Rows.Count > 0)
                        model = ParseContainerDetail(tbl.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return model;
        }
    
    private ContainerDTO ParseContainerDetail(DataRow dr)
        {
            var formattedAge = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(dr["manufacturedate"]));
            if (!string.IsNullOrEmpty(formattedAge.SubText))
            {
                formattedAge.SubText = formattedAge.SubText.Replace("ago", "old");
            }
            return new ContainerDTO()
            {
                containerid = new EncryptedData()
                {
                    NumericValue = Common.ToInt(dr["containerid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["containerid"]))
                },
                operatorid = new EncryptedData()
                {
                    NumericValue = Common.ToInt(dr["operatorid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["operatorid"]))
                },

                currentlocationid = new EncryptedData()
                {
                    NumericValue = Common.ToInt(dr["locationdetailid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["locationdetailid"]))
                },
                containeruuid = Common.ToString(dr["containeruuid"]),
                equipmentno = Common.ToString(dr["equipmentno"]),
                containermodeluuid = Common.ToString(dr["containermodeluuid"]),
                model_iso_code = Common.ToString(dr["model_iso_code"]),
                sizename = Common.ToString(dr["sizename"]),
                type_name = Common.ToString(dr["typename"]),
                operatorname = Common.GetOperatorName(Common.ToInt(dr["operatorid"])),
                locationname = Common.ToString(dr["locationname"]),
                portname = Common.ToString(dr["portname"]),
                islive = Common.ToBool(dr["islive"]),
                manufacturedate = formattedAge,
                bookingid = new EncryptedData()
                {
                    NumericValue = Common.ToInt(dr["bookingid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["bookingid"]))
                },
                is_empty = dr.Table.Columns.Contains("is_empty") ? Common.ToBool(dr["is_empty"]) : false,
                ageinyears = formattedAge.NumericValue != 0 ? Math.Abs(formattedAge.NumericValue / 365) : 0,
                //moveicon = Common.GetSelectedIconPath(Common.ToInt(dr["moveiconid"])),
                lastmove = Common.ToString(dr["movesname"]),
                status_code = Common.ToInt(dr["status_code"]),
                bookingno = Common.ToString(dr["bookingno"]),
                bookinguuid = Common.ToString(dr["bookinguuid"]),
                lastmovedatetime = Common.ToDateTimeString(Common.ToDateTime(dr["lastmovedatetime"]), Common.HumanDateTimeformat),
            };
        }
    }
}
