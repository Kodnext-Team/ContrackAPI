namespace ContrackAPI
{
    public interface IVoyageService
    {
        APIResponse GetDirectVoyageSearch(string originportid, string destinationportid);
        APIResponse GetVoyageByUUID(string voyageuuid);
        APIResponse GetVoyageList(VoyageFilter filter);


    }
}