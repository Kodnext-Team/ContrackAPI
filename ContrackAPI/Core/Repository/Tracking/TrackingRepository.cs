using Newtonsoft.Json;
using System.Data;

namespace ContrackAPI
{
    public class TrackingRepository : CustomException, ITrackingRepository
    {
        public List<TrackingListDTO> GetTrackingList(TrackingFilterPage filter)
        {
            List<TrackingListDTO> list = new List<TrackingListDTO>();

            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    string jsonFilters = JsonConvert.SerializeObject(filter);

                    string query = "SELECT * FROM tracking.container_movement_tracking_list(" +
                                   "p_hubid := " + 1 + "," +
                                   "p_containeruuid := " + (string.IsNullOrEmpty(filter.ContainerUUID) ? "NULL" : $"'{filter.ContainerUUID}'::uuid") + "," +
                                   "p_filters := '" + Common.Escape(jsonFilters) + "'::jsonb," +
                                   "p_userid := " + 2 + ");";

                    DataTable tbl = Db.GetDataTable(query);
                    if (tbl == null || tbl.Rows.Count == 0)
                        return null;

                    foreach (DataRow dr in tbl.Rows)
                    {
                        list.Add(ParseTrackingList(dr));
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
        private TrackingListDTO ParseTrackingList(DataRow dr)
        {
            return new TrackingListDTO()
            {
                rowcount = new TableCounts
                {
                    row_index = Common.ToInt(dr["row_index"]),
                    totalnoofrows = Common.ToInt(dr["total_count"])
                },
                trackingid = new EncryptedData
                {
                    NumericValue = Common.ToInt(dr["trackingid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["trackingid"]))
                },
                trackinguuid = Common.ToString(dr["trackinguuid"]),
                recorddatetime = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(dr["recorddatetime"])),
                movesname = Common.ToString(dr["movesname"]),
                move_icon = Common.GetSelectedIconPath(Common.ToInt(dr["move_iconid"])),               
                bookingid = new EncryptedData
                {
                    NumericValue = Common.ToInt(dr["bookingid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["bookingid"]))
                },
                locationname = Common.ToString(dr["locationname"]),
                location_portcode = Common.ToString(dr["location_portcode"]),
                location_countryname = Common.ToString(dr["location_countryname"]),
                location_countryflag = dr.Table.Columns.Contains("location_countryflag") ? Common.ToString(dr["location_countryflag"]) : "",
                containerno = Common.ToString(dr["containerno"]),
                containersizetype = Common.ToString(dr["containersizetype"]),
                isempty = Common.ToBool(dr["isempty"]),
                isdamaged = Common.ToBool(dr["isdamaged"]),
                noofdamages = Common.ToInt(dr["noofdamages"]),
                bookingno = Common.ToString(dr["bookingno"]),
                customername = Common.ToString(dr["customername"]),
                nextmovename = Common.ToString(dr["nextmovename"]),
                nextlocationname = Common.ToString(dr["nextlocationname"]),
                nextdatetime = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(dr["nextdatetime"])),
            };
        }
        public Result SaveTracking(TrackingDTO tracking)
        {
            Result result = new Result();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable("SELECT * FROM tracking.container_movement_tracking_save(" +
                         "p_trackingid := " + Common.Decrypt(tracking.TrackingId.EncryptedValue) + "," +
                         "p_pickselectionuuid := " + Common.GetUUID(tracking.PickSelectionUuid) + "," +
                         "p_containerid := " + Common.Decrypt(tracking.containerid.EncryptedValue) + "," +
                         "p_bookingid := " + Common.Decrypt(tracking.bookingid.EncryptedValue) + "," +
                         "p_hubid := " + 1 + "," +
                         "p_movetype := " + Common.Decrypt(tracking.Moves.EncryptedValue) + "," +
                         "p_locationdetailid := " + Common.Decrypt(tracking.LocationDetailId.EncryptedValue) + "," +
                         "p_currentvoyageid := " + Common.Decrypt(tracking.CurrentVoyageId.EncryptedValue) + "," +
                         "p_recorddatetime := '" + tracking.RecordDateTime + "'," +
                         "p_nextmove := " + Common.Decrypt(tracking.NextMoves.EncryptedValue) + "," +
                         "p_nextlocationdetailid := " + Common.Decrypt(tracking.NextLocationDetailId.EncryptedValue) + "," +
                         "p_nextvoyageid := " + Common.Decrypt(tracking.NextVoyageId.EncryptedValue) + "," +
                         "p_nextdatetime := " + Common.GetNullValue(tracking.NextDateTime) + "," +
                         "p_isdamaged := " + Common.ToBool(tracking.IsDamaged) + "," +
                         "p_noofdamages := " + Common.ToInt(tracking.NoOfDamages) + "," +
                         "p_createdby := " + 2 + "" +
                     ");");

                    if (tbl.Rows.Count > 0)
                    {
                        if (tbl.Rows[0][0].ToString() == "1")
                        {
                            result = Common.SuccessMessage(tbl.Rows[0][1].ToString());
                        }
                        else
                        {
                            result = Common.ErrorMessage(tbl.Rows[0][1].ToString());
                        }
                    }
                    else
                    {
                        result = Common.ErrorMessage("Cannot Save Tracking.");
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
        public Result SavePickSelection(List<TrackingSelectionDTO> selections)
        {
            Result result = new Result();
            try
            {
                using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
                {
                    List<string> otherIds = new List<string>();

                    foreach (var item in selections)
                    {
                        var containerId = Common.Decrypt(item.ContainerId.EncryptedValue);

                        var bookingId = Common.Decrypt(item.BookingId.EncryptedValue);

                        otherIds.Add($"{containerId}|{bookingId}");
                    }

                    DataTable tbl = db.GetDataTable("SELECT * FROM sandbox.create_pick_selection(" +
                         "p_hubid := " + 1 + "," +
                         "p_uuids := NULL," +
                         "p_otherids := ARRAY['" + string.Join("','", otherIds) + "']::text[]," +
                         "p_createdby := " + 2 + ");"
                    );

                    if (tbl.Rows.Count != 0)
                    {
                        if (Common.ToInt(tbl.Rows[0][0]) > 0)
                        {
                            result = Common.SuccessMessage("Success");
                            result.TargetUUID = Convert.ToString(tbl.Rows[0][2].ToString());
                        }
                        else
                        {
                            result = Common.ErrorMessage(tbl.Rows[0][1].ToString());
                        }
                    }
                    else
                    {
                        result = Common.ErrorMessage("Cannot process request");
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
        public List<TrackingDetailsDTO> GetTrackingDetails(string containerUuid, string bookingUuid)
        {
            List<TrackingDetailsDTO> trackingdetails = new List<TrackingDetailsDTO>();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable("SELECT * FROM tracking.container_movement_tracking_get_by_uuid('" + containerUuid + "', '" + bookingUuid + "', " + 1 + ", " + 2 + ");");
                    if (tbl == null || tbl.Rows.Count == 0)
                        return new List<TrackingDetailsDTO>();
                    trackingdetails = (from DataRow dr in tbl.Rows
                                       select new TrackingDetailsDTO
                                       {
                                           TrackingId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["trackingid"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["trackingid"]))
                                           },
                                           TrackingUuid = Common.ToString(dr["trackinguuid"]),                                          
                                           MoveTypeId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["movetype"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["movetype"]))
                                           },
                                           CurrentMovesName = Common.ToString(dr["currentmovesname"]),
                                           CurrentMovesIcon = Common.ToString(dr["currentmovesicon"]),                                         
                                           CurrentLocationName = Common.ToString(dr["currentlocationname"]),
                                           CurrentLocationCode = Common.ToString(dr["currentlocationcode"]),                                          
                                           CurrentLocationTypeName = Common.ToString(dr["currentlocationtypename"]),
                                           CurrentPortName = Common.ToString(dr["currentportname"]),
                                           CurrentPortCode = Common.ToString(dr["currentportcode"]),
                                           CurrentCountryName = Common.ToString(dr["currentcountryname"]),
                                           CurrentCountryCode = Common.ToString(dr["currentcountrycode"]),
                                           CurrentCountryFlag = Common.ToString(dr["currentcountryflag"]),
                                           RecordDateTime = Common.ToDateTimeString(dr["recorddatetime"], Common.HumanDateTimeformat),
                                           NextMoveId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["nextmove"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["nextmove"]))
                                           },
                                           NextMovesName = Common.ToString(dr["nextmovesname"]),
                                           NextMovesIcon = Common.ToString(dr["nextmovesicon"]),                                         
                                           NextLocationUuid = Common.ToString(dr["nextlocationuuid"]),
                                           NextLocationName = Common.ToString(dr["nextlocationname"]),
                                           NextLocationCode = Common.ToString(dr["nextlocationcode"]),
                                           NextPortId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["nextportid"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["nextportid"]))
                                           },
                                           NextLocationTypeName = Common.ToString(dr["nextlocationtypename"]),
                                           NextPortName = Common.ToString(dr["nextportname"]),
                                           NextPortCode = Common.ToString(dr["nextportcode"]),
                                           NextCountryName = Common.ToString(dr["nextcountryname"]),
                                           NextCountryCode = Common.ToString(dr["nextcountrycode"]),
                                           NextCountryFlag = Common.ToString(dr["nextcountryflag"]),
                                           NextDateTime = Common.ToDateTimeString(dr["nextdatetime"], Common.HumanDateTimeformat),
                                           IsDamaged = Common.ToBool(dr["isdamaged"]),
                                           ContainerEquipmentno = Common.ToString(dr["containerequipmentno"]),
                                           ContainerTypeName = Common.ToString(dr["containertypename"]),
                                           ContainerSizeName = Common.ToString(dr["containersizename"]),                                          
                                       }).ToList();
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
                throw;
            }
            return trackingdetails;
        }
        public TrackingDTO GetTrackingByUUID(string trackingUuid)
        {
            TrackingDTO tracking = new TrackingDTO();
            try
            {
                if (trackingUuid != "")
                {
                    tracking = ParseTracking("SELECT * FROM tracking.container_movement_tracking_get_by_trackinguuid('" + trackingUuid + "', '" + 1 + "', '" + 2 + "');");
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return tracking;
        }

        private TrackingDTO ParseTracking(string qry)
        {
            TrackingDTO tracking = new TrackingDTO();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(qry);
                    if (tbl == null || tbl.Rows.Count == 0)
                        return null;
                    {
                        tracking.TrackingId = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(tbl.Rows[0]["cmt_trackingid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(tbl.Rows[0]["cmt_trackingid"]))
                        };
                        tracking.TrackingUuid = Common.ToString(tbl.Rows[0]["cmt_trackinguuid"]);                       
                        tracking.LocationDetailId = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(tbl.Rows[0]["cmt_locationdetailid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(tbl.Rows[0]["cmt_locationdetailid"]))
                        };
                        tracking.RecordDateTime = Common.ToDateTimeString(tbl.Rows[0]["cmt_recorddatetime"], Common.DBDateTimeformat);
                        tracking.CurrentVoyageId = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(tbl.Rows[0]["cmt_voyageid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(tbl.Rows[0]["cmt_voyageid"]))
                        };
                       // tracking.CurrentVoyageName = Common.ToString(tbl.Rows[0]["cmt_currentvoyagename"]);               
                        tracking.NextLocationDetailId = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(tbl.Rows[0]["cmt_nextlocationdetailid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(tbl.Rows[0]["cmt_nextlocationdetailid"]))
                        };
                        tracking.NextDateTime = Common.ToDateTimeString(tbl.Rows[0]["cmt_nextdatetime"], Common.DBDateTimeformat);
                        tracking.NextVoyageId = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(tbl.Rows[0]["cmt_nextvoyageid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(tbl.Rows[0]["cmt_nextvoyageid"]))
                        };
                       // tracking.NextVoyageName = Common.ToString(tbl.Rows[0]["cmt_nextvoyagename"]);
                        tracking.IsDamaged = Common.ToBool(tbl.Rows[0]["cmt_isdamaged"]);
                        tracking.NoOfDamages = Common.ToInt(tbl.Rows[0]["cmt_noofdamages"]);                    
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }

            return tracking;
        }

        public Result SaveTempTracking(int containerId, int bookingId)
        {
            Result result = new Result();
            try
            {
                using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = db.GetDataTable(
                        "SELECT * FROM tracking.container_movement_tracking_temp_save(" +
                        "p_containerid := " + containerId + "," +
                        "p_bookingid := " + bookingId + "," +
                        "p_hubid := " + 1 +
                        ");"
                    );

                    if (tbl.Rows.Count != 0)
                    {
                        if (Common.ToInt(tbl.Rows[0][0]) > 0)
                        {
                            result = Common.SuccessMessage(Common.ToString(tbl.Rows[0][1]));
                            result.TargetUUID = Common.ToString(tbl.Rows[0][2]); 
                            result.TargetID = Common.ToInt(tbl.Rows[0][3]);
                        }
                        else
                        {
                            result = Common.ErrorMessage(Common.ToString(tbl.Rows[0][1]));
                        }
                    }
                    else
                    {
                        result = Common.ErrorMessage("Cannot process request");
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
    }
}
