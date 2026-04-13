namespace ContrackAPI
{
    public interface IBookingService
    {
        List<ContainerBookingListDTO> GetBookingList(BookingListFilter filter);
        ContainerBooking GetbookingByUUID(string bookinguuid, bool getsummary = false);
        ContainerSelection GetContainerSelection(string bookinguuid);


    }
}