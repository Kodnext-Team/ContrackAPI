namespace ContrackAPI
{
    public class TrackingService : CustomException, ITrackingService
    {
        private readonly ITrackingRepository _repo;
        private readonly IBookingService _bookingService;
        APIResponse response = new APIResponse();
        public TrackingService(ITrackingRepository repo, IBookingService bookingService)
        {
            _repo = repo;
            _bookingService = bookingService;
        }
        public APIResponse GetTrackingList(TrackingFilterPage filter)
        {
            try
            {
                ProcessFilters(filter);
                var data = _repo.GetTrackingList(filter);
                if (data != null)
                {
                    response.Result = Common.SuccessMessage("Success");
                    response.Data = data;
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
        private void ProcessFilters(TrackingFilterPage filter)
        {
            if (filter.filters == null) filter.filters = new TrackingFilter();
            if (!string.IsNullOrEmpty(filter.filters.activity_encrypted))
            {
                filter.filters.activityid = Common.ToInt(Common.Decrypt(filter.filters.activity_encrypted));
            }
            else
            {
                filter.filters.activityid = 0;
            }
            if (string.IsNullOrWhiteSpace(filter.filters.startdate))
                filter.filters.startdate = null;
            if (string.IsNullOrWhiteSpace(filter.filters.enddate))
                filter.filters.enddate = null;
        }
        public APIResponse SaveTracking(SaveTrackingRequestDTO request)
        {
            try
            {
                if (request == null)
                {
                    response.Result = Common.ErrorMessage("Invalid request.");
                    return response;
                }

                if (request.Tracking == null)
                {
                    response.Result = Common.ErrorMessage("Tracking details are missing.");
                    return response;
                }

                var model = request.Tracking;

                if (string.IsNullOrWhiteSpace(model?.Moves?.EncryptedValue))
                {
                    response.Result = Common.ErrorMessage("Please select a move type.");
                    return response;
                }

                if (!string.IsNullOrWhiteSpace(model?.NextMoves?.EncryptedValue))
                {
                    if (string.IsNullOrWhiteSpace(model.NextLocationDetailId?.EncryptedValue) &&
                        string.IsNullOrWhiteSpace(model.NextVoyageId?.EncryptedValue))
                    {
                        response.Result = Common.ErrorMessage("Please select next location or voyage.");
                        return response;
                    }

                    if (string.IsNullOrWhiteSpace(model.NextDateTime))
                    {
                        response.Result = Common.ErrorMessage("Please select next date time.");
                        return response;
                    }
                }

                if (!string.IsNullOrWhiteSpace(model?.NextLocationDetailId?.EncryptedValue) ||
                    !string.IsNullOrWhiteSpace(model?.NextDateTime))
                {
                    if (string.IsNullOrWhiteSpace(model?.NextMoves?.EncryptedValue))
                    {
                        response.Result = Common.ErrorMessage("Please select next move.");
                        return response;
                    }
                }

                if (!string.IsNullOrWhiteSpace(model.CurrentVoyageId?.EncryptedValue))
                    model.LocationDetailId.EncryptedValue = "";
                else if (!string.IsNullOrWhiteSpace(model.LocationDetailId?.EncryptedValue))
                    model.CurrentVoyageId.EncryptedValue = "";

                if (!string.IsNullOrWhiteSpace(model.NextVoyageId?.EncryptedValue))
                    model.NextLocationDetailId.EncryptedValue = "";
                else if (!string.IsNullOrWhiteSpace(model.NextLocationDetailId?.EncryptedValue))
                    model.NextVoyageId.EncryptedValue = "";
                // ADD or EDIT
                long trackingId = Common.Decrypt(model.TrackingId.EncryptedValue);

                if (trackingId == 0)
                {
                    // ADD (Bulk)
                    if (request.Selections == null || request.Selections.Count == 0)
                    {
                        response.Result = Common.ErrorMessage("Please select at least one container.");
                        return response;
                    }

                    Result pickResult = _repo.SavePickSelection(request.Selections);

                    if (pickResult == null)
                    {
                        response.Result = Common.ErrorMessage("Unable to create pick selection.");
                        return response;
                    }

                    model.PickSelectionUuid = pickResult.TargetUUID;
                }
                else
                {
                    // EDIT
                    model.PickSelectionUuid = "";
                }

                response.Result = _repo.SaveTracking(model);
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return response;
        }
      
        public APIResponse GetTrackingDetails(string containeruuid, string bookinguuid)
        {
            APIResponse response = new APIResponse();

            try
            {
                if (string.IsNullOrWhiteSpace(containeruuid) || string.IsNullOrWhiteSpace(bookinguuid))
                {
                    response.Result = Common.ErrorMessage("Invalid input");
                    return response;
                }

                // Get Booking Details
                var bookingResponse = _bookingService.GetBookingByUUID(bookinguuid);
                var booking = bookingResponse.Data as ContainerBooking;

                if (booking == null)
                {
                    response.Result = Common.ErrorMessage("Booking not found");
                    return response;
                }

                TrackingBookingSummaryDTO bookingSummary = new TrackingBookingSummaryDTO
                {
                    bookingid = booking.booking.bookingid,
                    bookinguuid = booking.booking.bookinguuid,
                    bookingno = booking.booking.bookingno,
                    customer = booking.booking.customer,
                    location = booking.booking.location
                };
                List<TrackingDetailsDTO> trackingList = _repo.GetTrackingDetails(containeruuid, bookinguuid);
                if (trackingList == null)
                {
                    trackingList = new List<TrackingDetailsDTO>();
                }
                if (trackingList.Any())
                {
                    AddNextMoveOfLastMove(trackingList);
                    AddVoyageOfBooking(trackingList, booking);

                    response.Result = Common.SuccessMessage("Success");
                }
                else
                {
                    response.Result = Common.SuccessMessage("Booking found. No tracking data available.");
                }

                response.Data = new TrackingDetails
                {
                    booking = bookingSummary,
                    Trackingdetails = trackingList
                };
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }

            return response;
        }
        private void AddNextMoveOfLastMove(List<TrackingDetailsDTO> list)
        {
            try
            {
                if (list.Count > 0)
                {
                    var lastmove = list.Last();
                    if (lastmove.NextMoveId.NumericValue > 0)
                    {
                        list.Add(new TrackingDetailsDTO()
                        {
                            MoveTypeId = lastmove.NextMoveId,
                            CurrentMovesName = lastmove.NextMovesName,
                            CurrentMovesIcon = lastmove.NextMovesIcon,
                           // LocationDetailId = lastmove.NextLocationDetailId,
                            CurrentLocationName = lastmove.NextLocationName,
                            CurrentLocationCode = lastmove.NextLocationCode,
                            CurrentCountryCode = lastmove.NextCountryCode,
                            CurrentCountryFlag = lastmove.NextCountryFlag,
                           // CurrentPortId = lastmove.NextPortId,
                            CurrentPortCode = lastmove.NextPortCode,
                            CurrentPortName = lastmove.NextPortName,
                            RecordDateTime = lastmove.NextDateTime,
                            CurrentCountryName = lastmove.NextCountryName,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }

        }
        private void AddVoyageOfBooking(List<TrackingDetailsDTO> list, ContainerBooking booking)
        {
            try
            {
                list.Add(new TrackingDetailsDTO()
                {
                    MoveTypeId = new EncryptedData(),
                    CurrentMovesName = "Port of Discharge",
                    CurrentMovesIcon = "",
                  //  LocationDetailId = new EncryptedData(),
                    CurrentLocationName = "",
                    CurrentLocationCode = "",
                    CurrentCountryCode = "",
                    CurrentCountryFlag = "",
                   // CurrentPortId = new EncryptedData(),
                    //CurrentPortCode = booking.voyage.Vesselname,
                    //CurrentPortName = booking.voyage.VoyageNumber,
                    RecordDateTime = booking.booking.location.pod_portname,
                    CurrentCountryName = "",
                });
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
        }
        public APIResponse GetTrackingByUUID(string trackinguuid)
        {
            try
            {
                var dto = _repo.GetTrackingByUUID(trackinguuid);
                if (dto != null && !string.IsNullOrEmpty(dto.TrackingUuid))
                {
                    response.Result = Common.SuccessMessage("Success");
                    response.Data = dto;
                }
                else
                {
                    response.Result = Common.ErrorMessage("No data found");
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return response;
        }

        public APIResponse SaveTempTracking(CreateTempTrackingRequest request)
        {
            try
            {               
                int containerId = Common.Decrypt(request.containerid.EncryptedValue);
                int bookingId = Common.Decrypt(request.bookingid.EncryptedValue);              
                var result = _repo.SaveTempTracking(containerId, bookingId);
                response.Result = result;
                if (result != null && result.ResultId == 1)
                {
                    response.Data = new
                    {
                        TrackingId = Common.Encrypt(result.TargetID),
                        TrackingUuid = result.TargetUUID
                    };
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return response;
        }
    }
}
