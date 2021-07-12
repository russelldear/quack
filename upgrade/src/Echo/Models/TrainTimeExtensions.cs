using System;
namespace Echo.Models
{
    public static class TrainTimeExtensions
    {
        public static int GetSecondsFromNow(this TrainTime trainTime)
        {
            TimeZoneInfo nzTimezone = null;
            try
            {
                nzTimezone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            }
            catch
            {
                nzTimezone = TimeZoneInfo.FindSystemTimeZoneById("Pacific/Auckland");
            }

            var coalescedTime = trainTime.Expected ?? trainTime.Aimed;

            var nzTrainTime = TimeZoneInfo.ConvertTimeFromUtc(coalescedTime.ToUniversalTime(), nzTimezone);

            var nzNowTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, nzTimezone);

            return Convert.ToInt32((nzTrainTime - nzNowTime).TotalSeconds);
        }
    }
}
