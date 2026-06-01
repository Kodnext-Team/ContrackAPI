using Newtonsoft.Json;
using System.Data;
using System.Reflection;
namespace ContrackAPI
{
    public class BookingRepository : CustomException, IBookingRepository
    {
        public List<ContainerBookingListDTO> GetbookingList(BookingListFilter filter)
        {
            List<ContainerBookingListDTO> list = new List<ContainerBookingListDTO>();
            try
            {
                using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
                {
                    string filters = JsonConvert.SerializeObject(filter ?? new BookingListFilter());
                    DataTable tbl = db.GetDataTable(
                        "SELECT * FROM booking.booking_list('" + 1 + "','" + Common.Escape(filters) + "','" + 2 + "');");
                      if (tbl == null || tbl.Rows.Count == 0)
                return null;
                    var statusList = Status.GetStatus();
                    list = (from DataRow dr in tbl.Rows
                            select new ContainerBookingListDTO
                            {
                                row_index = Common.ToInt(dr["row_index"]),
                                total_count = Common.ToInt(dr["total_count"]),
                                bookingid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(dr["bookingid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["bookingid"]))
                                },
                                bookinguuid = Common.ToString(dr["bookinguuid"]),
                                bookingno = Common.ToString(dr["bookingno"]),
                                bookingdate = FormatConvertor.ToClientDateTimeFormat(Common.ToDateTime(dr["bookingdate"])),
                                pol = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(dr["pol"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["pol"]))
                                },
                                pol_portname = Common.ToString(dr["pol_portname"]),
                                pol_portcode = Common.ToString(dr["pol_portcode"]),
                                pol_countryname = Common.ToString(dr["pol_countryname"]),
                                pol_countryflag = Common.ToString(dr["pol_countryflag"]),
                                pod = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(dr["pod"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["pod"]))
                                },
                                pod_portname = Common.ToString(dr["pod_portname"]),
                                pod_portcode = Common.ToString(dr["pod_portcode"]),
                                pod_countryname = Common.ToString(dr["pod_countryname"]),
                                pod_countryflag = Common.ToString(dr["pod_countryflag"]),                              
                                voyagenumber = Common.ToString(dr["voyagenumber"]),
                                vesselname = Common.ToString(dr["vesselname"]),
                                status = FormatConvertor.ToStatus(Common.ToInt(dr["status"]), StatusEnum.Booking, statusList),                              
                                agencyname = Common.ToString(dr["agencyname"]),
                                customername = Common.ToString(dr["customername"]),

                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
                throw new Exception(ex.Message, ex);
            }
            return list;
        }
        public BookingCustomerDTO GetBookingCustomerInfo(string bookinguuid, ContainerBookingDTO parent)
        {
            BookingCustomerDTO model = new BookingCustomerDTO();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                        "SELECT * FROM booking.booking_get_customer(" +
                        "p_bookinguuid := '" + Common.Escape(bookinguuid) + "'," +
                        "p_hubid := '" + 1 + "'" + ");");
                    var statusList = Status.GetStatus();
                    if (tbl.Rows.Count > 0)
                    {
                        DataRow dr = tbl.Rows[0];
                        if (parent != null && string.IsNullOrEmpty(parent.bookinguuid))
                        {
                            int bookingId = Common.ToInt(dr["bookingid"]);
                            parent.bookingid = new EncryptedData()
                            {
                                NumericValue = bookingId,
                                EncryptedValue = Common.Encrypt(bookingId)
                            };
                            parent.bookinguuid = Common.ToString(dr["bookinguuid"]);
                            parent.bookingno = Common.ToString(dr["bookingno"]);                           
                        }
                        
                        model.agencyname = Common.ToString(dr["agencyname"]);
                        int customerType = Common.ToInt(dr["customertype"]);

                        model.customertype = new EncryptedData()
                        {
                            NumericValue = customerType,
                            EncryptedValue = Common.Encrypt(customerType)
                        };

                        model.customertypename = Common.GetCustomerTypeName(customerType);
                        model.fullempty = Common.GetFullEmptyName(Common.ToString(dr["fullempty"]));
                        model.mode = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(dr["mode"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(dr["mode"]))
                        };
                        model.modename = Common.GetTransferTypeName(model.mode.NumericValue);
                        model.status = FormatConvertor.ToStatus(Common.ToInt(dr["status"]), StatusEnum.Booking, statusList);
                        model.client = new ClientDTO()
                        {
                            clientdetailid = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["clientdetailid"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["clientdetailid"]))
                            },
                            clientname = Common.ToString(dr["clientname"]),
                            email = Common.ToString(dr["clientemail"]),
                            phone = Common.ToString(dr["clientphone"]),
                            address = Common.ToString(dr["clientaddress"]),
                            
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return model;
        }

        public BookingLocationDTO GetBookingLocationInfo(string bookinguuid, ContainerBookingDTO parent)
        {
            BookingLocationDTO model = new BookingLocationDTO();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                        "SELECT * FROM booking.booking_get_location(" +
                        "p_bookinguuid := '" + Common.Escape(bookinguuid) + "'," +
                        "p_hubid := '" + 1 + "'" +");");
                    if (tbl.Rows.Count > 0)
                    {
                        DataRow dr = tbl.Rows[0];
                        if (parent != null && string.IsNullOrEmpty(parent.bookinguuid))
                        {
                            int bookingId = Common.ToInt(dr["bookingid"]);
                            parent.bookingid = new EncryptedData()
                            {
                                NumericValue = bookingId,
                                EncryptedValue = Common.Encrypt(bookingId)
                            };
                            parent.bookinguuid = Common.ToString(dr["bookinguuid"]);
                            parent.bookingno = Common.ToString(dr["bookingno"]);
                        }
                        model = new BookingLocationDTO()
                        {
                           
                            pol = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["pol_id"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["pol_id"]))
                            },
                            pol_portname = Common.ToString(dr["pol_portname"]),
                            pol_portcode = Common.ToString(dr["pol_portcode"]),
                            pol_countryname = Common.ToString(dr["pol_countryname"]),
                            pol_countrycode = Common.ToString(dr["pol_countrycode"]),
                            pol_countryflag = Common.ToString(dr["pol_countryflag"]),
                            pod = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["pod_id"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["pod_id"]))
                            },
                            pod_portname = Common.ToString(dr["pod_portname"]),
                            pod_portcode = Common.ToString(dr["pod_portcode"]),
                            pod_countryname = Common.ToString(dr["pod_countryname"]),
                            pod_countrycode = Common.ToString(dr["pod_countrycode"]),
                            pod_countryflag = Common.ToString(dr["pod_countryflag"]),
                            shippername = Common.ToString(dr["shippername"]),                        
                            shipperpiccustom = Common.ToString(dr["shipperpiccustom"]),                       
                            shipperemail = Common.ToString(dr["shipperemail"]),
                            shipperphone = Common.ToString(dr["shipperphone"]),
                            shipperaddress = Common.ToString(dr["shipperaddress"]),
                            consigneename = Common.ToString(dr["consigneename"]),
                            consigneepic = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["consigneepic"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["consigneepic"]))
                            },                           
                            consigneeemail = Common.ToString(dr["consigneeemail"]),
                            consigneephone = Common.ToString(dr["consigneephone"]),
                            consigneeaddress = Common.ToString(dr["consigneeaddress"]),
                            voyageuuid = Common.ToString(dr["voyageuuid"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return model;
        }
        public ContainerBookingDTO GetbookingByUUID(string bookinguuid)
        {
            ContainerBookingDTO booking = new ContainerBookingDTO();

            try
            {
                booking.customer = GetBookingCustomerInfo(bookinguuid, booking);
                booking.location = GetBookingLocationInfo(bookinguuid, booking);
                booking.details = GetContainerBookingDetailByBookingUUId(bookinguuid);
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return booking;
        }
        public BookingSummaryDTO GetBookingSummaryInfo(string bookinguuid)
        {
            BookingSummaryDTO model = new BookingSummaryDTO();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                        "SELECT * FROM booking.booking_get_summary(" +
                        "p_bookinguuid := '" + Common.Escape(bookinguuid) + "'," +
                        "p_hubid := '" + 1 + "'" + ");");

                    if (tbl.Rows.Count > 0)
                    {
                        DataRow dr = tbl.Rows[0];
                        model.summaryid = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(dr["summaryid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(dr["summaryid"]))
                        };
                        model.summaryuuid = Common.ToString(dr["summaryuuid"]);
                        model.bookinguuid = Common.ToString(dr["bookinguuid"]);
                        model.intramodal_transport = Common.ToBool(dr["intramodal_transport"]);
                        model.stuffing_unstuffing_units = Common.ToString(dr["stuffing_unstuffing_units"]);
                        model.pickupandreturn_empty_rentalunits = Common.ToString(dr["pickupandreturn_empty_rentalunits"]);
                        model.dropoff = Common.ToString(dr["dropoff"]);
                        model.cutoffdatetime = FormatConvertor.ToDateTimeFormat(Common.ToDateTime(dr["cutoffdatetime"]));
                        model.additionalterms = Common.ToString(dr["additionalterms"]);
                        model.currency = Common.ToString(dr["currency"]);
                        model.freetimepol = Common.ToInt(dr["freetimepol"]);
                        model.freetimepod = Common.ToInt(dr["freetimepod"]);
                        model.freightchargeamount = Common.ToDecimal(dr["freightchargeamount"]);
                        model.freightchargecomments = Common.ToString(dr["freightchargecomments"]);
                        model.totalothercharges = Common.ToDecimal(dr["totalothercharges"]);
                        if (dr["otherfees"] != DBNull.Value)
                        {
                            string json = dr["otherfees"].ToString();
                            model.otherfees = !string.IsNullOrEmpty(json) ? JsonConvert.DeserializeObject<List<OtherFeesDTO>>(json) : new List<OtherFeesDTO>();
                        }
                        else
                        {
                            model.otherfees = new List<OtherFeesDTO>();
                        }
                    }
                    else
                    {
                        model.bookinguuid = bookinguuid;
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return model;
        }
        public List<BookingAdditionalServicesDTO> GetBookingAdditionalServices(string bookinguuid)
        {
            List<BookingAdditionalServicesDTO> services = new List<BookingAdditionalServicesDTO>();

            try
            {
                using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = db.GetDataTable(
                        "SELECT * FROM booking.booking_get_additional_services(" +
                        "p_bookinguuid := '" + bookinguuid + "', " +
                        "p_hubid := '" + 1 + "'" +
                        ");"
                    );
                    services = (from DataRow dr in tbl.Rows
                                select new BookingAdditionalServicesDTO
                                {
                                    bookingadditionalserviceid = new EncryptedData
                                    {
                                        NumericValue = Common.ToInt(dr["bookingadditionalserviceid"]),
                                        EncryptedValue = Common.Encrypt(Common.ToInt(dr["bookingadditionalserviceid"]))
                                    },
                                    bookingid = new EncryptedData
                                    {
                                        NumericValue = Common.ToInt(dr["bookingid"]),
                                        EncryptedValue = Common.Encrypt(Common.ToInt(dr["bookingid"]))
                                    },
                                    additionalserviceid = new EncryptedData
                                    {
                                        NumericValue = Common.ToInt(dr["additionalserviceid"]),
                                        EncryptedValue = Common.Encrypt(Common.ToInt(dr["additionalserviceid"]))
                                    },
                                    locationtypeid = new EncryptedData
                                    {
                                        NumericValue = Common.ToInt(dr["locationtypeid"]),
                                        EncryptedValue = Common.Encrypt(Common.ToInt(dr["locationtypeid"]))
                                    },
                                    uom = Common.ToString(dr["uom"]),
                                    additionalserviceuuid = Common.ToString(dr["additionalserviceuuid"]),
                                    quantity = Common.ToDecimal(dr["quantity"]),
                                    servicename = Common.ToString(dr["servicename"]),
                                    description = Common.ToString(dr["description"]),
                                    order = Common.ToInt(dr["orderby"]),
                                    type = Common.ToInt(dr["type"])
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return services;
        }
       
        public List<ContainerBookingDetailDTO> GetContainerBookingDetailByBookingUUId(string bookinguuid)
        {
            List<ContainerBookingDetailDTO> list = new List<ContainerBookingDetailDTO>();

            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = Db.GetDataTable(
                        @"SELECT * FROM booking.booking_get_containerdetail(
                    p_bookinguuid := '" + Common.Escape(bookinguuid) + @"',
                    p_hubid := '1')"
                    );

                    var rows = tbl.AsEnumerable();

                    list = rows
                        .GroupBy(r => Common.ToInt(r["bookingdetailid"]))
                        .Select(containerGrp =>
                        {
                            var first = containerGrp.First();

                            var dto = new ContainerBookingDetailDTO
                            {
                                bookingdetailid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(first["bookingdetailid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(first["bookingdetailid"]))
                                },
                                bookingdetailuuid = Common.ToString(first["bookingdetailuuid"]),
                                ownership = Common.ToInt(first["ownership"]),
                                ownershipname = Common.GetOperatorName(Common.ToInt(first["ownership"])),
                                qty = Common.ToInt(first["qty"]),
                                commodity = Common.ToString(first["commodity"]),
                                volumeweight = Common.ToDecimal(first["volumeweight"]),
                                packagetype = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(first["packagetype"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(first["packagetype"]))
                                },
                                sizename = Common.ToString(first["sizename"]),                                
                                isocode = Common.ToString(first["iso_code"]),
                                containertypeuuid = Common.ToString(first["typeuuid"]),
                                containertypename = Common.ToString(first["typename"]),
                                icon = Common.ToString(first["icon"]),
                                empty_full = Common.GetFullEmptyName(Common.ToString(first["empty_full"]))
                            };

                            return dto;
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }

            return list;
        }
        public List<ContainerSelectionDTO> GetContainerSelection(string bookinguuid)
        {
            List<ContainerSelectionDTO> list = new List<ContainerSelectionDTO>();

            try
            {
                using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
                {
                    string query = @"SELECT * 
                                    FROM booking.container_equip_selection_list(
                                    p_bookinguuid := '" + bookinguuid + @"',
                                    p_hubid := '" + 1 + @"'
                                    );";

                    DataTable tbl = db.GetDataTable(query);
                    var rows = tbl.AsEnumerable();
                    list = rows
                            .GroupBy(r => Common.ToInt(r["locationid"]))
                            .Select(locationGroup =>
                            {
                                var loc = locationGroup.First();

                                int locationId = Common.ToInt(loc["locationid"]);

                                var dto = new ContainerSelectionDTO
                                {
                                    LocationName = Common.ToString(loc["locationname"]),
                                    LocationId = new EncryptedData
                                    {
                                        NumericValue = locationId,
                                        EncryptedValue = Common.Encrypt(locationId)
                                    },
                                    LocationUuid = Common.ToString(loc["locationuuid"]),
                                    LocationType = Common.ToString(loc["locationtypename"]),
                                    PortName = Common.ToString(loc["portname"]),
                                    PortCode = Common.ToString(loc["portcode"]),
                                    CountryName = Common.ToString(loc["countryname"]),
                                    CountryCode = Common.ToString(loc["countrycode"]),
                                    CountryFlag = Common.ToString(loc["flag"])
                                };
                                dto.Details = locationGroup
                                    .GroupBy(x => Common.ToString(x["modeluuid"]))
                                    .Select(modelGroup =>
                                    {
                                        var first = modelGroup.First();
                                        int reservationId = Common.ToInt(first["reservationid"]);
                                        int bookingDetailId = Common.ToInt(first["bookingdetailid"]);
                                        int modelId = Common.ToInt(first["modelid"]);
                                        var detail = new ContainerSelectionDetailDTO
                                        {
                                            ReservationID = new EncryptedData
                                            {
                                                NumericValue = reservationId,
                                                EncryptedValue = Common.Encrypt(reservationId)
                                            },
                                            RequiredCount = modelGroup.GroupBy(x => Common.ToInt(x["bookingdetailid"])).ToList().Sum(g => Common.ToInt(g.First()["qty"])),
                                            ContainerDetailID = new EncryptedData
                                            {
                                                NumericValue = bookingDetailId,
                                                EncryptedValue = Common.Encrypt(bookingDetailId)
                                            },
                                            ContainerTypeName = Common.ToString(first["typename"]),
                                            ContainerSizeName = Common.ToString(first["sizename"]),
                                            ContainerModelId = new EncryptedData
                                            {
                                                NumericValue = modelId,
                                                EncryptedValue = Common.Encrypt(modelId)
                                            },
                                            ContainerModelUuid = Common.ToString(first["modeluuid"]),
                                            ContainerModelIso = Common.ToString(first["iso_code"]),
                                            Containers = modelGroup.Select(c =>
                                            {
                                                int containerId = Common.ToInt(c["containerid"]);

                                                return new SelectionItemDTO
                                                {
                                                    ContainerID = new EncryptedData
                                                    {
                                                        NumericValue = containerId,
                                                        EncryptedValue = Common.Encrypt(containerId)
                                                    },
                                                    ContainerUUID = Common.ToString(c["containeruuid"]),
                                                    EquipmentNo = Common.ToString(c["equipmentno"]),
                                                    LastBookingDate = FormatConvertor.ToClientDateTimeFormat(
                                                        Common.ToDateTime(c["lastbookingdate"])
                                                    ),
                                                    AllocationBookingUUID = Common.ToString(c["allocation_bookinguuid"]),
                                                    AllocationDateTime = FormatConvertor.ToClientDateTimeFormat(
                                                        Common.ToDateTime(c["allocation_datetime"])
                                                    )
                                                };
                                            }).ToList()
                                        };

                                        return detail;
                                    }).ToList();
                                return dto;
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return list;
        }
        public List<ContainerAllottedDTO> GetContainerAllotment(string bookinguuid)
        {
            List<ContainerAllottedDTO> list = new List<ContainerAllottedDTO>();
            try
            {
                using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = db.GetDataTable("SELECT * FROM booking.container_selection_get('" + 1+ "','" + bookinguuid + "');");
                    list = (from DataRow dr in tbl.Rows
                            select new ContainerAllottedDTO
                            {
                                containerid = new EncryptedData()
                                {
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["containerid"])),
                                    NumericValue = Common.ToInt(dr["containerid"]),
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
        public Result SaveContainerSelection(string bookingid, List<ContainerSelectionDTO> selections)
        {
            Result result = new Result();
            try
            {
                using (SqlDB Db = new SqlDB(DatabaseCollection.Contrack))
                {

                    var details = selections
                                    .SelectMany(location => location.Details
                                        .SelectMany(model => model.Containers
                                        .Where(cont => cont.Selected)
                                        .Select(cont => new
                                        {
                                            containerid = Common.Decrypt(cont.ContainerID.EncryptedValue),
                                            isdeleted = cont.IsDeleted
                                        })
                                    )).ToList();

                    string detailsJson = System.Text.Json.JsonSerializer.Serialize(details);

                    string query = @"SELECT * FROM booking.container_selection_save(
                                     p_bookingid := '" + Common.Decrypt(bookingid) + @"',
                                     p_json := '" + Common.Escape(detailsJson) + @"'::jsonb,
                                     p_hubid := '" + Common.HubID + @"',
                                     p_userid := '" + Common.UserID + @"');";

                    DataTable tbl = Db.GetDataTable(query);
                    if (tbl != null && tbl.Rows.Count > 0)
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
                        result = Common.ErrorMessage("Cannot save container selection.");
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
                result = Common.ErrorMessage(ex.Message);
            }
            return result;
        }

    }
}