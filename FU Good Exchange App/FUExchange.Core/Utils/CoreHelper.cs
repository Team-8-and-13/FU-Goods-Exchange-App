namespace FUExchange.Core.Utils
{
    public class CoreHelper
    {
        //public static DateTimeOffset SystemTimeNow => TimeHelper.ConvertToUtcPlus7(DateTimeOffset.Now);
        public static DateTime SystemTimeNow => TimeHelper.ConvertToUtcPlus7(DateTime.UtcNow);

    }
}
