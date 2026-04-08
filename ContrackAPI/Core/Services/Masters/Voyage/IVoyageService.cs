namespace ContrackAPI
{
    public interface IVoyageService
    {
        List<VoyageDTO> GetDirectVoyageSearch(string originportid, string destinationportid);
    }
}