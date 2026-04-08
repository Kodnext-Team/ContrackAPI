namespace ContrackAPI
{
    public interface ITrackingRepository
    {
        List<TrackingListDTO> GetTrackingList(TrackingFilterPage filter);
        Result SaveTracking(TrackingDTO tracking);
        List<TrackingDetailsDTO> GetTrackingDetails(string containerUuid, string bookingUuid);


    }
}
