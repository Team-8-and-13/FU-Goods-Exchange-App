namespace FUExchange.Core.Utils
{
    public static class TimeHelper
    {
        public static DateTime ConvertToUtcPlus7(DateTime dateTime)
        {
            // UTC+7 is 7 hours ahead of UTC
            return dateTime.AddHours(7);
        }

        public static DateTime ConvertToUtcPlus7NotChanges(DateTime dateTime)
        {
            // UTC+7 is 7 hours ahead of UTC
            return dateTime;
        }
        //public static DateTimeOffset ConvertToUtcPlus7(DateTimeOffset dateTimeOffset)
        //{
        //    // UTC+7 is 7 hours ahead of UTC
        //    TimeSpan utcPlus7Offset = new(7, 0, 0);
        //    return dateTimeOffset.ToOffset(utcPlus7Offset);
        //}

        //public static DateTimeOffset ConvertToUtcPlus7NotChanges(DateTimeOffset dateTimeOffset)
        //{
        //    // UTC+7 is 7 hours ahead of UTC
        //    TimeSpan utcPlus7Offset = new(7, 0, 0);
        //    return dateTimeOffset.ToOffset(utcPlus7Offset).AddHours(-7);
        //}
    }
}
