using ContrackAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class CommonRepository : CustomException, ICommonRepository
    {
        public List<IconDTO> GetIcons()
        {
            List<IconDTO> list = new List<IconDTO>();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                        "SELECT * FROM masters.icons where hubid='" + Common.HubID + "'");

                    list = (from DataRow dr in tbl.Rows
                            select new IconDTO()
                            {
                                icon = Common.ToString(dr["icon"]),
                                IconId = Common.ToInt(dr["iconid"]),
                                type = Common.ToInt(dr["type"]),
                                iconselected = Common.ToString(dr["iconselected"]),
                                iscss = Common.ToBool(dr["iscss"]),
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return list;
        }
    }
}