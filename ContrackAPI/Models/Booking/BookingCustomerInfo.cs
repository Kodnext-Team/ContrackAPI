using ContrackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class ContainerBooking
    {
        public ContainerBookingDTO booking { get; set; } = new ContainerBookingDTO();
        public BookingVoyageDTO voyage { get; set; } = new BookingVoyageDTO();
        //public BookingSummaryDTO bookingSummary { get; set; } = new BookingSummaryDTO();
    }

}