using Newtonsoft.Json;
using System.Data;

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
                    string filters = Newtonsoft.Json.JsonConvert.SerializeObject(filter);
                    DataTable tbl = db.GetDataTable(
                        "SELECT * FROM booking.booking_list('"
                        + Common.HubID + "','"
                        + filters + "','"
                        + Common.UserID + "');"
                    );
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
                                bookingdate = Common.ToDateTime(dr["bookingdate"]),
                                agencydetailid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(dr["agencydetailid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["agencydetailid"]))
                                },
                                clientdetailid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(dr["clientdetailid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["clientdetailid"]))
                                },
                                agencyuuid = Common.ToString(dr["agencyuuid"]),
                                clientuuid = Common.ToString(dr["clientuuid"]),
                                createdat = FormatConvertor.ToClientDateTimeFormat(Common.ToDateTime(dr["createdat"])),
                                customertype = Common.ToInt(dr["customertype"]),
                                customertypename = IDReferences.GetCustomerTypeName(Common.ToInt(dr["customertype"])),
                                pol = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(dr["pol"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["pol"]))
                                },
                                pol_portname = Common.ToString(dr["pol_portname"]),
                                pol_portcode = Common.ToString(dr["pol_portcode"]),
                                pol_countryname = Common.ToString(dr["pol_countryname"]),
                                pol_countrycode = Common.ToString(dr["pol_countrycode"]),
                                pol_countryflag = Common.ToString(dr["pol_countryflag"]),
                                pod = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(dr["pod"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["pod"]))
                                },
                                pod_portname = Common.ToString(dr["pod_portname"]),
                                pod_portcode = Common.ToString(dr["pod_portcode"]),
                                pod_countryname = Common.ToString(dr["pod_countryname"]),
                                pod_countrycode = Common.ToString(dr["pod_countrycode"]),
                                pod_countryflag = Common.ToString(dr["pod_countryflag"]),
                                shipperdetailid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(dr["shipperdetailid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["shipperdetailid"]))
                                },
                                consigneedetailid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(dr["consigneedetailid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(dr["consigneedetailid"]))
                                },
                                voyageuuid = Common.ToString(dr["voyageuuid"]),
                                voyagenumber = Common.ToString(dr["voyagenumber"]),
                                vesselname = Common.ToString(dr["vesselname"]),
                                status = FormatConvertor.ToStatus(Common.ToInt(dr["status"]), StatusEnum.Booking, statusList),
                                createdby = Common.ToInt(dr["createdby"]),
                                createdbyusername = Common.ToString(dr["createdby_username"]),
                                agencyname = Common.ToString(dr["agencyname"]),
                                customername = Common.ToString(dr["customername"]),
                                booking_details = Common.ToString(dr["booking_details"])
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
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
                        "p_hubid := '" + Common.HubID + "'" +
                        ");");
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
                            parent.createdat = Common.ToDateTime(dr["createdat"]);
                            parent.createdusername = Common.ToString(dr["createdusername"]);
                        }
                        model.bookingdate = Common.ToDateTimeString(dr["bookingdate"], "yyyy-MM-dd HH:mm");
                        model.customertype = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(dr["customertype"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(dr["customertype"]))
                        };
                        model.agencydetailid = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(dr["agencydetailid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(dr["agencydetailid"]))
                        };
                        model.agencyname = Common.ToString(dr["agencyname"]);
                        model.isconfirmed = Common.ToBool(dr["isconfirmed"]);
                        model.mode = new EncryptedData()
                        {
                            NumericValue = Common.ToInt(dr["mode"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(dr["mode"]))
                        };
                        model.fullempty = Common.ToString(dr["fullempty"]);
                        model.status = FormatConvertor.ToStatus(Common.ToInt(dr["status"]), StatusEnum.Booking, statusList);
                        model.client = new ClientDTO()
                        {
                            clientdetailid = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["clientdetailid"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["clientdetailid"]))
                            },
                            clientuuid = Common.ToString(dr["clientuuid"]),
                            clientname = Common.ToString(dr["clientname"]),
                            email = Common.ToString(dr["clientemail"]),
                            phone = Common.ToString(dr["clientphone"]),
                            address = Common.ToString(dr["clientaddress"]),
                            agency = new AgencyDTO()
                            {
                                uuid = Common.ToString(dr["agencyuuid"]),
                                agencyname = Common.ToString(dr["agencyname"]),
                                email = Common.ToString(dr["agencyemail"]),
                                phone = Common.ToString(dr["agencyphone"])
                            }
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
                        "p_hubid := '" + Common.HubID + "'" +
                        ");");

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
                            //bookingid = new EncryptedData()
                            //{
                            //    NumericValue = Common.ToInt(dr["bookingid"]),
                            //    EncryptedValue = Common.Encrypt(Common.ToInt(dr["bookingid"]))
                            //},
                            //bookinguuid = Common.ToString(dr["bookinguuid"]),
                            pol = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["pol_id"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["pol_id"]))
                            },
                            pol_portname = Common.ToString(dr["pol_portname"]),
                            pol_portcode = Common.ToString(dr["pol_portcode"]),
                            pol_countryid = Common.ToInt(dr["pol_countryid"]),
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
                            pod_countryid = Common.ToInt(dr["pod_countryid"]),
                            pod_countryname = Common.ToString(dr["pod_countryname"]),
                            pod_countrycode = Common.ToString(dr["pod_countrycode"]),
                            pod_countryflag = Common.ToString(dr["pod_countryflag"]),
                            shipperdetailid = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["shipperdetailid"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["shipperdetailid"]))
                            },
                            shippername = Common.ToString(dr["shippername"]),
                            shipperpic = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["shipperpic"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["shipperpic"]))
                            },
                            shipperpiccustom = Common.ToString(dr["shipperpiccustom"]),
                            shipperpic_name = Common.ToString(dr["shipperpic_name"]),
                            shipperpic_email = Common.ToString(dr["shipperpic_email"]),
                            shipperpic_phone = Common.ToString(dr["shipperpic_phone"]),
                            shipperemail = Common.ToString(dr["shipperemail"]),
                            shipperphone = Common.ToString(dr["shipperphone"]),
                            shipperaddress = Common.ToString(dr["shipperaddress"]),
                            consigneedetailid = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["consigneedetailid"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["consigneedetailid"]))
                            },
                            consigneename = Common.ToString(dr["consigneename"]),
                            consigneepic = new EncryptedData()
                            {
                                NumericValue = Common.ToInt(dr["consigneepic"]),
                                EncryptedValue = Common.Encrypt(Common.ToInt(dr["consigneepic"]))
                            },
                            consigneepiccustom = Common.ToString(dr["consigneepiccustom"]),
                            consigneepic_name = Common.ToString(dr["consigneepic_name"]),
                            consigneepic_email = Common.ToString(dr["consigneepic_email"]),
                            consigneepic_phone = Common.ToString(dr["consigneepic_phone"]),
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
                        "p_hubid := '" + Common.HubID + "'" + ");");

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
                        "p_hubid := '" + Common.HubID + "'" +
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
                        @"SELECT * FROM booking.booking_get_containerdetail(" +
                        "p_bookinguuid := '" + Common.Escape(bookinguuid) + "'," +
                        "p_hubid := '" + Common.HubID + "')"
                    );

                    var rows = tbl.AsEnumerable();

                    list = rows.GroupBy(r => Common.ToInt(r["bookingdetailid"]))
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
                                bookingid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(first["bookingid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(first["bookingid"]))
                                },
                                containermodeluuid = Common.ToString(first["modeluuid"]),
                                containertypeid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(first["typeid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(first["typeid"]))
                                },
                                sizeid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(first["sizeid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(first["sizeid"]))
                                },
                                ownership = Common.ToInt(first["ownership"]),
                                qty = Common.ToInt(first["qty"]),
                                commodity = Common.ToString(first["commodity"]),
                                grossweight = Common.ToDecimal(first["grossweight"]),
                                volumeweight = Common.ToDecimal(first["volumeweight"]),
                                hscode = Common.ToString(first["hscode"]),
                                cargovalue = Common.ToDecimal(first["cargovalue"]),
                                packagetype = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(first["packagetype"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(first["packagetype"]))
                                },
                                expectedstuffingdate = FormatConvertor.ToDateFormat(Common.ToDateTime(first["expectedstuffingdate"])),
                                stuffinglocation = Common.ToString(first["stuffinglocation"]),
                                pickuplocation = Common.ToString(first["pickuplocation"]),
                                isdg = Common.ToBool(first["isdg"]),
                                isreefer = Common.ToBool(first["isreefer"]),
                                sizename = Common.ToString(first["sizename"]),
                                length = Common.ToString(first["length"]),
                                width = Common.ToString(first["width"]),
                                height = Common.ToString(first["height"]),
                                isocode = Common.ToString(first["iso_code"]),
                                modeldescription = Common.ToString(first["description"]),
                                containertypeuuid = Common.ToString(first["typeuuid"]),
                                containertypename = Common.ToString(first["typename"]),
                                containertypeshortname = Common.ToString(first["typeshortname"]),
                                iconid = new EncryptedData
                                {
                                    NumericValue = Common.ToInt(first["iconid"]),
                                    EncryptedValue = Common.Encrypt(Common.ToInt(first["iconid"]))
                                },
                                icon = Common.ToString(first["icon"]),
                                empty_full = Common.ToString(first["empty_full"]),
                            };

                            dto.services = containerGrp
                                .Where(r => Common.ToInt(r["serviceid"]) > 0)
                                .GroupBy(r => Common.ToInt(r["serviceid"]))
                                .Select(serviceGrp =>
                                {
                                    var s = serviceGrp.First();

                                    return new ContainerBookingDetailServicesDTO
                                    {
                                        bookingdetailserviceid = new EncryptedData
                                        {
                                            NumericValue = Common.ToInt(s["bookingdetailserviceid"]),
                                            EncryptedValue = Common.Encrypt(Common.ToInt(s["bookingdetailserviceid"]))
                                        },
                                        bookingdetailid = new EncryptedData
                                        {
                                            NumericValue = Common.ToInt(first["bookingdetailid"]),
                                            EncryptedValue = Common.Encrypt(Common.ToInt(first["bookingdetailid"]))
                                        },
                                        serviceid = new EncryptedData
                                        {
                                            NumericValue = Common.ToInt(s["serviceid"]),
                                            EncryptedValue = Common.Encrypt(Common.ToInt(s["serviceid"]))
                                        },
                                        servicetype = new EncryptedData
                                        {
                                            NumericValue = Common.ToInt(s["servicetype"]),
                                            EncryptedValue = Common.Encrypt(Common.ToInt(s["servicetype"]))
                                        },
                                        servicename = Common.ToString(s["servicename"]),
                                        serviceorderby = Common.ToInt(s["serviceorderby"])
                                    };
                                }).OrderBy(s => s.serviceorderby).ToList();
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
                                    p_hubid := '" + Common.HubID + @"'
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
                                    })
                                    .ToList();

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
        public List<ContainerAllottedDTO> GetContainerAllotment(string bookinguuid)
        {
            List<ContainerAllottedDTO> list = new List<ContainerAllottedDTO>();
            try
            {
                using (SqlDB db = new SqlDB(DatabaseCollection.Contrack))
                {
                    DataTable tbl = db.GetDataTable("SELECT * FROM booking.container_selection_get('" + Common.HubID + "','" + bookinguuid + "');");
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
    }
}