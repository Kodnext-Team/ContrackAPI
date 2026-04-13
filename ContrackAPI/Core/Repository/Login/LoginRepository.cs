using System.Data;

namespace ContrackAPI
{
    public class LoginRepository : CustomException, ILoginRepository
    {
        public Result ValidateLogin(LoginDTO login)
        {
            Result result = new Result();
            try
            {
                using (SqlDB Db = new SqlDB())
                {
                    DataTable tbl = Db.GetDataTable("SELECT * FROM masters.login_validation('" + login.UserName + "');");
                    if (tbl.Rows.Count > 0)
                    {
                        int ResultID = Common.ToInt(tbl.Rows[0]["resultid"]);
                        string ResultMsg = Common.ToString(tbl.Rows[0]["resultmessage"]);
                        if (ResultID == 1)
                        {
                            string DBHash = Common.ToString(tbl.Rows[0]["passwordhash"]);
                            string DBSalt = Common.ToString(tbl.Rows[0]["salt"]);
                            bool isvalid = Encryption.IsPasswordValid(login.Password, DBSalt + ":" + DBHash);
                            if (isvalid)
                            {
                                result = Common.SuccessMessage("Login Success");
                                login.UserID = new EncryptedData { NumericValue = Common.ToInt(tbl.Rows[0]["userid"]) };
                                login.UserName = Common.ToString(tbl.Rows[0]["username"]);
                                login.Name = Common.ToString(tbl.Rows[0]["fullname"]);
                                login.Type = new EncryptedData { NumericValue = Common.ToInt(tbl.Rows[0]["usertype"]) };
                                login.HubID = Common.ToInt(tbl.Rows[0]["hubid"]);
                                login.RoleID = new EncryptedData { NumericValue = Common.ToInt(tbl.Rows[0]["role_id"]) };
                                login.RoleName = Common.ToString(tbl.Rows[0]["role_name"]);
                            }
                            else
                            {
                                result = Common.ErrorMessage("Invalid Username/Password");
                            }
                        }
                        else
                        {
                            result = Common.ErrorMessage(ResultMsg);
                        }
                    }
                    else
                    {
                        result = Common.ErrorMessage("Your account doesn't exist");
                    }
                }
            }
            catch (Exception ex)
            {
                result = Common.ErrorMessage(ex.Message.ToString());
                RecordException(ex);
            }
            return result;
        }

        public UserDTO GetUserByID(int userId)
        {
            UserDTO user = new UserDTO();

            try
            {
                using (var Db = new SqlDB())
                {
                    DataTable tbl = Db.GetDataTable(
                        $"SELECT * FROM masters.getuserbyid('{Common.HubID}','{userId}');"
                    );

                    if (tbl.Rows.Count > 0)
                    {
                        DataRow row = tbl.Rows[0];

                        user.UserID = new EncryptedData
                        {
                            NumericValue = Common.ToInt(row["userid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(row["userid"]))
                        };

                        user.UserName = Common.ToString(row["username"]);
                        user.Name = Common.ToString(row["fullname"]);
                        user.Email = Common.ToString(row["email"]);
                        user.Phone = Common.ToString(row["phone"]);
                        user.Salt = Common.ToString(row["salt"]);
                        user.Password = Common.ToString(row["passwordhash"]);

                        user.Type = new EncryptedData
                        {
                            NumericValue = Common.ToInt(row["usertypeid"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(row["usertypeid"]))
                        };

                        user.TypeName = Common.ToString(row["usertype"]);

                        user.EntityName = string.Join(",",
                            tbl.AsEnumerable()
                               .Select(r => Common.ToString(r["entity_name"]))
                               .Distinct());

                        user.EntityIDEncryptedList = tbl.AsEnumerable()
                            .Select(r => Common.Encrypt(Common.ToInt(r["entityid"])))
                            .Distinct()
                            .ToList();

                        user.DateTimeCreated = Common.ToDateTime(row["createdat"]);
                        user.Status = Common.ToInt(row["isactive"]);

                        user.RoleID = new EncryptedData
                        {
                            NumericValue = Common.ToInt(row["role_id"]),
                            EncryptedValue = Common.Encrypt(Common.ToInt(row["role_id"]))
                        };

                        user.RoleName = Common.ToString(row["role_name"]);
                        user.RoleIcon = Common.ToString(row["role_icon"]);
                    }
                }
            }
            catch (Exception ex)
            {
                RecordException(ex);
            }

            return user;
        }
    }
}