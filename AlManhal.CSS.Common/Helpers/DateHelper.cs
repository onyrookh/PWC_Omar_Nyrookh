using System.Collections.Generic;

namespace System
{
    public class DateHelper
    {
        #region Constants

        private const string TIME_STAMP_FORMAT = "{0:D2}h:{1:D2}m:{2:D2}s";
        private const string TIME_STAMP_MINUTES_FORMAT = "{0:D2}h:{1:D2}m";
        #endregion
        public static string ConvertToMinuts(double seconde)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconde);

            string time = string.Format(TIME_STAMP_FORMAT,
                            timeSpan.Hours,
                            timeSpan.Minutes,
                            timeSpan.Seconds,
                            timeSpan.Milliseconds);
            return time;
        }
        public static string ConvertToHours(double Minutes)
        {
            TimeSpan timeSpan = TimeSpan.FromMinutes(Minutes);

            string time = string.Format(TIME_STAMP_MINUTES_FORMAT,
                            timeSpan.Hours,
                            timeSpan.Minutes);
            return time;
        }

        public static List<DateTime> GetMonthsBetween(DateTime from, DateTime to)
        {
            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                monthDiff -= 1;
            }

            List<DateTime> results = new List<DateTime>();
            for (int i = monthDiff; i >= 0; i--)
            {
                results.Add(to.AddMonths(-i));
            }

            return results;
        }
    }
}
