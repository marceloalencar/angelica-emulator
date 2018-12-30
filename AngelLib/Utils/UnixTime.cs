using System;

namespace AngelLib.Utils
{
    public class UnixTime
    {
        public static uint ToTimestamp(DateTime dateTime)
        {
            DateTime dtBase = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan timeSpan = new TimeSpan(dateTime.Ticks - dtBase.Ticks);
            return Convert.ToUInt32(timeSpan.TotalSeconds);
        }

        public static DateTime ToDateTime(uint timestamp)
        {
            DateTime newDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            newDate = newDate.AddSeconds(timestamp);
            return newDate;
        }
    }
}
