﻿namespace WORLDGAMEDEVELOPMENT
{
    public static class Helper
    {
        public static DateTime ChangeDate(this DateTime dateTime, TimeSpan timeSpan)
        {
            return new DateTime(
               dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds);
        }
    }
}
