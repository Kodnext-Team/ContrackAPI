namespace ContrackAPI
{
    public class HubService : CustomException, IHubService
    {
        public Result result = new Result();
        private readonly IHubRepository _repo;
        public HubService(IHubRepository repo)
        {
            _repo = repo;
        }
        public HubDTO GetHubByID(int HubID)
        {
            HubDTO hub = _repo.GetHubByID(HubID);
            return hub;
        }
    }

}
