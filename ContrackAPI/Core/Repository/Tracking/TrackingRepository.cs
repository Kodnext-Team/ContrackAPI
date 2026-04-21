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
                                   "p_hubid := " + Common.HubID + "," +
                                   "p_containeruuid := " + (string.IsNullOrEmpty(filter.ContainerUUID) ? "NULL" : $"'{filter.ContainerUUID}'::uuid") + "," +
                                   "p_filters := '" + Common.Escape(jsonFilters) + "'::jsonb," +
                                   "p_userid := " + Common.UserID + ");";

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
                //move_icon = Common.GetSelectedIconPath(Common.ToInt(dr["move_iconid"])),
                containerid = new EncryptedData
                {
                    NumericValue = Common.ToInt(dr["containerid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["containerid"]))
                },
                bookingid = new EncryptedData
                {
                    NumericValue = Common.ToInt(dr["bookingid"]),
                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["bookingid"]))
                },
                locationuuid = Common.ToString(dr["locationuuid"]),
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
                canedit = Common.ToBool(dr["canedit"])
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
                         "p_hubid := " + Common.HubID + "," +
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
                         "p_createdby := " + Common.UserID + "" +
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
        public List<TrackingDetailsDTO> GetTrackingDetails(string containerUuid, string bookingUuid)
        {
            List<TrackingDetailsDTO> trackingdetails = new List<TrackingDetailsDTO>();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable("SELECT * FROM tracking.container_movement_tracking_get_by_uuid('" + containerUuid + "', '" + bookingUuid + "', " + Common.HubID + ", " + Common.UserID + ");");
                    if (tbl == null || tbl.Rows.Count == 0)
                        return null;
                    trackingdetails = (from DataRow dr in tbl.Rows
                                       select new TrackingDetailsDTO
                                       {
                                           TrackingId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["trackingid"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["trackingid"]))
                                           },
                                           TrackingUuid = Common.ToString(dr["trackinguuid"]),
                                           ContainerId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["containerid"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["containerid"]))
                                           },
                                           ContainerUuid = Common.ToString(dr["containeruuid"]),
                                           BookingId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["bookingid"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["bookingid"]))
                                           },
                                           BookingUuid = Common.ToString(dr["bookinguuid"]),
                                           MoveTypeId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["movetype"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["movetype"]))
                                           },
                                           CurrentMovesName = Common.ToString(dr["currentmovesname"]),
                                           CurrentMovesIcon = Common.ToString(dr["currentmovesicon"]),
                                           LocationDetailId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["locationdetailid"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["locationdetailid"]))
                                           },
                                           LocationUuid = Common.ToString(dr["locationuuid"]),
                                           CurrentLocationName = Common.ToString(dr["currentlocationname"]),
                                           CurrentLocationCode = Common.ToString(dr["currentlocationcode"]),
                                           CurrentPortId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["currentportid"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["currentportid"]))
                                           },
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
                                           NextLocationDetailId = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["nextlocationdetailid"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["nextlocationdetailid"]))
                                           },
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
                                           NoOfDamages = Common.ToInt(dr["noofdamages"]),
                                           ContainerEquipmentno = Common.ToString(dr["containerequipmentno"]),
                                           ContainerTypeName = Common.ToString(dr["containertypename"]),
                                           ContainerSizeName = Common.ToString(dr["containersizename"]),
                                           CreatedAt = Common.ToDateTimeString(Common.ToClientDateTime(dr["createdat"]), Common.HumanDateTimeformat),
                                           CreatedBy = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["createdby"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["createdby"]))
                                           },
                                           IsDeleted = Common.ToBool(dr["isdeleted"]),
                                           DeletedBy = new EncryptedData
                                           {
                                               NumericValue = Common.ToInt(dr["deletedby"]),
                                               EncryptedValue = Common.Encrypt(Common.ToInt(dr["deletedby"]))
                                           },
                                           DeletedAt = Common.ToDateTimeString(dr["deletedat"], "yyyy-MM-dd HH:mm")
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
    }
}
