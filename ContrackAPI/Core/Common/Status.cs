using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class Status
    {
        public int statusid { get; set; } = 0;
        public string statusname { get; set; } = "";
        public string description { get; set; } = "";
        public int type_id { get; set; } = 0;
        public string type_desc { get; set; } = "";
        public string styling { get; set; } = "";
        public static List<Status> GetStatus()
        {
            List<Status> result = new List<Status>();
            try
            {

                using (SqlDB Db = new SqlDB())
                {
                    DataTable tbl = Db.GetDataTable("SELECT * FROM procurement.status_master");

                    result = (from DataRow dr in tbl.Rows
                              select new Status()
                              {
                                  statusid = Common.ToInt(dr["statusid"]),
                                  statusname = Common.ToString(dr["statusname"]),
                                  description = Common.ToString(dr["description"]),
                                  type_id = Common.ToInt(dr["type_id"]),
                                  type_desc = Common.ToString(dr["type_desc"]),
                                  styling = Common.ToString(dr["styling"]),
                              }).ToList();

                }
            }
            catch (Exception ex)
            { }
            return result;
        }

        //public static string GetStatusValue(int type, string statusdesc)
        //{
        //    string output = statusdesc;
        //    try
        //    {
        //        var allstatus = SessionManager.AllStatus;
        //        if (allstatus == null)
        //        {
        //            allstatus = GetStatus();
        //            SessionManager.AllStatus = allstatus;
        //        }
        //        var selectedstatus = allstatus
        //                             .Where(x => x.type_id == type && x.statusname == statusdesc)
        //                            .FirstOrDefault();
        //        if (selectedstatus != null)
        //        {
        //            output = selectedstatus.styling.Replace("[StatusName]", selectedstatus.statusname);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return output;
        //}

        //public static List<int> GetStatusList(int type)
        //{
        //    List<int> output = new List<int>();
        //    try
        //    {
        //        var allstatus = SessionManager.AllStatus;
        //        if (allstatus == null)
        //        {
        //            allstatus = GetStatus();
        //            SessionManager.AllStatus = allstatus;
        //        }
        //        output = allstatus
        //                             .Where(x => x.type_id == type)
        //                            .Select(x => x.statusid).ToList();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return output;
        //}

        //public static string GetStatusValueByID(int type, int statusid)
        //{
        //    string output = "";
        //    try
        //    {
        //        var allstatus = SessionManager.AllStatus;
        //        if (allstatus == null)
        //        {
        //            allstatus = GetStatus();
        //            SessionManager.AllStatus = allstatus;
        //        }
        //        var selectedstatus = allstatus
        //                             .Where(x => x.type_id == type && x.statusid == statusid)
        //                            .FirstOrDefault();
        //        if (selectedstatus != null)
        //        {
        //            output = selectedstatus.styling.Replace("[StatusName]", selectedstatus.statusname);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return output;
        //}
        //public static string GetStatusValue(int type, int statusid)
        //{
        //    string output = "";
        //    try
        //    {
        //        var allstatus = SessionManager.AllStatus;
        //        if (allstatus == null)
        //        {
        //            allstatus = GetStatus();
        //            SessionManager.AllStatus = allstatus;
        //        }
        //        var selectedstatus = allstatus
        //                             .Where(x => x.type_id == type && x.statusid == statusid)
        //                            .FirstOrDefault();
        //        if (selectedstatus != null)
        //        {
        //            output = selectedstatus.statusname;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return output;
        //}
    }
}