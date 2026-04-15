namespace ContrackAPI
{
    public class TrackingService : CustomException, ITrackingService
    {
        private readonly ITrackingRepository _repo;
        public TrackingService(ITrackingRepository repo)
        {
            _repo = repo;
        }
        public List<TrackingListDTO> GetTrackingList(TrackingFilterPage filter)
        {
            try
            {
                ProcessFilters(filter);
                return _repo.GetTrackingList(filter);
            }
            catch (Exception ex)
            {
                RecordException(ex);
                return new List<TrackingListDTO>();
            }
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
        public Result SaveTracking(TrackingDTO tracking)
        {
            Result result = new Result();
            try
            {
                var model = tracking;
                if (string.IsNullOrWhiteSpace(model?.Moves?.EncryptedValue))
                    return Common.ErrorMessage("Please select a move type.");
                if (!string.IsNullOrWhiteSpace(model?.NextMoves?.EncryptedValue))
                {
                    if (string.IsNullOrWhiteSpace(model.NextLocationDetailId?.EncryptedValue) &&
                        string.IsNullOrWhiteSpace(model.NextVoyageId?.EncryptedValue))
                        return Common.ErrorMessage("Please select next location or voyage");
                    if (string.IsNullOrWhiteSpace(model.NextDateTime))
                        return Common.ErrorMessage("Please select next date time.");
                }
                if (!string.IsNullOrWhiteSpace(model?.NextLocationDetailId?.EncryptedValue) ||
                    !string.IsNullOrWhiteSpace(model?.NextDateTime))
                {
                    if (string.IsNullOrWhiteSpace(model?.NextMoves?.EncryptedValue))
                        return Common.ErrorMessage("Please select next move.");
                }
                if (!string.IsNullOrWhiteSpace(model.CurrentVoyageId?.EncryptedValue))
                    model.LocationDetailId.EncryptedValue = "";
                else if (!string.IsNullOrWhiteSpace(model.LocationDetailId?.EncryptedValue))
                    model.CurrentVoyageId.EncryptedValue = "";

                if (!string.IsNullOrWhiteSpace(model.NextVoyageId?.EncryptedValue))
                    model.NextLocationDetailId.EncryptedValue = "";
                else if (!string.IsNullOrWhiteSpace(model.NextLocationDetailId?.EncryptedValue))
                    model.NextVoyageId.EncryptedValue = "";
                return _repo.SaveTracking(model);
            }
            catch (Exception ex)
            {
                RecordException(ex);
                return Common.ErrorMessage("Error while saving tracking");
            }
        }
        public TrackingDetails GetTrackingDetails(string containeruuid, string bookinguuid, ContainerBooking booking)
        {
            List<TrackingDetailsDTO> list = new List<TrackingDetailsDTO>();
            try
            {
                if (!string.IsNullOrEmpty(containeruuid) && !string.IsNullOrEmpty(bookinguuid))
                {
                    list = _repo.GetTrackingDetails(containeruuid, bookinguuid);

                    if (list.Count > 0)
                    {
                        AddNextMoveOfLastMove(list);
                        AddVoyageOfBooking(list, booking);
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }

            return new TrackingDetails()
            {
                Trackingdetails = list
            };
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
                            LocationDetailId = lastmove.NextLocationDetailId,
                            CurrentLocationName = lastmove.NextLocationName,
                            CurrentLocationCode = lastmove.NextLocationCode,
                            CurrentCountryCode = lastmove.NextCountryCode,
                            CurrentCountryFlag = lastmove.NextCountryFlag,
                            CurrentPortId = lastmove.NextPortId,
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
                    LocationDetailId = new EncryptedData(),
                    CurrentLocationName = "",
                    CurrentLocationCode = "",
                    CurrentCountryCode = "",
                    CurrentCountryFlag = "",
                    CurrentPortId = new EncryptedData(),
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
    }
}
