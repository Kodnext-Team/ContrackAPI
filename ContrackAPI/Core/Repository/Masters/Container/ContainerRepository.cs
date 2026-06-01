using Newtonsoft.Json;
using System.Data;
using System.Reflection;

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
                                   "p_hubid := " + 1 + "," +
                                   "p_filters := '" + Common.Escape(jsonFilters) + "'::jsonb," +
                                   "p_userid := " + 2 +");";
                    DataTable tbl = Db.GetDataTable(query);
                    if (tbl == null || tbl.Rows.Count == 0)
                        return new List<ContainerDTO>();
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
                throw;
            }
            return list;
        }
        private ContainerDTO ParseContainerList(DataRow dr)
        {           
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
                //lastbookingdate = formattedLastBooking,
                locationtypename = Common.ToString(dr["locationtypename"]),
                locationicon = Common.GetIconPath(Common.ToInt(dr["locationtypeiconid"])),
                moveicon = Common.GetSelectedIconPath(Common.ToInt(dr["moveiconid"])),
                lastmove = Common.ToString(dr["movesname"]),
                isdamaged = dr.Table.Columns.Contains("isdamaged") ? Common.ToBool(dr["isdamaged"]) : false,
                is_empty = FormatConvertor.ToEmptyFull(Common.ToBool(dr["is_empty"])),
                status_code = FormatConvertor.ToContainerStatus(Common.ToInt(dr["status_code"])),            
                lastmovedatetime = FormatConvertor.ToClientDateTimeFormat(Common.ToDateTime(dr["lastmovedatetime"])),
            };
        }
        public ContainerDetailDTO GetContainerByUUID(string containeruuid)
        {
            ContainerDetailDTO model = new ContainerDetailDTO();
            try
            {
                if (string.IsNullOrEmpty(containeruuid)) return model;
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable("SELECT * FROM masters.container_equip_get_byuuid('" + containeruuid + "'," + 1 + ");");
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
    
    private ContainerDetailDTO ParseContainerDetail(DataRow dr)
        {
            var formattedAge = FormatConvertor.ToDateTimeFormat(Common.ToDateTimeOff(dr["manufacturedate"]));
            if (!string.IsNullOrEmpty(formattedAge.SubText))
            {
                formattedAge.SubText = formattedAge.SubText.Replace("ago", "old");
            }
            int ageInYears = formattedAge.NumericValue != 0 ? Math.Abs(formattedAge.NumericValue / 365) : 0;
            return new ContainerDetailDTO()
            {
                containerid = new EncryptedData()
                {
                    NumericValue = Common.ToInt(dr["containerid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["containerid"]))
                },
                containeruuid = Common.ToString(dr["containeruuid"]),
                equipmentno = Common.ToString(dr["equipmentno"]),
                containermodeluuid = Common.ToString(dr["containermodeluuid"]),
                type_name = Common.ToString(dr["typename"]),
                operatorname = Common.GetOperatorName(Common.ToInt(dr["operatorid"])),
                locationname = Common.ToString(dr["locationname"]),
                model_iso_code = Common.ToString(dr["model_iso_code"]),
                sizename = Common.ToString(dr["sizename"]),
                manufacturedate = formattedAge,
                is_empty = FormatConvertor.ToEmptyFull(Common.ToBool(dr["is_empty"])),
                status_code = FormatConvertor.ToContainerStatus(Common.ToInt(dr["status_code"])),
                ageinyears = ageInYears,
                agetext = Common.GetAgeGrade(ageInYears),
                moveicon = Common.GetSelectedIconPath(Common.ToInt(dr["moveiconid"])),
                lastmove = Common.ToString(dr["movesname"]),
                bookingno = Common.ToString(dr["bookingno"]),
                bookinguuid = Common.ToString(dr["bookinguuid"]),
                lastmovedatetime = FormatConvertor.ToClientDateTimeFormat(Common.ToDateTime(dr["lastmovedatetime"]))
            };
        }

        public List<ContainerEquipmentDTO> GetContainerByEquipmentno(string equipmentno)
        {
            List<ContainerEquipmentDTO> result = new List<ContainerEquipmentDTO>();

            try
            {
                if (string.IsNullOrEmpty(equipmentno))
                    return result;

                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                        "SELECT * FROM masters.container_equip_get_byequipmentno('" +
                        Common.Escape(equipmentno) + "'," + 1 + ");");

                    if (tbl != null && tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            result.Add(ParseContainerEquipment(dr));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }

            return result;
        }
        private ContainerEquipmentDTO ParseContainerEquipment(DataRow dr)
        {          
            return new ContainerEquipmentDTO()
            {
                containerid = new EncryptedData()
                {
                    NumericValue = Common.ToInt(dr["containerid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["containerid"]))
                },
                containeruuid = Common.ToString(dr["containeruuid"]),
                equipmentno = Common.ToString(dr["equipmentno"]),
                type_name = Common.ToString(dr["typename"]),
                operatorname = Common.GetOperatorName(Common.ToInt(dr["operatorid"])),
                model_iso_code = Common.ToString(dr["model_iso_code"]),
                sizename = Common.ToString(dr["sizename"]),
                
            };
        }
    }
}
