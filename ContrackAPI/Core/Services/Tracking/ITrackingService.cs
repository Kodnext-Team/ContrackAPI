namespace ContrackAPI
{
    public interface ITrackingService
    {
        APIResponse GetTrackingList(TrackingFilterPage filter);
        APIResponse SaveTracking(TrackingDTO tracking);
        APIResponse GetTrackingDetails(string containeruuid, string bookinguuid);


    }
}
