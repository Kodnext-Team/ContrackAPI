namespace ContrackAPI
{
    public class VoyageService : CustomException, IVoyageService
    {
        private readonly IVoyageRepository _repo;
        public VoyageService(IVoyageRepository repo)
        {
            _repo = repo;
        }
        public List<VoyageDTO> GetDirectVoyageSearch(string originportid, string destinationportid)
        {
            try
            {
                var list = _repo.GetDirectVoyageSearch(originportid, destinationportid);
                return list;
            }
            catch (Exception ex)
            {
                RecordException(ex);
                return new List<VoyageDTO>();
            }
        }
    }
}