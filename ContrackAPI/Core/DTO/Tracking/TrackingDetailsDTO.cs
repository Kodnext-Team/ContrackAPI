namespace ContrackAPI
{
    public class TrackingDetailsDTO
    {
        public EncryptedData TrackingId { get; set; } = new EncryptedData();
        public string TrackingUuid { get; set; } = "";
        public EncryptedData ContainerId { get; set; } = new EncryptedData();
        public string ContainerUuid { get; set; } = "";
        public EncryptedData BookingId { get; set; } = new EncryptedData();
        public string BookingUuid { get; set; } = "";
        public EncryptedData MoveTypeId { get; set; } = new EncryptedData();
        public string CurrentMovesName { get; set; } = "";
        public string CurrentMovesIcon { get; set; } = "";
        public EncryptedData LocationDetailId { get; set; } = new EncryptedData();
        public string LocationUuid { get; set; } = "";
        public string CurrentLocationName { get; set; } = "";
        public string CurrentLocationCode { get; set; } = "";
        public EncryptedData CurrentPortId { get; set; } = new EncryptedData();
        public string CurrentLocationTypeName { get; set; } = "";
        public string CurrentPortName { get; set; } = "";
        public string CurrentPortCode { get; set; } = "";
        public string CurrentCountryName { get; set; } = "";
        public string CurrentCountryCode { get; set; } = "";
        public string CurrentCountryFlag { get; set; } = "";
        public string RecordDateTime { get; set; } = "";
        public EncryptedData NextMoveId { get; set; } = new EncryptedData();
        public string NextMovesName { get; set; } = "";
        public string NextMovesIcon { get; set; } = "";
        public EncryptedData NextLocationDetailId { get; set; } = new EncryptedData();
        public string NextLocationUuid { get; set; } = "";
        public string NextLocationName { get; set; } = "";
        public string NextLocationCode { get; set; } = "";
        public EncryptedData NextPortId { get; set; } = new EncryptedData();
        public string NextLocationTypeName { get; set; } = "";
        public string NextPortName { get; set; } = "";
        public string NextPortCode { get; set; } = "";
        public string NextCountryName { get; set; } = "";
        public string NextCountryCode { get; set; } = "";
        public string NextCountryFlag { get; set; } = "";
        public string NextDateTime { get; set; } = "";
        public bool IsDamaged { get; set; } = false;
        public int NoOfDamages { get; set; } = 0;
        public string ContainerEquipmentno { get; set; } = "";
        public string ContainerTypeName { get; set; } = "";
        public string ContainerSizeName { get; set; } = "";
        public string CreatedAt { get; set; } = "";
        public EncryptedData CreatedBy { get; set; } = new EncryptedData();
        public bool IsDeleted { get; set; }
        public EncryptedData DeletedBy { get; set; } = new EncryptedData();
        public string DeletedAt { get; set; } = "";
    }
}
