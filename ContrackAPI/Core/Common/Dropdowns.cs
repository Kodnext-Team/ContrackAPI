using System.Data;

namespace ContrackAPI
{
    public class DropdownItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public class Dropdowns
    {
        public static List<DropdownItem> GetMovesDropdown(bool showempty = true)
        {
            List<DropdownItem> result = new List<DropdownItem>();

            using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
            {
                DataTable tbl = Db.GetDataTable(
                    "SELECT * FROM masters.moves_list('" + Common.HubID + "','" + Common.UserID + "');"
                );

                result = (from DataRow dr in tbl.Rows
                          select new DropdownItem()
                          {
                              Text = Common.ToString(dr["movesname"]),
                              Value = Common.Encrypt(Common.ToInt(dr["movesid"]))
                          }).ToList();
            }

            if (showempty)
                result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });

            return result;
        }
        public static List<DropdownItem> GetLocationDropdown(bool showempty = true)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                        "SELECT * FROM masters.container_location_list(" +
                        "p_hubid := '" + Common.HubID + "'," +
                        "p_filters := '{}'," +
                        "p_userid := '" + Common.UserID + "'" +
                        ");"
                    );
                    result = (from DataRow dr in tbl.Rows
                              select new DropdownItem()
                              {
                                  Text = $"{Common.ToString(dr["locationname"])} ({Common.ToString(dr["locationcode"])})",
                                  Value = Common.Encrypt(Common.ToInt(dr["locationdetailid"]))
                              }).ToList();
                }
                if (showempty)
                    result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            }
            catch (Exception)
            {
            }

            return result;
        }
        public static List<MovesDTO> GetNewMovesDropdown(bool showempty = true)
        {
            List<MovesDTO> result = new List<MovesDTO>();
            using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
            {
                DataTable tbl = new DataTable();
                tbl = Db.GetDataTable("SELECT * FROM  masters.moves_list('" + Common.HubID + "','" + Common.UserID + "');");

                result = (from DataRow dr in tbl.Rows
                          select new MovesDTO()
                          {
                              MovesName = Common.ToString(dr["movesname"]),
                              MovesId = new EncryptedData()
                              {
                                  EncryptedValue = Common.Encrypt(Common.ToInt(dr["movesid"]))
                              },
                              ShowVoyage = Common.ToBool(dr["showvoyage"])
                          }).ToList();
            }
            if (showempty)
                result.Insert(0, new MovesDTO()
                {
                    MovesName = "-Select-",
                    MovesId = new EncryptedData()
                    {
                        EncryptedValue = ""
                    },
                    ShowVoyage = false
                });

            return result;
        }
        public static List<VoyageDTO> GetVoyageSearch(string search, bool createnew = true)
        {
            IVoyageRepository repo = new VoyageRepository();
            return repo.SearchVoyage(search, createnew);
        }
    }
}
