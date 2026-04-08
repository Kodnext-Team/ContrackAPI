namespace ContrackAPI
{
    public interface IContainerService
    {
        List<ContainerDTO> GetContainerList(ContainerFilterPage filter);
        ContainerModal GetContainerByUUID(string containeruuid);

    }
}