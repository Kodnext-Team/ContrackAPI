using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ContrackAPI
{
    public class ContainerTypeRepository : CustomException, IContainerTypeRepository
    {
        public Result SaveContainerType(ContainerTypeDTO type)
        {
            Result result = new Result();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    string query = "SELECT * FROM masters.container_type_save(" +
                                   "p_containertypeid := '" + Common.Decrypt(type.containertypeid.EncryptedValue) + "'," +
                                   "p_hub_id := '" + Common.HubID + "'," +
                                   "p_typename := '" + Common.Escape(type.typename) + "'," +
                                   "p_icon := '" + Common.Decrypt(type.icon.iconid.EncryptedValue) + "'," +
                                   "p_user_id := '" + Common.UserID + "'" +
                                   ");";

                    DataTable tbl = Db.GetDataTable(query);

                    if (tbl.Rows.Count > 0)
                    {
                        string resultId = tbl.Rows[0][0].ToString();
                        string message = tbl.Rows[0][1].ToString();
                        if (resultId == "1")
                        {
                            result = Common.SuccessMessage(message);
                            result.TargetID = Convert.ToInt32(tbl.Rows[0][2]);
                        }
                        else
                        {
                            result = Common.ErrorMessage(message);
                        }
                    }
                    else
                    {
                        result = Common.ErrorMessage("Cannot save container type");
                    }
                }
            }
            catch (Exception ex)
            {
                result = Common.ErrorMessage(ex.Message);
                RecordException(ex);
            }
            return result;
        }

        public Result DeleteContainerType(string containertypeid)
        {
            Result result = new Result();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    string query = "SELECT * FROM masters.container_type_delete(" +
                                    "p_hub_id := '" + Common.HubID + "'," +
                                    "p_containertypeid := '" + Common.Decrypt(containertypeid) + "'," +
                                    "p_deletedby := '" + Common.UserID + "'" +
                                    ");";

                    DataTable tbl = Db.GetDataTable(query);

                    if (tbl.Rows.Count > 0)
                    {
                        if (tbl.Rows[0][0].ToString() == "1")
                            result = Common.SuccessMessage(tbl.Rows[0][1].ToString());
                        else
                            result = Common.ErrorMessage(tbl.Rows[0][1].ToString());
                    }
                    else
                        result = Common.ErrorMessage("Cannot delete container type");
                }
            }
            catch (Exception ex)
            {
                result = Common.ErrorMessage(ex.Message);
                RecordException(ex);
            }
            return result;
        }

        public List<ContainerTypeDTO> GetContainerTypesList()
        {
            List<ContainerTypeDTO> list = new List<ContainerTypeDTO>();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                    "SELECT * FROM masters.container_type_list_simple(" +
                    "p_hubid := '" + 1 + "'" + ");");

                    list = (from DataRow dr in tbl.Rows
                            select new ContainerTypeDTO()
                            {
                                containertypeid = new EncryptedData()
                                {
                                    NumericValue = Common.ToInt(dr["containertypeid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["containertypeid"]))
                                },
                                typeuuid = Common.ToString(dr["typeuuid"]),
                                typename = Common.ToString(dr["typename"]),
                                typeshortname = Common.ToString(dr["typeshortname"]),
                                createdat = Common.ToDateTime(dr["createdat"]),
                                icon = new Icon()
                                {
                                    iconid = new EncryptedData()
                                    {
                                        NumericValue = Common.ToInt(dr["iconid"]),
                                        EncryptedValue = Common.Encrypt(Common.ToInt(dr["iconid"]))
                                    },
                                    icon = Common.ToString(dr["icon"]),
                                    type = new EncryptedData()
                                    {
                                        NumericValue = Common.ToInt(dr["type"]),
                                        EncryptedValue = Common.Encrypt(Common.ToInt(dr["type"]))
                                    }
                                }
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return list;
        }

        public ContainerTypeDTO GetContainerTypeByID(string typeid)
        {
            ContainerTypeDTO type = new ContainerTypeDTO();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                         "SELECT * FROM masters.container_type_get_byid(" +
                         "p_hubid := '" + Common.HubID + "'," +
                         "p_containertypeid := '" + Common.Decrypt(typeid) + "'" +
                         ");"
                     );

                    if (tbl.Rows.Count > 0)
                    {
                        DataRow dr = tbl.Rows[0];

                        AdditionalData additionalData = new AdditionalData();
                        if (dr["addtionaldata"] != DBNull.Value)
                        {
                            var json = Common.ToString(dr["addtionaldata"]);
                            if (!string.IsNullOrEmpty(json))
                            {
                                additionalData = Newtonsoft.Json.JsonConvert.DeserializeObject<AdditionalData>(json);
                            }
                        }
                        type = new ContainerTypeDTO()
                        {
                            containertypeid = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["containertypeid"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["containertypeid"]))
                            },
                            typeuuid = Common.ToString(dr["typeuuid"]),
                            typename = Common.ToString(dr["typename"]),
                            typeshortname = Common.ToString(dr["typeshortname"]),
                            createdat = Common.ToDateTime(dr["createdat"]),
                            addtionaldata = additionalData,
                            icon = new Icon()
                            {
                                iconid = new EncryptedData()
                                {
                                    NumericValue = Common.ToInt(dr["iconid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["iconid"]))
                                },
                                icon = Common.ToString(dr["icon"]),
                                type = new EncryptedData()
                                {
                                    NumericValue = Common.ToInt(dr["type"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["type"]))
                                }
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return type;
        }

        public ContainerTypeDTO GetContainerTypeByUUID(string uuid)
        {
            ContainerTypeDTO type = new ContainerTypeDTO();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                            "SELECT * FROM masters.container_type_get_byuuid(" +
                            "p_hubid := '" + Common.HubID + "'," +
                            "p_typeuuid := " + Common.GetUUID(uuid) +
                            ");"
                        );

                    if (tbl.Rows.Count > 0)
                    {
                        DataRow dr = tbl.Rows[0];

                        AdditionalData additionalData = new AdditionalData();
                        if (dr["addtionaldata"] != DBNull.Value)
                        {
                            var json = Common.ToString(dr["addtionaldata"]);
                            if (!string.IsNullOrEmpty(json))
                            {
                                additionalData = Newtonsoft.Json.JsonConvert.DeserializeObject<AdditionalData>(json);
                            }
                        }
                        type = new ContainerTypeDTO()
                        {
                            containertypeid = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["containertypeid"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["containertypeid"]))
                            },
                            typeuuid = Common.ToString(dr["typeuuid"]),
                            typename = Common.ToString(dr["typename"]),
                            typeshortname = Common.ToString(dr["typeshortname"]),
                            createdat = Common.ToDateTime(dr["createdat"]),
                            addtionaldata = additionalData,
                            icon = new Icon()
                            {
                                iconid = new EncryptedData()
                                {
                                    NumericValue = Common.ToInt(dr["iconid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["iconid"]))
                                },
                                icon = Common.ToString(dr["icon"]),
                                type = new EncryptedData()
                                {
                                    NumericValue = Common.ToInt(dr["type"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["type"]))
                                }
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return type;
        }
    }
}