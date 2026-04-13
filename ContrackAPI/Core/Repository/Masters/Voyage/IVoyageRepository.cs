namespace ContrackAPI
{
    public interface IVoyageRepository
    {
        List<VoyageDTO> GetDirectVoyageSearch(string originPortId, string destinationPortId);
        VoyageDTO GetVoyageByUUID(string uuid);
        List<VoyageDTO> SearchVoyage(string search, bool createnew);


    }
}