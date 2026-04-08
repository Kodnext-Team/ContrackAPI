using System.Collections.Generic;

namespace ContrackAPI
{
    public class BookingService : CustomException, IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly IVoyageRepository _voyageRepo;
        public BookingService(IBookingRepository repo, IVoyageRepository voyageRepo)
        {
            _repo = repo;
            _voyageRepo = voyageRepo;
        }

        public List<ContainerBookingListDTO> GetBookingList(BookingListFilter filter)
        {
            try
            {
                return _repo.GetbookingList(filter);
            }
            catch (Exception ex)
            {
                RecordException(ex);
                return new List<ContainerBookingListDTO>();
            }
        }

        public ContainerBooking GetbookingByUUID(string bookinguuid, bool getsummary = false)
        {
            ContainerBooking booking = new ContainerBooking();
            try
            {
                if (!string.IsNullOrEmpty(bookinguuid))
                {
                    booking.booking = _repo.GetbookingByUUID(bookinguuid);
                    if (getsummary)
                        booking.bookingSummary = GetBookingSummaryInfo(booking);

                    GetVoyageInfo(booking);
                    PrefillShipperConsignee(booking);

                    var allServices = _repo.GetBookingAdditionalServices(bookinguuid);

                    if (allServices != null && allServices.Count > 0)
                    {
                        booking.booking.additionalservices = allServices
                            .Where(x => x.type == 1)
                            .OrderBy(x => x.order)
                            .ToList();

                        booking.booking.PODadditionalservices = allServices
                            .Where(x => x.type == 2)
                            .OrderBy(x => x.order)
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }

            return booking;
        }

        private void GetVoyageInfo(ContainerBooking booking)
        {
            try
            {
                if (!string.IsNullOrEmpty(booking.booking.location.voyageuuid))
                {
                    booking.voyage = _voyageRepo.GetVoyageByUUID(booking.booking.location.voyageuuid);
                }
            }
            catch (Exception)
            { }
        }
        private BookingSummaryDTO GetBookingSummaryInfo(ContainerBooking booking)
        {
            BookingSummaryDTO model = new BookingSummaryDTO();
            try
            {
                model = _repo.GetBookingSummaryInfo(booking.booking.bookinguuid);
                //if (model.summaryid.NumericValue == 0)
                //{
                //    var client = GetBillToAddressByClient(booking.booking.customer.client.clientdetailid.EncryptedValue);

                //    model.currency = client.preferredcurrency;

                //    var pricinglist = _PricingRepository.GetPricingByBookingUUID(booking.booking.bookinguuid, model.currency, booking.booking.customer.client.clientdetailid.EncryptedValue);
                //    model.freightchargeamount = pricinglist.Details.Where(x => x.IsFrightCharges).Sum(x => x.Amount);
                //    model.otherfees.AddRange(
                //                 pricinglist.Details
                //                 .Where(x => !x.IsFrightCharges)
                //                 .Select(x => new OtherFeesDTO()
                //                 {
                //                     chargename = x.LineItemDesc,
                //                     amount = x.Amount,
                //                     remarks = x.Comments,
                //                     uom = x.UOM
                //                 })
                //                 .ToList());
                //}
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return model;
        }

        private void PrefillShipperConsignee(ContainerBooking booking)
        {
            try
            {
                var customer = booking.booking.customer;
                var client = customer.client;
                var location = booking.booking.location;

                if (client.clientdetailid.NumericValue > 0)
                {
                    switch (customer.customertype.NumericValue)
                    {
                        case 1: // Shipper
                            if (string.IsNullOrEmpty(location.shippername))
                            {
                                location.shipperdetailid = client.clientdetailid;
                                location.shipperpic = location.shipperpic;
                                location.shipperpiccustom = location.shipperpiccustom;
                                location.shippername = client.clientname;
                                location.shipperemail = client.email;
                                location.shipperphone = client.phone;
                                location.shipperaddress = client.address;
                            }
                            break;

                        case 2: // Consignee
                            if (string.IsNullOrEmpty(location.consigneename))
                            {
                                location.consigneedetailid = client.clientdetailid;
                                location.consigneepic = location.consigneepic;
                                location.shipperpiccustom = location.shipperpiccustom;
                                location.consigneename = client.clientname;
                                location.consigneeemail = client.email;
                                location.consigneephone = client.phone;
                                location.consigneeaddress = client.address;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            { RecordException(ex); }
        }
        public ContainerSelection GetContainerSelection(string bookinguuid)
        {
            try
            {
                ContainerBookingDTO booking = _repo.GetbookingByUUID(bookinguuid);
                var selections = _repo.GetContainerSelection(bookinguuid);
                var allotted = _repo.GetContainerAllotment(bookinguuid);

                selections
                    .SelectMany(l => l.Details)
                    .SelectMany(d => d.Containers)
                    .ToList()
                    .ForEach(c =>
                    {
                        c.Locked = !string.IsNullOrEmpty(c.AllocationBookingUUID)
                                   && c.AllocationBookingUUID != booking.bookinguuid;

                        c.Allotted = c.AllocationBookingUUID == booking.bookinguuid
                                     || allotted.Exists(x => x.containerid.NumericValue == c.ContainerID.NumericValue);
                    });

                ContainerSelection selection = new ContainerSelection()
                {
                    Selections = selections,
                    Booking = booking,
                    Allotted = allotted
                };

               // SessionManager.CurrentContainerSelection = selection;

                return selection;
            }
            catch (Exception ex)
            {
                RecordException(ex);
                return new ContainerSelection();
            }
        }
        public Result SaveContainerSelection(string bookingid, List<ContainerSelectionDTO> selections)
        {
            Result result = new Result();

            try
            {
                selections = selections ?? new List<ContainerSelectionDTO>();

                var validSelections = selections
                    .SelectMany(l => l.Details ?? new List<ContainerSelectionDetailDTO>())
                    .SelectMany(d => d.Containers ?? new List<SelectionItemDTO>())
                    .Where(x => x.Selected)
                    .ToList();

                if (validSelections.Count == 0)
                {
                    return Common.ErrorMessage("No containers selected.");
                }

                result = _repo.SaveContainerSelection(bookingid, selections);
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