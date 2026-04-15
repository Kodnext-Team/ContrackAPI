using System.Text.Json.Serialization;
namespace ContrackAPI
{
    public class UserExtra
    {
        public string shortcode { get; set; } = "";
        public string color { get; set; } = "";
        public string bgcolor { get; set; } = "";
    }
    public class UserDTO
    {
        public int row_index { get; set; } = 0;
        public int totalnoofrows { get; set; } = 0;
        public EncryptedData UserID { get; set; } = new EncryptedData();
        public EncryptedData Type { get; set; } = new EncryptedData();
        public EncryptedData RoleID { get; set; } = new EncryptedData();
        public List<string> EntityIDEncryptedList { get; set; } = new List<string>();
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public int Status { get; set; } = 0;

        [JsonIgnore]
        public string Password { get; set; } = "";
        public string Salt { get; set; } = "";
        public string TypeName { get; set; } = "";
        public string EntityName { get; set; } = "";
        public string RoleName { get; set; } = "";
        public string RoleIcon { get; set; } = "";
        public DateTime DateTimeCreated { get; set; }
        public int HubID { get; set; } = 0;
        public UserExtra extras { get; set; } = new UserExtra();

    }
}
