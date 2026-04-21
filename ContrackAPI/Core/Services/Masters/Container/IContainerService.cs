namespace ContrackAPI
{
    public interface IContainerService
    {
        APIResponse GetContainerList(ContainerFilterPage filter);
        APIResponse GetContainerByUUID(string containeruuid);

    }
}