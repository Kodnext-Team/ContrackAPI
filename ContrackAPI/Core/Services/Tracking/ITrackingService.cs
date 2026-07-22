namespace ContrackAPI
{
    public interface ITrackingService
    {
        APIResponse GetTrackingList(TrackingFilterPage filter);
        APIResponse SaveTracking(SaveTrackingRequestDTO tracking);
        APIResponse GetTrackingDetails(string containeruuid, string bookinguuid);
        APIResponse GetTrackingByUUID(string trackinguuid);
        APIResponse SaveTempTracking(CreateTempTrackingRequest request);
      //  void SavePickSelection(List<TrackingSelectionDTO> model);



    }
}
