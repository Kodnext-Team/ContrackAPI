using ContrackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class ContainerSelectionDTO
    {
        public string LocationName { get; set; } = "";
        public EncryptedData LocationId { get; set; } = new EncryptedData();
        public string LocationUuid { get; set; } = "";
        public string LocationType { get; set; } = "";
        public int LocationTypeIcon { get; set; } = 0;
        public string PortName { get; set; } = "";
        public string CountryName { get; set; } = "";
        public string CountryFlag { get; set; } = "";
        public string PortCode { get; set; } = "";
        public string CountryCode { get; set; } = "";
        public List<ContainerSelectionDetailDTO> Details { get; set; } = new List<ContainerSelectionDetailDTO>();
    }
    public class ContainerSelectionDetailDTO
    {
        public EncryptedData ReservationID { get; set; } = new EncryptedData();
        public EncryptedData ContainerDetailID { get; set; } = new EncryptedData();
        public string ContainerTypeName { get; set; } = "";
        public string ContainerSizeName { get; set; } = "";
        public EncryptedData ContainerModelId { get; set; } = new EncryptedData();
        public string ContainerModelUuid { get; set; } = "";
        public string ContainerModelIso { get; set; } = "";
        public int RequiredCount { get; set; } = 0;
        public List<SelectionItemDTO> Containers { get; set; } = new List<SelectionItemDTO>();

    }
    public class SelectionItemDTO
    {
        public EncryptedData ContainerID { get; set; } = new EncryptedData();
        public string ContainerUUID { get; set; } = "";
        public string EquipmentNo { get; set; } = "";
        public bool Selected { get; set; } = false;
        public bool Allotted { get; set; } = false;
        public bool Locked { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public string AllocationBookingUUID { get; set; } = "";
        public FormattedValue<DateTime> AllocationDateTime { get; set; } = new FormattedValue<DateTime>();
        public FormattedValue<DateTime> LastBookingDate { get; set; } = new FormattedValue<DateTime>();

    }


    public class ContainerAllottedDTO
    {
        public EncryptedData containerid { get; set; } = new EncryptedData();
    }
}