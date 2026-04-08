namespace ContrackAPI
{
    public interface ITrackingService
    {
        List<TrackingListDTO> GetTrackingList(TrackingFilterPage filter);
        public Result SaveTracking(TrackingDTO tracking);
        TrackingDetails GetTrackingDetails(string containeruuid, string bookinguuid, ContainerBooking booking);


    }
}
