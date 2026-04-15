namespace ContrackAPI
{
    public class ContainerSelection
    {
        public string UniqueKey { get; set; } = Guid.NewGuid().ToString();
        public List<ContainerSelectionDTO> Selections { get; set; } = new List<ContainerSelectionDTO>();
        public List<ContainerAllottedDTO> Allotted { get; set; } = new List<ContainerAllottedDTO>();
        public ContainerBookingDTO Booking { get; set; } = new ContainerBookingDTO();
    }
}