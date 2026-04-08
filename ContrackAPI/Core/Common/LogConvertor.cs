using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ContrackAPI
{
    public static class LogConvertor
    {
        public static string GetTimeAgo(DateTime dateTime, bool showtime = false)
        {
            if (dateTime == null)
                return "N/A";

            TimeSpan timeDifference = Common.ToClientDateTime(DateTime.UtcNow) - dateTime;

            if (timeDifference.TotalSeconds < 60)
                return "Just now";
            if (timeDifference.TotalMinutes < 60)
                return $"{(int)timeDifference.TotalMinutes} minutes ago" + (showtime ? (", " + dateTime.ToString("dd MMM yyyy hh:mm tt")) : "");
            if (timeDifference.TotalHours < 24)
                return $"{(int)timeDifference.TotalHours} hours ago" + (showtime ? (", " + dateTime.ToString("dd MMM yyyy hh:mm tt")) : "");
            if (timeDifference.TotalDays < 7)
                return $"{(int)timeDifference.TotalDays} days ago" + (showtime ? (", " + dateTime.ToString("dd MMM yyyy hh:mm tt")) : "");
            if (timeDifference.TotalDays < 30)
                return $"{(int)(timeDifference.TotalDays / 7)} weeks ago" + (showtime ? (", " + dateTime.ToString("dd MMM yyyy hh:mm tt")) : "");
            if (timeDifference.TotalDays < 365)
                return $"{(int)(timeDifference.TotalDays / 30)} months ago" + (showtime ? (", " + dateTime.ToString("dd MMM yyyy hh:mm tt")) : "");

            return $"{(int)(timeDifference.TotalDays / 365)} years ago" + (showtime ? (", " + dateTime.ToString("dd MMM yyyy hh:mm tt")) : "");
        }

        public static List<LogResult> GetLogDifference(object objcurrent, object objold, string ParametersToCheck = "")
        {
            List<LogResult> output = new List<LogResult>();
            try
            {
                string[] comparefields = ParametersToCheck.Split(',');
                Type typeold = objold.GetType();
                Type typecurrent = objcurrent.GetType();
                PropertyInfo[] properties = typeold.GetProperties();
                PropertyInfo[] properties1 = typecurrent.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    string propname = property.Name;
                    var tempname = comparefields.Where(x => x == propname || x.Contains(propname + "#")).FirstOrDefault();
                    if (tempname != null)
                    {
                        var splitarray = tempname.Split('#');
                        if (splitarray.Length > 1)
                        {
                            propname = splitarray[1];
                        }
                        object vobjoldval = property.GetValue(objold, null);
                        object vobjcurrentval = property.GetValue(objcurrent, null);
                        string oldvalue = Convert.ToString(vobjoldval);
                        string currentvalue = Convert.ToString(vobjcurrentval);
                        if (oldvalue.ToUpper() != currentvalue.ToUpper())
                        {
                            if (Convert.ToString(vobjoldval) == "")
                            {
                                output.Add(new LogResult
                                {
                                    FieldName = propname,
                                    Type = 1,
                                    OldValue = oldvalue,
                                    NewValue = currentvalue
                                });
                            }
                            //if (Convert.ToString(vobjcurrentval) == "")
                            //{
                            //    output.Add(new LogResult
                            //    {
                            //        FieldName = propname,
                            //        Type = 3,
                            //        OldValue = oldvalue,
                            //        NewValue = currentvalue
                            //    });
                            //}
                            else
                            {
                                output.Add(new LogResult
                                {
                                    FieldName = propname,
                                    Type = 2,
                                    NewValue = currentvalue,
                                    OldValue = oldvalue
                                });
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return output;
        }
        public static List<LogResult> GetLogDifferenceForCustomAttribute(List<KeyValuePair> objcurrent, List<KeyValuePair> objold)
        {
            List<LogResult> output = new List<LogResult>();
            try
            {
                if (objcurrent == null)
                    objcurrent = new List<KeyValuePair>();
                if (objold == null)
                    objold = new List<KeyValuePair>();
                foreach (var oldval in objold)
                {
                    var newmatched = objcurrent.Where(x => x.KeyName == oldval.KeyName).FirstOrDefault();
                    if (newmatched != null)
                    {
                        if (newmatched.KeyValue.ToUpper() != oldval.KeyValue.ToUpper())
                        {
                            if (Convert.ToString(oldval.KeyValue) == "")
                            {
                                output.Add(new LogResult
                                {
                                    FieldName = oldval.KeyName,
                                    Type = 1,
                                    OldValue = oldval.KeyValue,
                                    NewValue = newmatched.KeyValue
                                });
                            }
                            else
                            {
                                output.Add(new LogResult
                                {
                                    FieldName = oldval.KeyName,
                                    Type = 2,
                                    OldValue = oldval.KeyValue,
                                    NewValue = newmatched.KeyValue
                                });
                            }
                        }
                    }
                    else
                    {
                        // Removed
                        output.Add(new LogResult
                        {
                            FieldName = oldval.KeyName,
                            Type = 2,
                            OldValue = oldval.KeyValue,
                            NewValue = ""
                        });
                    }
                }
                objcurrent = objcurrent.Where(x => objold.Where(y => y.KeyName == x.KeyName).Count() == 0).ToList();
                foreach (var currentval in objcurrent)
                {
                    output.Add(new LogResult
                    {
                        FieldName = currentval.KeyName,
                        Type = 1,
                        OldValue = "",
                        NewValue = currentval.KeyValue
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return output;
        }
        //public static List<LogResult> GetLogDifferenceForBankAccount(List<CustomBankInfo> objcurrent, List<CustomBankInfo> objold)
        //{
        //    List<LogResult> output = new List<LogResult>();
        //    try
        //    {
        //        if (objcurrent == null)
        //            objcurrent = new List<CustomBankInfo>();
        //        if (objold == null)
        //            objold = new List<CustomBankInfo>();


        //        foreach (var oldval in objold)
        //        {
        //            var newmatched = objcurrent.Where(x => x.BankAccountUUID == oldval.BankAccountUUID).FirstOrDefault();
        //            if (newmatched != null)
        //            {
        //                oldval.BankAttributes.ForEach(x => x.UUID = "");
        //                newmatched.BankAttributes.ForEach(x => x.UUID = "");
        //                oldval.BankAttributes.RemoveAll(x => string.IsNullOrEmpty(x.KeyName));
        //                newmatched.BankAttributes.RemoveAll(x => string.IsNullOrEmpty(x.KeyName));
        //                var oldjson = JsonConvert.SerializeObject(oldval);
        //                var newjson = JsonConvert.SerializeObject(newmatched);
        //                if (newjson != oldjson)
        //                //if (newmatched.KeyValue.ToUpper() != oldval.KeyValue.ToUpper())
        //                {
        //                    if (Convert.ToString(oldjson) == "")
        //                    {
        //                        output.Add(new LogResult
        //                        {
        //                            //FieldName = "Bank Account - " + oldval.AliasName,
        //                            FieldName = "Bank Account",
        //                            Type = 1,
        //                            OldValue = "",
        //                            NewValue = GetBakDetails(newmatched)
        //                        });
        //                    }
        //                    else
        //                    {
        //                        output.Add(new LogResult
        //                        {
        //                            //FieldName = "Bank Account - " + oldval.AliasName,
        //                            FieldName = "Bank Account",
        //                            Type = 2,
        //                            OldValue = GetBakDetails(oldval),
        //                            NewValue = GetBakDetails(newmatched)
        //                        });
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                // Removed
        //                output.Add(new LogResult
        //                {
        //                    //FieldName = "Bank Account - " + oldval.AliasName,
        //                    FieldName = "Bank Account",
        //                    Type = 2,
        //                    OldValue = GetBakDetails(oldval),
        //                    NewValue = ""
        //                });
        //            }
        //        }
        //        objcurrent = objcurrent.Where(x => objold.Where(y => y.BankAccountUUID == x.BankAccountUUID).Count() == 0).ToList();
        //        foreach (var currentval in objcurrent)
        //        {
        //            output.Add(new LogResult
        //            {
        //                //FieldName = "Bank Account - " + currentval.AliasName,
        //                FieldName = "Bank Account",
        //                Type = 1,
        //                OldValue = "",
        //                NewValue = GetBakDetails(currentval)
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return output;
        //}
        //private static string GetBakDetails(CustomBankInfo bank)
        //{
        //    string output = "";
        //    try
        //    {
        //        if (output == "")
        //        {
        //            output = "Alias: " + bank.AliasName;
        //        }
        //        else
        //        {
        //            output = output + "<br/>Alias: " + bank.AliasName;
        //        }
        //        foreach (var s in bank.BankAttributes.Where(x => !string.IsNullOrEmpty(x.KeyName)))
        //        {
        //            if (output == "")
        //            {
        //                output = s.KeyName + ": " + s.KeyValue;
        //            }
        //            else
        //            {
        //                output = output + "<br/>" + s.KeyName + ": " + s.KeyValue;
        //            }
        //        }
        //        if (output == "")
        //        {
        //            output = "Currency: " + bank.Currency;
        //        }
        //        else
        //        {
        //            output = output + "<br/>Currency: " + bank.Currency;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return output;
        //}
    }
}