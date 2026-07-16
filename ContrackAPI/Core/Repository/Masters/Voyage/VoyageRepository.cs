using Newtonsoft.Json;
using System.Data;
namespace ContrackAPI
{
    public class VoyageRepository : CustomException, IVoyageRepository
    {
        private static (DateTime MinDate, DateTime MaxDate) GetMaxMinDate(List<VoyageDetailDTO> list)
        {
            DateTime mindate = DateTime.MinValue;
            DateTime maxdate = DateTime.MinValue;
            try
            {
                var arrivals = list
               .Select(x => x.ATA.Value != DateTime.MinValue ? x.ATA : x.ETA)
               .Select(d => d).ToList();

                var departures = list
                                .Select(x => x.ATD.Value != DateTime.MinValue ? x.ATD : x.ETD)
                                .Select(d => d).ToList();

                arrivals.AddRange(departures);

                mindate = arrivals.Min(x => x.Value);
                maxdate = arrivals.Max(x => x.Value);
            }
            catch (Exception)
            { }
            return (mindate, maxdate);
        }
        public List<VoyageDTO> GetDirectVoyageSearch(string originPortId, string destinationPortId)
        {
            List<VoyageDTO> list = new List<VoyageDTO>();

            try
            {
                using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
                {
                    string query = $@"SELECT * FROM masters.voyage_search_direct(
                                p_hubid := {1},
                                p_originportid := {Common.Decrypt(originPortId)},
                                p_destinationportid := {Common.Decrypt(destinationPortId)});";
                    DataTable tbl = db.GetDataTable(query);
                    if (tbl == null || tbl.Rows.Count == 0)
                        return null;
                    list = tbl.AsEnumerable().Select(r =>
                        {
                            return new VoyageDTO
                            {
                                VoyageId = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(r["voyageid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(r["voyageid"]))
                                },
                                VoyageUuid = Common.ToString(r["voyageuuid"]),
                                VoyageNumber = Common.ToString(r["voyagenumber"]),
                                OriginPort = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(r["originportid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(r["originportid"]))
                                },
                                DestinationPort = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(r["destinationportid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(r["destinationportid"]))
                                },
                                VesseDetailId = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(r["vesseldetailid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(r["vesseldetailid"]))
                                },
                                Vesselname = Common.ToString(r["vesselname"]),
                                VoyageDetails = r["segmentjson"] == DBNull.Value
                                    ? new List<VoyageDetailDTO>()
                                    : Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(r["segmentjson"].ToString())
                                        .Select((d, index) => new VoyageDetailDTO
                                        {
                                            VoyageDetailId = new EncryptedData
                                            {
                                                NumericValue = Common.ToInt(d.voyagedetailid),
                                                EncryptedValue = Common.Encrypt(Common.ToInt(d.voyagedetailid))
                                            },
                                            VoyageId = new EncryptedData
                                            {
                                                NumericValue = Common.ToInt(r["voyageid"]),
                                                EncryptedValue = Common.Encrypt(Common.ToInt(r["voyageid"]))
                                            },
                                            PortId = new EncryptedData
                                            {
                                                NumericValue = Common.ToInt(d.portid),
                                                EncryptedValue = Common.Encrypt(Common.ToInt(d.portid))
                                            },
                                            portname = Common.ToString(d.portname),
                                            portcode = Common.ToString(d.portcode),
                                            CountryId = Common.ToInt(d.countryid),
                                            CountryName = Common.ToString(d.countryname),
                                            CountryFlag = Common.ToString(d.flag),
                                            ETA = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.eta)),
                                            ETD = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.etd)),
                                            ATA = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.ata)),
                                            ATD = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.atd)),
                                            CutoffDeadline = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.cutoffdeadline)),
                                            Terminal = Common.ToString(d.terminal),
                                            ArrivalCaptain = Common.ToString(d.arrivalcaptain),
                                            DepartureCaptain = Common.ToString(d.departurecaptain),
                                            SortOrder = index + 1,
                                            CreatedAt = Common.ToDateTime(d.time)
                                        }).ToList()
                            };
                        }).ToList();
                    list.ForEach(x =>
                    {
                        var (minDate, maxDate) = GetMaxMinDate(x.VoyageDetails);
                        x.minDate = FormatConvertor.ToDateTimeFormat(minDate);
                        x.maxDate = FormatConvertor.ToDateTimeFormat(maxDate);

                        if (x.minDate != null && x.maxDate != null)
                        {
                            x.NoOfDays = x.maxDate.Value.Date.Subtract(x.minDate.Value.Date).Days;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return list;
        }
        public VoyageDTO GetVoyageByUUID(string uuid)
        {
            VoyageDTO voyage = new VoyageDTO();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    string query = "SELECT * FROM masters.voyage_get_by_uuid(" + "p_hubid := '" + 1 + "', " + "p_voyageuuid := '" + Common.Escape(uuid) + "'::uuid" + "," + "p_userid := '" + 2 + "');";
                    DataTable dt = Db.GetDataTable(query);
                    if (dt == null || dt.Rows.Count == 0)
                        return null;
                    var rows = dt.AsEnumerable();
                    var first = rows.First();
                    voyage = new VoyageDTO
                    {
                        VoyageId = new EncryptedData
                        {
                            NumericValue = Common.ToInt(first["h_voyageid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(first["h_voyageid"]))
                        },
                        VoyageUuid = Common.ToString(first["h_voyageuuid"]),
                        VoyageNumber = Common.ToString(first["h_voyagenumber"]),
                        VesseDetailId = new EncryptedData
                        {
                            NumericValue = Common.ToInt(first["h_vesseldetailid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(first["h_vesseldetailid"]))
                        },
                        Vesselname = Common.ToString(first["h_vesselname"]),
                        vesselassignmentid = new EncryptedData
                        {
                            NumericValue = Common.ToInt(first["h_vesselassignmentid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(first["h_vesselassignmentid"]))
                        },
                        IsLive = Common.ToBool(first["h_islive"]),
                        Description = Common.ToString(first["h_description"]),
                        CreatedAt = Common.ToDateTime(first["h_createdat"]),
                        VoyageDetails = new List<VoyageDetailDTO>()
                    };
                    voyage.VoyageDetails = rows
                        .Where(r => Common.ToInt(r["d_voyagedetailid"]) > 0)
                        .GroupBy(r => Common.ToInt(r["d_voyagedetailid"]))
                        .Select(g =>
                        {
                            var d = g.First();
                            return new VoyageDetailDTO
                            {
                                VoyageDetailId = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(d["d_voyagedetailid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(d["d_voyagedetailid"]))
                                },
                                VoyageId = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(d["d_voyageid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(d["d_voyageid"]))
                                },
                                PortId = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(d["d_portid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(d["d_portid"]))
                                },
                                Terminal = Common.ToString(d["d_terminal"]),
                                ETA = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d["d_eta"])),
                                ETD = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d["d_etd"])),
                                CutoffDeadline = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d["d_cutoffdeadline"])),
                                ATA = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d["d_ata"])),
                                ATD = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d["d_atd"])),
                                ArrivalCaptain = Common.ToString(d["d_arrivalcaptain"]),
                                DepartureCaptain = Common.ToString(d["d_departurecaptain"]),
                                SortOrder = Common.ToInt(d["d_sortorder"]),
                                CreatedAt = Common.ToDateTime(d["d_createdat"])
                            };
                        }).ToList();

                    DateTime mindate, maxdate;
                    (mindate, maxdate) = GetMaxMinDate(voyage.VoyageDetails);
                    voyage.minDate = FormatConvertor.ToDateTimeFormat(mindate);
                    voyage.maxDate = FormatConvertor.ToDateTimeFormat(maxdate);
                    voyage.NoOfDays = voyage.maxDate.Value.Date.Subtract(voyage.minDate.Value.Date).Days;
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return voyage;
        }
        public List<VoyageDTO> SearchVoyage(string search, bool createnew)
        {
            var list = new List<VoyageDTO>();

            using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
            {
                string safeSearch = Common.Escape(search ?? "");

                string query = @" SELECT * FROM masters.voyage_search(p_hubid := " + 1 + @",p_allow_create := " + (createnew ? "true" : "false") + @", p_search_text := '" + safeSearch + @"');";

                DataTable tbl = db.GetDataTable(query);

                foreach (DataRow dr in tbl.Rows)
                {
                    list.Add(new VoyageDTO
                    {
                        VoyageId = new EncryptedData
                        {
                            NumericValue = Common.ToInt(dr["voyageid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(dr["voyageid"]))
                        },
                        VoyageNumber = Common.ToString(dr["voyagenumber"]),
                        ActualVoyageNumber = Common.ToString(dr["actualvoyagenumber"]),
                        Vesselname = Common.ToString(dr["vesselname"]),
                        Description = Common.ToString(dr["description"]),
                        VesseDetailId = new EncryptedData
                        {
                            NumericValue = Common.ToInt(dr["vesseldetailid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(dr["vesseldetailid"]))
                        },
                        IsNew = Common.ToBool(dr["isnew"])
                    });
                }
            }
            return list;
        }
        public List<VoyageDTO> GetVoyageList(VoyageFilter filter)
        {
            List<VoyageDTO> list = new List<VoyageDTO>();
            try
            {
                using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
                {
                    string filterJson = JsonConvert.SerializeObject(filter);
                    string query = $@"SELECT * FROM masters.voyage_list_full(
                    p_hubid   := {1},
                    p_filters := '{filterJson}'::jsonb,
                    p_userid  := {2});";
                    DataTable tbl = db.GetDataTable(query);
                    if (tbl == null || tbl.Rows.Count == 0)
                        return null;
                    list = tbl.AsEnumerable()
                        .Select(r =>
                        {
                            return new VoyageDTO
                            {
                                VoyageId = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(r["voyageid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(r["voyageid"]))
                                },
                                VoyageUuid = Common.ToString(r["voyageuuid"]),
                                VoyageNumber = Common.ToString(r["voyagenumber"]),
                                Vesselname = Common.ToString(r["vesselname"]),
                                IsLive = Common.ToBool(r["islive"]),
                                CreatedAt = Common.ToDateTime(r["createdat"]),
                                minDate = FormatConvertor.ToDateFormat(Common.ToDateTime(r["mindate"])),
                                maxDate = FormatConvertor.ToDateFormat(Common.ToDateTime(r["maxdate"])),
                                totalnoofrows = Common.ToInt(r["total_count"]),
                                VoyageDetails = r["detailjson"] == DBNull.Value
                                    ? new List<VoyageDetailDTO>()
                                    : JsonConvert.DeserializeObject<List<dynamic>>(r["detailjson"].ToString())
                                        .Select(d =>
                                        {
                                            var port = d.port;
                                            var country = d.country;
                                            return new VoyageDetailDTO
                                            {
                                                VoyageDetailId = new EncryptedData
                                                {
                                                    NumericValue = Common.ToInt(d.voyagedetailid),
                                                    EncryptedValue = Common.Encrypt(Common.ToInt(d.voyagedetailid))
                                                },
                                                VoyageId = new EncryptedData
                                                {
                                                    NumericValue = Common.ToInt(d.voyageid),
                                                    EncryptedValue = Common.Encrypt(Common.ToInt(d.voyageid))
                                                },
                                                PortId = new EncryptedData
                                                {
                                                    NumericValue = Common.ToInt(d.portid),
                                                    EncryptedValue = Common.Encrypt(Common.ToInt(d.portid))
                                                },
                                                portname = Common.ToString(port.portname),
                                                portcode = Common.ToString(port.portcode),
                                                PortCountryId = Common.ToInt(port.countryid),
                                                CountryId = Common.ToInt(country.countryid),
                                                CountryName = Common.ToString(country.countryname),
                                                CountryFlag = Common.ToString(country.flag),
                                                Terminal = Common.ToString(d.terminal),
                                                ETA = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.eta)),
                                                ETD = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.etd)),
                                                ETB = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.etb)),
                                                ATA = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.ata)),
                                                ATD = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.atd)),
                                                CutoffDeadline = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(d.cutoffdeadline)),
                                                ArrivalCaptain = Common.ToString(d.arrivalcaptain),
                                                DepartureCaptain = Common.ToString(d.departurecaptain),
                                                SortOrder = Common.ToInt(d.sortorder)
                                            };
                                        }).ToList()
                            };
                        }).ToList();
                    list.ForEach(x =>
                    {
                        x.Status = GetStatus(x.VoyageDetails);
                        x.NoOfDays = x.maxDate.Value.Date.Subtract(x.minDate.Value.Date).Days;
                        x.VoyageDetails.ForEach(dtl =>
                        {
                            dtl.PortStatus = GetPortStatus(dtl);
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return list;
        }
        private string GetStatus(List<VoyageDetailDTO> list)
        {
            if (list.Count > 0)
            {
                DateTime mindate, maxdate;
                (mindate, maxdate) = Common.GetMaxMinDate(list);
                //DateTime mindate = list.Min(y => y.ATA.Value == DateTime.MinValue ? y.ETA.Value : y.ATA.Value);
                //DateTime maxdate = list.Max(y => y.ATA.Value == DateTime.MinValue ? y.ETA.Value : y.ATA.Value);
                if (maxdate.Date < DateTime.Now.Date)
                {
                    return "<span class=\"badge-styling gray voyage-past\">Past</span>";// Past
                }
                else if (mindate.Date > DateTime.Now.Date)
                {
                    return "<span class=\"badge-styling green voyage-upcoming\">Upcoming</span>"; // Future
                }
                else
                {
                    return "<span class=\"badge-styling blue voyage-ongoing\">Ongoing</span>"; ;// Current
                }
            }
            else
                return "";
        }



        private string GetPortStatus(VoyageDetailDTO detail)
        {
            if (detail.ATD.Value > DateTime.MinValue)
            {
                return "<span><span class=\"badge-styling gray voyage-depart\">Departed</span></span>";
            }
            else if (detail.ATA.Value > DateTime.MinValue)
            {
                return "<span><span class=\"badge-styling green voyage-arrived\">Arrived</span></span>";
            }
            else
            {
                return "<span><span class=\"badge-styling blue voyage-scheduled\">Scheduled</span></span>";
            }
        }
    }
}