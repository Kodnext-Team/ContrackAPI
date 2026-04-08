namespace ContrackAPI
{
    public interface IVoyageRepository
    {
        List<VoyageDTO> GetDirectVoyageSearch(string originPortId, string destinationPortId);
        VoyageDTO GetVoyageByUUID(string uuid);

    }
}