namespace System
{
    public static class TimeSpanExtensions
    {
        #region Methods

        public static int GetYears(this TimeSpan timespan)
        {
            return (int)((double)timespan.Days / 365.2425);
        }

        public static int GetMonths(this TimeSpan timespan)
        {
            return (int)((double)timespan.Days / 30.436875);
        }

        #endregion
    }
}
