namespace ContrackAPI
{
    public interface IBookingService
    {
        APIResponse GetBookingList(BookingListFilter filter);
        APIResponse GetPortCombinationList();
        APIResponse GetBookingByUUID(string bookinguuid);
        APIResponse GetContainerSelection(string bookinguuid);
        APIResponse SaveContainerSelection(ContainerSelection bookingmodel);
        APIResponse GetBookedContainers(string bookinguuid, BookedContainerFilter filter);
    }
}