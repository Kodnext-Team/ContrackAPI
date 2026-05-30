namespace ContrackAPI
{
    public class VoyageService : CustomException, IVoyageService
    {
        private readonly IVoyageRepository _repo;
        APIResponse response = new APIResponse();

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
        public APIResponse GetVoyageByUUID(string containeruuid)
        {
            try
            {
                var dto = _repo.GetVoyageByUUID(containeruuid);
                
            }
            catch (Exception ex)
            {
                RecordException(ex);
                response.Result = Common.ErrorMessage(ex.Message);
            }
            return response;
        }

    }
}