namespace ContrackAPI
{
    public static class Display
    {
        public static string DisplayString(string input)
        {
            return string.IsNullOrEmpty(input) ? "-" : input;
        }
        public static string DisplayStringNA(string input)
        {
            return string.IsNullOrEmpty(input) ? "N/A" : input;
        }

        public static string DisplayCurrency(decimal input)
        {
            return input.ToString("N2");
        }
        public static string DisplayCurrencyWithAutoCorrect(decimal input)
        {
            if (input < 0 && input > Common.CurrencyAdjustThreshold)
                input = 0;
            return input.ToString("N2");
        }
        public static string DisplayDateTimeHumanFriendly(DateTime date)
        {
            if (date == DateTime.MinValue)
                return "-";
            else
                return date.ToString(Common.HumanDateTimeformat);
        }

        public static string DisplayDateHumanFriendly(DateTime date)
        {
            if (date == DateTime.MinValue)
                return "-";
            else
                return date.ToString(Common.HumanDateformat);
        }

        public static string DisplayClientDateTimeHumanFriendly(DateTime date)
        {
            if (date == DateTime.MinValue)
                return "-";
            else
            {
                try
                {
                    //return date.AddMinutes(Convert.ToDouble(SessionManager.TimeOffSet)).ToString(Common.HumanDateTimeformat);
                    return date.AddMinutes(Convert.ToDouble(Common.GetClientTimeOffset())).ToString(Common.HumanDateTimeformat);
                }
                catch (Exception ex)
                {
                    return date.ToString(Common.HumanDateTimeformat);
                }
            }

        }

        public static DateTime GetClientDateTime(DateTime date)
        {
            if (date == DateTime.MinValue)
                return date;
            else
            {
                try
                {
                     return date.AddMinutes(Convert.ToDouble(Common.GetClientTimeOffset()));

                }
                catch (Exception ex)
                {
                    return date;
                }
            }

        }

        public static string GetFileSize(long byteCount)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = byteCount;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }


    }
}