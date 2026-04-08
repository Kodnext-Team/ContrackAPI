using System.Data;

namespace ContrackAPI
{
    public class HubRepository : CustomException, IHubRepository
    {
        public HubDTO GetHubByID(int HubID)
        {
            HubDTO hub = new HubDTO();
            try
            {
                using (SqlDB Db = new SqlDB())
                {
                    DataTable tbl = Db.GetDataTable("SELECT * FROM  masters.gethubbyid('" + HubID + "');");
                    if (tbl.Rows.Count != 0)
                    {
                        hub.hubid = HubID;
                        hub.uuid = Common.ToString(tbl.Rows[0]["uuid"]);
                        hub.hubname = Common.ToString(tbl.Rows[0]["hubname"]);
                        hub.imono = Common.ToString(tbl.Rows[0]["imono"]);
                        hub.address = Common.ToString(tbl.Rows[0]["address"]);
                        hub.email = Common.ToString(tbl.Rows[0]["email"]);
                        hub.phone = Common.ToString(tbl.Rows[0]["phone"]);
                        hub.hcreatedt = Common.ToDateTime(tbl.Rows[0]["hcreatedt"]);
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return hub;
        }
    }

}
