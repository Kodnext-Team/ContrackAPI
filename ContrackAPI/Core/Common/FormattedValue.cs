using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContrackAPI
{
    public class FormattedValue<T>
    {
        public T Value { get; set; }
        public string Text { get; set; } = "";
        public string DBFormattedText { get; set; } = "";  // Only for date
        public string SubText { get; set; } = "";
        public int NumericValue { get; set; } = 0;
    }

    public class FormatConvertor
    {
        public static FormattedValue<DateTime> ToDateFormat(DateTime value, string format = Common.HumanDateformat)
        {
            FormattedValue<DateTime> result = new FormattedValue<DateTime>();
            try
            {
                bool isETA = value > DateTime.Now;

                result.Value = value;
                result.Text = value.Date == DateTime.MinValue ? "" : value.ToString(format);
                result.DBFormattedText = value.Date == DateTime.MinValue ? "" : value.ToString("yyyy-MM-dd");
                result.SubText = value.Date == DateTime.MinValue ? "" : (isETA ? ("in " + Common.GetETADays(value) + " days") : LogConvertor.GetTimeAgo(value, false));
                result.NumericValue = Common.GetETADays(value);
            }
            catch (Exception ex)
            { }
            return result;
        }

        public static FormattedValue<DateTime> ToDateTimeFormat(DateTime value, string format = Common.HumanDateTimeformat)
        {
            FormattedValue<DateTime> result = new FormattedValue<DateTime>();
            try
            {
                bool isETA = value > DateTime.Now;

                result.Value = value;
                result.Text = value == DateTime.MinValue ? "" : value.ToString(format);
                result.DBFormattedText = value.Date == DateTime.MinValue ? "" : value.ToString("yyyy-MM-dd HH:mm");
                result.SubText = value == DateTime.MinValue ? "" : (isETA ? ("in " + Common.GetETADays(value) + " days") : LogConvertor.GetTimeAgo(value, false));
                result.NumericValue = Common.GetETADays(value);
            }
            catch (Exception ex)
            { }
            return result;
        }
        public static FormattedValue<DateTime> ToClientDateTimeFormat(DateTime value, string format = Common.HumanDateTimeformat)
        {
            FormattedValue<DateTime> result = new FormattedValue<DateTime>();
            try
            {
                value = Common.ToClientDateTime(value);
                bool isETA = value > DateTime.Now;

                result.Value = value;
                result.Text = value == DateTime.MinValue ? "" : value.ToString(format);
                result.SubText = value == DateTime.MinValue ? "" : (isETA ? ("in " + Common.GetETADays(value) + " days") : LogConvertor.GetTimeAgo(value, false));
                result.NumericValue = Common.GetETADays(value);
            }
            catch (Exception ex)
            { }
            return result;
        }
        public static FormattedValue<int> ToStatus(int statusId, int type, List<Status> statusList)
        {
            FormattedValue<int> result = new FormattedValue<int>();
            try
            {
                var selectedStatus = statusList.FirstOrDefault(x => x.type_id == type && x.statusid == statusId);
                result.Value = statusId;
                result.NumericValue = statusId;
                if (selectedStatus != null)
                {
                    result.Text = selectedStatus.statusname;
                    result.SubText = selectedStatus.styling.Replace("[StatusName]", selectedStatus.statusname);
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}