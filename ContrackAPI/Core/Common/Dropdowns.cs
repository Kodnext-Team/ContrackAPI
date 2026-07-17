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
                    "SELECT * FROM masters.moves_list('" + 1 + "','" + 2 + "');"
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
                        "p_hubid := '" + 1 + "'," +
                        "p_filters := '{}'," +
                        "p_userid := '" + 2 + "'" +
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
                tbl = Db.GetDataTable("SELECT * FROM  masters.moves_list('" + 1 + "','" + 2 + "');");

                result = (from DataRow dr in tbl.Rows
                          select new MovesDTO()
                          {
                              MovesName = Common.ToString(dr["movesname"]),
                              MovesId = new EncryptedData()
                              {
                                  EncryptedValue = Common.Encrypt(Common.ToInt(dr["movesid"]))
                              },
                              ShowVoyage = Common.ToBool(dr["showvoyage"]),
                              DontShowPublic = Common.ToBool(dr["dontshowpublic"]),
                              IconPath = Common.GetIconPath(Common.ToInt(dr["iconid"])),
                              SelectedIcon = Common.GetSelectedIconPath(Common.ToInt(dr["iconid"])),
                              InOut = Common.ToString(dr["in_out"]),
                              MovesUuid = Common.ToString(dr["movesuuid"]),
                              FullEmptyNone = Common.ToString(dr["full_empty_none"]),


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
        public static List<DropdownItem> GetPortDropdown(string countryid = "", bool showempty = false)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            using (SqlDB Db = new SqlDB())
            {
                DataTable tbl = new DataTable();
                if (countryid == "")
                    tbl = Db.GetDataTable("select * from masters.getportlist('" + 1 + "')");
                else
                    tbl = Db.GetDataTable("select * from masters.getportlist('" + 1 + "')");
                result = (from DataRow dr in tbl.Rows
                          select new DropdownItem()
                          {
                              Text = Common.ToString(dr["portname"]),
                              Value = Common.Encrypt(Common.ToInt(dr["portid"]))
                          }).ToList();
            }
            if (showempty)
                result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            return result;
        }
        public static List<DropdownItem> GetAgenciesUUIDDropdown(bool multiple = true)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            using (SqlDB Db = new SqlDB())
            {
                DataTable tbl = new DataTable();
                tbl = Db.GetDataTable("SELECT * FROM  masters.getagencylist('" + 1 + "','" + 2 + "');");
                result = (from DataRow dr in tbl.Rows
                          select new DropdownItem()
                          {
                              Text = Common.ToString(dr["agencyname"]),
                              Value = Common.ToString(dr["uuid"]),
                          }).ToList();
            }
            if (!multiple)
                result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            return result;
        }
        public static List<DropdownItem> GetClientsByUserIDDropdown(bool multiple = false, bool returnid = false)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            using (SqlDB Db = new SqlDB())
            {
                DataTable tbl = new DataTable();
                tbl = Db.GetDataTable("SELECT * FROM  masters.getclientlist_userid(" + 1 + "," + 2 + ");");
                result = (from DataRow dr in tbl.Rows
                          select new DropdownItem()
                          {
                              Text = Common.ToString(dr["clientname"]),
                              Value = returnid ? Common.Encrypt(Common.ToInt(dr["clientid"])) : Common.ToString(dr["clientuuid"])
                          }).ToList();
            }
            if (multiple)
                result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            return result;
        }
        public static List<DropdownItem> GetLoginUsersByRole(string RoleEncrypted, string AgencyEncrypted, bool showselect = true, bool encrypt = true)
        {
            List<DropdownItem> result = new List<DropdownItem>();

            UserFilter login = new UserFilter
            {
                limit = -1,
                Role = RoleEncrypted,
                EntityID = new List<string> { AgencyEncrypted }
            };

            var repo = new LoginRepository();
            var list = repo.GetUserLoginList(login);

            result = list.Select(dr => new DropdownItem
            {
                Text = dr.Name,
                Value = encrypt
                    ? Common.Encrypt(Common.ToInt(dr.UserID.NumericValue))
                    : dr.UserID.NumericValue.ToString()
            }).ToList();

            if (showselect)
                result.Insert(0, new DropdownItem { Text = "-Select-", Value = "" });

            return result;
        }
        public static List<DropdownItem> GetStatusDropdown(int type, bool showall = true)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            using (SqlDB Db = new SqlDB())
            {
                DataTable tbl = Db.GetDataTable("SELECT * FROM procurement.get_status_list_by_type(" +
                    "p_hubid := " + Common.HubID + ", " +
                    "p_type_id := " + type +
                ");");

                if (tbl != null && tbl.Rows.Count > 0)
                {
                    result = (from DataRow dr in tbl.Rows
                              select new DropdownItem()
                              {
                                  Value = Common.ToString(dr["status_id"]),
                                  Text = Common.ToString(dr["status_name"])
                              }).ToList();
                }
            }
            if (showall)
                result.Insert(0, new DropdownItem() { Text = "- All Statuses -", Value = "" });
            return result;
        }

       
        public static List<DropdownItem> GetVesselDropdownSearch(string search, string multiple = "")
        {
            List<DropdownItem> result = new List<DropdownItem>();
            using (SqlDB Db = new SqlDB())
            {
                DataTable tbl = new DataTable();
                tbl = Db.GetDataTable("SELECT * FROM  masters.getvessellist_userid(" + 1 + "," + 2 + ",'" + search + "');");
                result = (from DataRow dr in tbl.Rows
                          select new DropdownItem()
                          {
                              Text = Common.ToString(dr["vesselname"]),
                              Value = Common.Encrypt(Common.ToInt(dr["vesseldetailid"]))
                          }).ToList();
            }
            if (multiple == "")
                result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            return result;
        }
        public static List<DropdownItem> GetVesselDropdown(string AgencyDetailID, string search, string multiple = "", bool useuuid = false)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            using (SqlDB Db = new SqlDB())
            {
                DataTable tbl = new DataTable();
                tbl = Db.GetDataTable("SELECT * FROM  masters.getvessellist(" + Common.HubID + "," + Common.Decrypt(AgencyDetailID) + ",'" + search + "');");
                result = (from DataRow dr in tbl.Rows
                          select new DropdownItem()
                          {
                              Text = Common.ToString(dr["vesselname"]),
                              Value = useuuid ? Common.ToString(dr["assignmentuuid"]) : Common.Encrypt(Common.ToInt(dr["vesseldetailid"]))
                          }).ToList();
            }
            if (multiple == "")
                result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            return result;
        }
        public static List<DropdownItem> GetContainerTypesDropdown(bool showempty = true)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
            {
                DataTable tbl = Db.GetDataTable(
                    "SELECT * FROM masters.container_type_list_simple(" +
                    "p_hubid := '" + 1 + "'" + ");");
                result = (from DataRow dr in tbl.Rows
                          select new DropdownItem()
                          {
                              Text = Common.ToString(dr["typename"]),
                              Value = Common.Encrypt(Common.ToInt(dr["containertypeid"]))
                          }).ToList();
            }
            if (showempty)
                result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            return result;
        }

        public static List<DropdownItem> GetContainerSizesDropdown(bool showempty = true)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
            {
                DataTable tbl = Db.GetDataTable(
                    "SELECT * FROM masters.container_size_list();");
                result = (from DataRow dr in tbl.Rows
                          select new DropdownItem()
                          {
                              Text = Common.ToString(dr["sizename"]),
                              Value = Common.Encrypt(Common.ToInt(dr["sizeid"]))
                          }).ToList();
            }
            if (showempty)
                result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            return result;
        }

        public static List<DropdownItem> GetContainerModelsDropdown(bool showempty = true)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                        "SELECT * FROM masters.container_type_list(" +
                        "p_hubid := " + 1 +
                        ");");

                    result = (from DataRow dr in tbl.Rows
                              where Common.ToInt(dr["modelid"]) > 0
                              select new DropdownItem()
                              {
                                  Text = $"{Common.ToString(dr["iso_code"])} - {Common.ToString(dr["sizename"])} {Common.ToString(dr["typename"])}",
                                  Value = Common.ToString(dr["modeluuid"])
                              }).OrderBy(x => x.Text).ToList();
                }

                if (showempty)
                    result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public static List<DropdownItem> GetLocationUUIDDropdown(bool showempty = true)
        {
            List<DropdownItem> result = new List<DropdownItem>();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                        "SELECT * FROM masters.container_location_list(" +
                        "p_hubid := '" + 1 + "'," +
                        "p_filters := '{}'," +
                        "p_userid := '" +2 + "'" +
                        ");");

                    result = (from DataRow dr in tbl.Rows
                              select new DropdownItem()
                              {
                                  Text = $"{Common.ToString(dr["locationname"])} ({Common.ToString(dr["locationcode"])})",
                                  Value = Common.ToString(dr["locationuuid"])
                              }).ToList();
                }

                if (showempty)
                    result.Insert(0, new DropdownItem() { Text = "-Select-", Value = "" });
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public static List<DropdownItem> GetContainerOperatorDropdown()
        {
            // 1=Owned, 2=Leased, 3=SOC, 4=COC
            var list = new List<DropdownItem>
        {
            new DropdownItem { Text = "Owned", Value = Common.Encrypt(1) },
            new DropdownItem { Text = "Leased", Value = Common.Encrypt(2) },
            new DropdownItem { Text = "SOC", Value = Common.Encrypt(3) },
            new DropdownItem { Text = "COC", Value = Common.Encrypt(4) }
        };
            list.Insert(0, new DropdownItem { Text = "-Select-", Value = "" });
            return list;
        }
        public static List<DropdownItem> GetContainerStatusDropdown()
        {
            // Static values: 1 = Available, 2 = Booked, 3 = Damaged
            var list =  new List<DropdownItem>
            {
                new DropdownItem { Text = "Available", Value = "1" },
                new DropdownItem { Text = "Booked",    Value = "2" },
                new DropdownItem { Text = "Damaged",   Value = "3" }
            };
            list.Insert(0, new DropdownItem { Text = "-Select-", Value = "" });
            return list;

        }
    }
}
