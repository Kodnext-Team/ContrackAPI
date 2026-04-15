namespace ContrackAPI
{
    public interface IBookingService
    {
        List<ContainerBookingListDTO> GetBookingList(BookingListFilter filter);
        ContainerBooking GetbookingByUUID(string bookinguuid);
        ContainerSelection GetContainerSelection(string bookinguuid);


    }
}