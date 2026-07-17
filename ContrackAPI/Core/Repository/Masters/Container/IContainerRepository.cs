namespace ContrackAPI
{
    public interface IContainerRepository
    {
        List<ContainerDTO> GetContainerList(ContainerFilterPage filter);
        ContainerDetailDTO GetContainerByUUID(string containeruuid);
        ContainerEquipmentResponseDTO GetContainerByEquipmentno(string equipmentno);
    }
}