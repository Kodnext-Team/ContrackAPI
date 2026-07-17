using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class TrackingSelectionDTO
    {
        public EncryptedData ContainerId { get; set; } = new EncryptedData();
        public EncryptedData BookingId { get; set; } = new EncryptedData();
    }

}