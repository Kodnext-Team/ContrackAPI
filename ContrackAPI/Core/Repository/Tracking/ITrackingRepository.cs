namespace ContrackAPI
{
    public interface ITrackingRepository
    {
        List<TrackingListDTO> GetTrackingList(TrackingFilterPage filter);
        Result SaveTracking(TrackingDTO tracking);
        List<TrackingDetailsDTO> GetTrackingDetails(string containerUuid, string bookingUuid);

        TrackingDTO GetTrackingByUUID(string trackinguuid);
        Result SavePickSelection(List<TrackingSelectionDTO> containerBookingSelection);
        Result SaveTempTracking(int containerId, int bookingId);
    }
}
