using static System.Collections.Specialized.BitVector32;

namespace ContrackAPI
{
    public class BookingService : CustomException, IBookingService
    {
        private readonly IBookingRepository _repo;
        public BookingService(IBookingRepository repo)
        {
            _repo = repo;
        }
        public APIResponse GetBookingList(BookingListFilter filter)
        {
            var response = new APIResponse();
            try
            {
                var data = _repo.GetbookingList(filter);
                if (data == null)
                {
                    response.Result = Common.ErrorMessage("No data found");
                }
                else
                {
                    response.Result = Common.SuccessMessage("Success");
                    response.Data = data;
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return response;
        }
        public APIResponse GetBookingByUUID(string bookinguuid)
        {
            var response = new APIResponse();
            try
            {
                var data = _repo.GetbookingByUUID(bookinguuid);
                if (data != null && !string.IsNullOrEmpty(data.bookinguuid))
                {
                    var booking = new ContainerBooking
                    {
                        booking = data
                    };
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
                  //  SessionManager.Booking = booking;
                    response.Result = Common.SuccessMessage("Success");
                    response.Data = booking;
                }
                else
                {
                    response.Result = Common.ErrorMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return response;
        }

        //private void GetVoyageInfo(ContainerBooking booking)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(booking.booking.location.voyageuuid))
        //        {
        //            booking.voyage = _voyageRepo.GetVoyageByUUID(booking.booking.location.voyageuuid);
        //        }
        //    }
        //    catch (Exception)
        //    { }
        //}       
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
        public APIResponse GetContainerSelection(string bookinguuid)
        {
            var response = new APIResponse();
            try
            {
             ContainerBookingDTO booking = _repo.GetbookingByUUID(bookinguuid);
                if (booking == null || string.IsNullOrEmpty(booking.bookinguuid))
                {
                    response.Result = Common.ErrorMessage("No data found");
                    return response;
                }
                var selections = _repo.GetContainerSelection(bookinguuid);
                var allotted = _repo.GetContainerAllotment(bookinguuid);
                selections.SelectMany(l => l.Details).SelectMany(d => d.Containers).ToList().ForEach(c =>
                    {
                        c.Locked = !string.IsNullOrEmpty(c.AllocationBookingUUID)&& c.AllocationBookingUUID != booking.bookinguuid;
                        c.Allotted = c.AllocationBookingUUID == booking.bookinguuid|| allotted.Exists(x => x.containerid.NumericValue == c.ContainerID.NumericValue);
                    });
                ContainerSelection selection = new ContainerSelection()
                {
                    Selections = selections,
                    Booking = booking,
                    Allotted = allotted
                };
               // SessionManager.CurrentContainerSelection = seletion;
                response.Result = Common.SuccessMessage("Success");
                response.Data = selection;
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return response;
        }
        //public Result SaveContainerSelection(string bookingid, List<ContainerSelectionDTO> selections)
        //{
        //    Result result = new Result();

        //    try
        //    {
        //        selections = selections ?? new List<ContainerSelectionDTO>();
        //        var seletion = SessionManager.CurrentContainerSelection ?? new ContainerSelection();

        //        var latestContainerIds = selections
        //            .SelectMany(l => l.Details)
        //            .SelectMany(d => d.Containers)
        //            .Where(x => x.Selected)
        //            .Select(c => c.ContainerID.EncryptedValue)
        //            .ToHashSet();

        //        foreach (var oldLocation in seletion.Selections)
        //        {
        //            var newLocation = selections
        //                .FirstOrDefault(x => x.LocationUuid == oldLocation.LocationUuid);

        //            if (newLocation == null)
        //            {
        //                newLocation = new ContainerSelectionDTO
        //                {
        //                    LocationUuid = oldLocation.LocationUuid,
        //                    Details = new List<ContainerSelectionDetailDTO>()
        //                };
        //                selections.Add(newLocation);
        //            }

        //            foreach (var oldModel in oldLocation.Details)
        //            {
        //                var newModel = newLocation.Details
        //                    .FirstOrDefault(x => x.ContainerModelUuid == oldModel.ContainerModelUuid);

        //                if (newModel == null)
        //                {
        //                    newModel = new ContainerSelectionDetailDTO
        //                    {
        //                        ContainerModelUuid = oldModel.ContainerModelUuid,
        //                        Containers = new List<SelectionItemDTO>()
        //                    };
        //                    newLocation.Details.Add(newModel);
        //                }

        //                foreach (var oldCont in oldModel.Containers)
        //                {
        //                    if (!latestContainerIds.Contains(oldCont.ContainerID.EncryptedValue))
        //                    {
        //                        newModel.Containers.Add(new SelectionItemDTO
        //                        {
        //                            ContainerID = oldCont.ContainerID,
        //                            IsDeleted = true,
        //                            Selected = true
        //                        });
        //                    }
        //                }
        //            }
        //        }

        //        result = _repo.SaveContainerSelection(bookingid, selections);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = Common.ErrorMessage(ex.Message);
        //        RecordException(ex);
        //    }

        //    return result;
        //}
        public APIResponse SaveContainerSelection(ContainerSelection bookingmodel)
        {
            var response = new APIResponse();
            Result result = new Result();
            try
            {
                var bookingid = bookingmodel.Booking.bookingid.EncryptedValue;
                var selections = bookingmodel.Selections ?? new List<ContainerSelectionDTO>();
                selections = selections ?? new List<ContainerSelectionDTO>();

                var latestContainerIds = selections
                    .SelectMany(l => l.Details ?? new List<ContainerSelectionDetailDTO>())
                    .SelectMany(d => d.Containers ?? new List<SelectionItemDTO>())
                    .Where(x => x.Selected)
                    .Select(c => c.ContainerID.EncryptedValue)
                    .ToHashSet();
                foreach (var location in selections)
                {
                    if (location.Details == null) continue;

                    foreach (var model in location.Details)
                    {
                        if (model.Containers == null) continue;

                        foreach (var cont in model.Containers)
                        {
                            if (!latestContainerIds.Contains(cont.ContainerID.EncryptedValue))
                            {
                                cont.IsDeleted = true;
                            }
                        }
                    }
                }
                // Save to DB
                result = _repo.SaveContainerSelection(bookingid, selections);
                response.Result = result;
            }
            catch (Exception ex)
            {
                result = Common.ErrorMessage(ex.Message);
                RecordException(ex);
            }
            return response;
        }
    }
}