namespace ContrackAPI
{
    public class MovesDTO
    {
        public EncryptedData MovesId { get; set; } = new EncryptedData();
        public string MovesUuid { get; set; } = "";
        public string MovesName { get; set; } = "";
        public int IconId { get; set; } = 0;
        public string IconPath { get; set; } = "";
        public string SelectedIcon { get; set; } = "";
        public string locationicon { get; set; } = "";
        public string InOut { get; set; } = "";
        public string FullEmptyNone { get; set; } = "";
        public bool DontShowPublic { get; set; } = false;
        public bool ShowVoyage { get; set; } = false;
        public CreationInfo CreationInfo { get; set; } = new CreationInfo();
    }
}