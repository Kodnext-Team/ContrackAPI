namespace ContrackAPI
{
    public interface IBookingRepository
    {
        List<ContainerBookingListDTO> GetbookingList(BookingListFilter filter);
        List<ContainerBookingDetailDTO> GetContainerBookingDetailByBookingUUId(string bookingId);
        ContainerBookingDTO GetbookingByUUID(string bookinguuid);
        BookingCustomerDTO GetBookingCustomerInfo(string bookinguuid, ContainerBookingDTO parent);
        BookingLocationDTO GetBookingLocationInfo(string bookinguuid, ContainerBookingDTO parent);
        BookingSummaryDTO GetBookingSummaryInfo(string bookinguuid);
        List<BookingAdditionalServicesDTO> GetBookingAdditionalServices(string bookinguuid);
        List<ContainerSelectionDTO> GetContainerSelection(string bookinguuid);
        List<ContainerAllottedDTO> GetContainerAllotment(string bookinguuid);

    }
}