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
        public APIResponse GetDirectVoyageSearch(string originportid, string destinationportid)
        {
            var response = new APIResponse();
            try
            {
                var data = _repo.GetDirectVoyageSearch(originportid, destinationportid);
                if (data == null)
                {
                    response.Result = Common.ErrorMessage("No data found");
                }
                else
                {
                    response.Result = Common.SuccessMessage("Success");
                    response.Data = data;
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return response;
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
        public APIResponse GetVoyageList(VoyageFilter filter)
        {
            var response = new APIResponse();
            try
            {
                var data = _repo.GetVoyageList(filter);
                if (data == null)
                {
                    response.Result = Common.ErrorMessage("No data found");
                }
                else
                {
                    response.Result = Common.SuccessMessage("Success");
                    response.Data = data;
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }
            return response;
        }
    }
}