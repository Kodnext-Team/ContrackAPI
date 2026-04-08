using System.Text.Json.Serialization;

namespace ContrackAPI
{
    public class LoginDTO
    {
        public EncryptedData UserID { get; set; } = new EncryptedData();
        public EncryptedData Type { get; set; } = new EncryptedData();
        public string EntityName { get; set; } = "";
        public List<string> EntityIDEncryptedList { get; set; } = new List<string>();
        public string UserName { get; set; } = "";
        [JsonIgnore]
        public string Password { get; set; } = "";
        public string Name { get; set; } = "";
        public int Status { get; set; } = 0;
        public string TypeName { get; set; } = "";
        public string Salt { get; set; } = "";
        public DateTime DateTimeCreated { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public int HubID { get; set; } = 0;
        public string AssociateID { get; set; } = "";
        public int EntityID { get; set; } = 0;
        public EncryptedData RoleID { get; set; } = new EncryptedData();
        public string RoleName { get; set; } = "";
        public string RoleIcon { get; set; } = "";
        public long LoginID { get; set; } = 0;
        public List<int> AssociateIDList { get; set; } = new List<int>();
    }

}
