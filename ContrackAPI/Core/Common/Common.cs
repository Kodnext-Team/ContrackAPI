using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Newtonsoft.Json;
namespace ContrackAPI
{

    public class TableCounts
    {
        public int totalnoofrows { get; set; } = 0;
        public int row_index { get; set; } = 0;
    }
    public class FormatDisplay
    {
        public string Text { get; set; }
        public string Delimiter { get; set; }
        public string Name { get; set; }
    }
    public class dropdown
    {
        public string id { get; set; }
        public string text { get; set; }
    }
    public class EncryptedData
    {
        public int NumericValue { get; set; } = 0;
        public string EncryptedValue { get; set; } = "";
    }
    public class KeyValuePair
    {
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public string KeyName { get; set; } = "";
        public string KeyValue { get; set; } = "";
    }
    public class NotFound
    {
        public string PageName { get; set; }
        public string Message { get; set; }
        public string ReturnURL { get; set; }
        public string ReturnButton { get; set; }
    }
    public class Result
    {
        public int ResultId { get; set; }
        public string ResultMessage { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string TargetUUID { get; set; } = "";
        [System.Text.Json.Serialization.JsonIgnore]
        public int TargetID { get; set; } = 0;
    }
    public class LogResult
    {
        public string LogChangeKey { get; set; } // -- Modification
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int Type { get; set; } // 1 - Added, 2- Edited
    }
    public class LogNames
    {
        public int DetailID { get; set; } = 0;
        public string Type { get; set; } = "";
        public string Name { get; set; } = "";
    }
    public class Roles
    {
        public int role_id { get; set; } = 0;
        public string role_name { get; set; } = "";
        public string description { get; set; } = "";
        public string icon { get; set; } = "";
    }

    public static class Common
    {
        public const string AESKey = "4F67A92B13D4E9FC1A7B8C3D5E6F0123";
        public const string AESIV = "A1B2C3D4E5F60789";
        public const string DBDateTimeformat = "yyyy-MM-dd HH:mm";
        public const string DBDateformat = "yyyy-MM-dd";
        public const string HumanDateTimeformat = "MMM dd, yyyy h:mm tt";
        public const string HumanDateformat = "MMM dd, yyyy";
        public const decimal CurrencyAdjustThreshold = -1;
        public const string VesselName = "Ship / Cost center";
        public const int MyAppID = 2;

        private static readonly (string Color, string Bg)[] ColorList = {
            ("#872094", "#FCE7FF"), // Purple bg → white text
            ("#944920", "#FFEADE"), // Brown bg → white text
            ("#217F1A", "#E6FFE4"), // Green bg → white text
            ("#205C94", "#DEF0FF")  // Blue bg → white text
        };
        public static (string Color, string Bg) GetColorFromName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return ("#808080", "#FFFFFF"); // default gray bg, white text

                name = name.Trim().ToUpper();

                int hash = 0;
                foreach (char c in name)
                {
                    hash = (hash * 31 + c) % 1000;
                }

                int index = hash % ColorList.Length;
                return ColorList[index];
            }
            catch (Exception)
            {
                return ColorList[0];
            }
        }

        public static string FlagFolder
        {
            get
            {
                return "/assets/Flags/";
            }
        }

        public static string IconFolder
        {
            get
            {
                return "/assets/dbicons/";
            }
        }

        public static Result SuccessMessage(string message)
        {
            return new Result { ResultId = 1, ResultMessage = message };
        }

        public static Result ErrorMessage(string message)
        {
            return new Result { ResultId = 0, ResultMessage = message };
        }
        public static string Escape(string input)
        {
            if (input == null)
                input = "";
            input = input.Replace("'", "''");
            return input;
        }

        public static string GetUUID(string input)
        {
            return (string.IsNullOrEmpty(input) ? "null" : ("'" + input + "'"));
        }
        public static double GetClientTimeOffset()
        {
            return 0;
        }

        public static string GetNullDate(DateTime date)
        {
            return (date == DateTime.MinValue ? "null" : ("'" + date.ToString("yyyy-MM-dd hh:mm:ss") + "'"));
        }

        public static string GetNullValue(string input)
        {
            return (string.IsNullOrEmpty(input) ? "null" : ("'" + input + "'"));
        }


        public static string DisplayInfoTableWithAddEdit(string heading, string value, string href, string onclick, bool noaction = false)
        {
            string output = "<td class='py-3 master-detail-heading'>" + heading + "</td>";
            if (string.IsNullOrEmpty(value))
                output += "<td class='py-3 text-gray-700 text-2sm font-medium'>-</td>";
            else
                output += "<td class='py-3 text-gray-700 text-sm font-medium'>" + value + "</td>";

            if (!noaction)
            {
                if (string.IsNullOrEmpty(value))
                    output += "<td class='py-3 text-right'><a class='btn btn-link btn-sm' href='" + href + "' onclick='" + onclick + "'>Add</a></td>";
                else
                    output += "<td class='py-3 text-right'><a class='btn btn-sm btn-icon btn-clear btn-primary' href='" + href + "' onclick='" + onclick + "'><i class='ki-filled ki-notepad-edit'></i></a></td>";
            }
            else
            {
                output += "<td class='py-3 text-right'><a class='btn btn-sm btn-icon' href='javascript:void(0)'><i></i></a></td>";
            }

            return output;
        }
        public static string DisplayInfoTableWithAddEditDelete(string heading, string value, string href, string onclick, string ondelete, bool noaction = false)
        {
            string output = "<td class='py-3 master-detail-heading'>" + heading + "</td>";
            if (string.IsNullOrEmpty(value))
                output += "<td class='py-3 text-gray-700 text-2sm font-medium'>-</td>";
            else
                output += "<td class='py-3 text-gray-700 text-sm font-medium'>" + value + "</td>";

            if (!noaction)
            {
                if (string.IsNullOrEmpty(value))
                    output += "<td class='py-3 text-right'><a class='btn btn-link btn-sm' href='" + href + "' onclick='" + onclick + "'>Add</a></td>";
                else
                    output += "<td class='py-3 text-right'><a class='btn btn-sm btn-icon btn-clear btn-primary' href='" + href + "' onclick='" + onclick + "'><i class='ki-filled ki-notepad-edit'></i></a><a class='btn btn-sm btn-icon btn-clear btn-danger' href='javascript:void(0)' onclick='" + ondelete + "'><i class='ki-filled ki-filled ki-trash'></i></a></td>";
            }
            else
            {
                output += "<td class='py-3 text-right'><a class='btn btn-sm btn-icon' href='javascript:void(0)'><i></i></a></td>";
            }
            return output;
        }
        public static IHttpContextAccessor HttpContextAccessor;
        public static int HubID
        {
            get
            {
                var claim = HttpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "HubId");
                if (claim != null && !string.IsNullOrEmpty(claim.Value))
                {
                    return Convert.ToInt32(claim.Value);
                }
                var hubid = HttpContextAccessor?.HttpContext?.Request?.Query["HubID"].ToString();
                if (string.IsNullOrEmpty(hubid))
                    return 0;
                return Convert.ToInt32(hubid);
            }
        }
        public static int UserID
        {
            get
            {
                var claim = HttpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "UserId");
                if (claim != null && !string.IsNullOrEmpty(claim.Value))
                {
                    return Convert.ToInt32(claim.Value);
                }
                var UserId = HttpContextAccessor?.HttpContext?.Request?.Query["UserId"].ToString();
                if (string.IsNullOrEmpty(UserId))
                    return 0;
                return Convert.ToInt32(UserId);
            }
        }
        public static int ToInt(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? 0 : Convert.ToInt32(data);
            }
            catch (Exception ex)
            {
                return 0;

            }
        }

        public static long ToLong(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? 0 : Convert.ToInt64(data);
            }
            catch (Exception ex)
            {
                return 0;

            }
        }

        public static string ToString(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? "" : Convert.ToString(data);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static int GetETADays(DateTime date, bool dateonly = true)
        {
            try
            {
                var currentdate = Display.GetClientDateTime(DateTime.UtcNow);
                if (dateonly)
                    currentdate = currentdate.Date;

                var datediff = (Convert.ToDateTime(date) - currentdate).Days;
                return datediff;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        
        public static DateTime ToClientDateTime(object data)
        {
            try
            {
              //  return string.IsNullOrEmpty(Convert.ToString(data)) ? DateTime.MinValue : Convert.ToDateTime(data).AddMinutes(Convert.ToDouble(SessionManager.TimeOffSet));

                return string.IsNullOrEmpty(Convert.ToString(data)) ? DateTime.MinValue : Convert.ToDateTime(data).AddMinutes(GetClientTimeOffset());
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }

        public static string GetOperatorName(int operatorId)
        {
            switch (operatorId)
            {
                case 1:
                    return "Owned";
                case 2:
                    return "Leased";
                case 3:
                    return "SOC";
                default:
                    return "COC";
            }
        }
        public static bool ToBool(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? false : Convert.ToBoolean(data);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string[] ToStringArray(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? new string[] { } : (string[])data;
            }
            catch (Exception ex)
            {
                return new string[] { };
            }
        }

        public static string[] ToGUIDStringArray(object data)
        {
            try
            {
                if (data == null || string.IsNullOrEmpty(data.ToString()))
                    return Array.Empty<string>();

                // If it's already a string[]
                if (data is string[] stringArray)
                    return stringArray;

                // If it's a Guid[]
                if (data is Guid[] guidArray)
                    return guidArray.Select(g => g.ToString()).ToArray();

                // If it's a single Guid
                if (data is Guid guidValue)
                    return new[] { guidValue.ToString() };

                // If it's a single string
                if (data is string str)
                    return new[] { str };

                return Array.Empty<string>();
            }
            catch
            {
                return Array.Empty<string>();
            }
        }
        public static int[] ToIntArray(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? new int[] { } : (int[])data;
            }
            catch (Exception ex)
            {
                return new int[] { };
            }
        }
        public static Int64[] ToInt64Array(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? new Int64[] { } : (Int64[])data;
            }
            catch (Exception ex)
            {
                return new Int64[] { };
            }
        }

        public static int[] ToInt32Array(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? new int[] { } : (int[])data;
            }
            catch (Exception ex)
            {
                return new int[] { };
            }
        }


        public static DateTime ToDateTime(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? DateTime.MinValue : Convert.ToDateTime(data);
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }

        //public static DateTime ToClientDateTime(object data)
        //{
        //    try
        //    {
        //        return string.IsNullOrEmpty(Convert.ToString(data)) ? DateTime.MinValue : Convert.ToDateTime(data).AddMinutes(Convert.ToDouble(SessionManager.TimeOffSet));
        //    }
        //    catch (Exception ex)
        //    {
        //        return DateTime.MinValue;
        //    }
        //}
        public static string ToDateTimeString(object data, string format)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(data)))
                    return "";
                else
                    return Convert.ToDateTime(data).ToString(format);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        //public static string GetIconPath(int iconid, bool filenameonly = false)
        //{
        //    var icon = GetIcon(iconid);
        //    string iconfile = icon.icon;
        //    return icon.iscss ? iconfile : (filenameonly ? (IconFolder + iconfile) : "<img class='dynamicicon' src='" + (IconFolder + iconfile) + "'/>");
        //}
        //public static string GetSelectedIconPath(int iconid, bool filenameonly = false)
        //{
        //    var icon = GetIcon(iconid);
        //    string iconfile = icon.iconselected;
        //    return icon.iscss ? iconfile : (filenameonly ? (IconFolder + iconfile) : "<img class='dynamicicon' src='" + (IconFolder + iconfile) + "'/>");
        //}

        public static decimal ToDecimal(object data)
        {
            try
            {
                return string.IsNullOrEmpty(Convert.ToString(data)) ? 0 : Convert.ToDecimal(data);
            }
            catch (Exception ex)
            {
                return 0;

            }
        }

        public static DateTime ToDateTimeHumanFriendly(object data)
        {
            try
            {
                //data = "April 25, 2025 11:55 PM";
                CultureInfo provider = CultureInfo.InvariantCulture;

                DateTime result = DateTime.ParseExact(ToString(data), HumanDateTimeformat, provider);
                return result;
            }
            catch (Exception ex)
            {
                return new DateTime();

            }
        }



        public static string Encrypt(int ID)
        {
            string output = "";
            try
            {
                byte[] bytes = Encryption.Encrypt(ID.ToString(), Encoding.ASCII.GetBytes(AESKey), Encoding.ASCII.GetBytes(AESIV));

                StringBuilder hex = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes)
                    hex.AppendFormat("{0:x2}", b);

                return hex.ToString();

                //string EncryptedID = Convert.ToBase64String(Encryption.Encrypt(ID.ToString(), Encoding.ASCII.GetBytes(AESKey), Encoding.ASCII.GetBytes(AESIV)));
                //output = EncryptedID.Replace('+', '.').Replace('/', '_').Replace('=', '-');
            }
            catch (Exception ex)
            { }
            return output;
        }

        public static int Decrypt(string data)
        {
            int output = 0;
            try
            {
                if (string.IsNullOrEmpty(data))
                    return output;
                //data = data.Replace('.', '+').Replace('_', '/').Replace('-', '=').Replace(" ", "+");
                //output = Convert.ToInt32(Encryption.Decrypt(Convert.FromBase64String(data), Encoding.ASCII.GetBytes(AESKey), Encoding.ASCII.GetBytes(AESIV)));
                int length = data.Length / 2;
                byte[] bytes = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    bytes[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
                }
                output = Convert.ToInt32(Encryption.Decrypt(bytes, Encoding.ASCII.GetBytes(AESKey), Encoding.ASCII.GetBytes(AESIV)));
            }
            catch (Exception ex)
            { }
            return output;
        }

        public static string FormatNumberDecimal(long num)
        {
            if (num >= 100000000)
            {
                return (num / 1000000D).ToString("0.#M");
            }
            if (num >= 1000000)
            {
                return (num / 1000000D).ToString("0.##M");
            }
            if (num >= 100000)
            {
                return (num / 1000D).ToString("0.#k");
            }
            if (num >= 10000)
            {
                return (num / 1000D).ToString("0.##k");
            }
            return num.ToString("0.00");
        }
        public static string FormatNumberInteger(long num)
        {
            if (num >= 100000000)
            {
                return (num / 1000000D).ToString("0.#M");
            }
            if (num >= 1000000)
            {
                return (num / 1000000D).ToString("0.##M");
            }
            if (num >= 100000)
            {
                return (num / 1000D).ToString("0.#k");
            }
            if (num >= 10000)
            {
                return (num / 1000D).ToString("0.##k");
            }
            return num.ToString("0");
        }

        public static string GetShortcode(string name)
        {
            string output = "";
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    name = name.Replace("  ", " ").Trim();
                    var splitArray = name.Split(' ');
                    var splitArrayDot = name.Split('.');

                    if (splitArray.Length > 1)
                    {
                        output = splitArray[0].Substring(0, 1) + splitArray[1].Substring(0, 1);
                    }
                    else if (splitArrayDot.Length > 1)
                    {
                        if (!string.IsNullOrEmpty(splitArrayDot[1]))
                        {
                            output = splitArrayDot[0].Substring(0, 1) + splitArrayDot[1].Substring(0, 1);
                        }
                        else
                        {
                            output = splitArrayDot[0].Length > 1
                                ? splitArrayDot[0].Substring(0, 2)
                                : splitArrayDot[0].Substring(0, 1);
                        }
                    }
                    else
                    {
                        output = name.Length > 1 ? name.Substring(0, 2) : name.Substring(0, 1);
                    }
                }
            }
            catch (Exception e)
            { }
            return output.ToUpper();
        }
        public static string[] SplitString(string data, string[] delimiter)
        {
            if (data == null) return new string[] { };

            var str = ToString(data);
            if (string.IsNullOrWhiteSpace(str)) return new string[] { };

            //var delimiters = new[] { ',', '\n', '\r' };

            return str
                .Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
        }

        public static T ConvertJson<T>(string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        public static string BuildContactLine(string email, string phone)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(email))
                parts.Add(email);
            if (!string.IsNullOrWhiteSpace(phone))
                parts.Add("Ph: " + phone);
            return string.Join(" | ", parts);
        }

        public static string BuildMultilineText(IEnumerable<string> lines)
        {
            if (lines == null)
                return string.Empty;

            return string.Join(
                Environment.NewLine,
                lines.Where(l => !string.IsNullOrWhiteSpace(l))
            );
        }
        //public static IconDTO GetIcon(int iconid)
        //{
        //    List<IconDTO> iconlist = SessionManager.Icons;
        //    if (iconlist == null)
        //    {
        //        ICommonRepository repo = new CommonRepository();
        //        iconlist = repo.GetIcons();
        //        SessionManager.Icons = iconlist;
        //    }
        //    return iconlist.Where(x => x.IconId == iconid).FirstOrDefault() ?? new IconDTO();
        //}
        //public static string GetIconPath(string iconfile)
        //{
        //    return IconFolder + iconfile;
        //}
        //public static string GetIconPath(int iconid, bool filenameonly = false)
        //{
        //    var icon = GetIcon(iconid);
        //    string iconfile = icon.icon;
        //    return icon.iscss ? iconfile : (filenameonly ? (IconFolder + iconfile) : "<img class='dynamicicon' src='" + (IconFolder + iconfile) + "'/>");
        //}
        //public static string GetSelectedIconPath(int iconid, bool filenameonly = false)
        //{
        //    var icon = GetIcon(iconid);
        //    string iconfile = icon.iconselected;
        //    return icon.iscss ? iconfile : (filenameonly ? (IconFolder + iconfile) : "<img class='dynamicicon' src='" + (IconFolder + iconfile) + "'/>");
        //}

    }
    public static class Constants
    {
        public static string DateFormat = "dd'/'MM'/'yyyy";
        public static string DateTimeFormat = "dd'/'MM'/'yyyy hh:mm:ss tt";
        public static string DateFormatInvoice = "dd'-'MMM'-'yyyy";
    }

    public class EmptyTable
    {
        public string Heading { get; set; }
        public string SubHead { get; set; }
        public string Icon { get; set; }
        public string Button { get; set; }
        public bool NoleftRadius { get; set; } = false;

    }

}
