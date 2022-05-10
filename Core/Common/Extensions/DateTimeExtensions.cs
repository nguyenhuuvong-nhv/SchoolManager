using System;

namespace Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FromUnixTimeStamp(this long unixTimeStamp)
        {
            var timeSpan = TimeSpan.FromSeconds(unixTimeStamp);
            return new DateTime(timeSpan.Ticks + new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).UtcToLocalTime().Ticks);
        }

        public static DateTime? FromUnixTimeStamp(this long? unixTimeStamp)
        {
            return unixTimeStamp >= -6847786800 ? FromUnixTimeStamp(unixTimeStamp.Value) : (DateTime?)null;
        }

        public static long? ToSecondsTimestamp(this DateTime? date)
        {
            if (date == null)
                return null;

            return ToSecondsTimestamp((DateTime)date);
        }

        public static long? ToSecondsTimestamp(this DateTime date)
        {
            if (date.Year == 9999)
            {
                return null;
            }
            var span = date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).UtcToLocalTime());
            return (long)span.TotalSeconds;
        }

        public static DateTime UtcToLocalTime(this DateTime date)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Utc.Id, TimeZoneInfo.Local.Id);
        }
    }
}
