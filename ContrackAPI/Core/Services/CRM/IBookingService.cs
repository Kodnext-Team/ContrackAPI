namespace ContrackAPI
{
    public interface IBookingService
    {
        APIResponse GetBookingList(BookingListFilter filter);
        APIResponse GetBookingByUUID(string bookinguuid);
        APIResponse GetContainerSelection(string bookinguuid);
        APIResponse SaveContainerSelection(ContainerSelection bookingmodel);



    }
}