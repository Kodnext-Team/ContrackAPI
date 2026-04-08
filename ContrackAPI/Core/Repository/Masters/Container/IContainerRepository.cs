namespace ContrackAPI
{
    public interface IContainerRepository
    {
        List<ContainerDTO> GetContainerList(ContainerFilterPage filter);
        ContainerDTO GetContainerByUUID(string containeruuid);

    }
}