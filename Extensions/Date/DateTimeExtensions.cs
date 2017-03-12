using System;

namespace Extensions.Date
{
    public static class DateTimeExtensions
    {
        public static DateTime UNIXTimeToDateTime(this int unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime).ToLocalTime();
        }
    }
}
