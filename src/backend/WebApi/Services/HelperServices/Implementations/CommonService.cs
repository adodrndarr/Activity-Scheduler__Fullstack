using ActivityScheduler.Services.HelperServices.Interfaces;
using System;


namespace ActivityScheduler.Services.HelperServices
{
    public class CommonService : ICommonService
    {
        public string CalculateDuration(DateTime startTime, DateTime endTime)
        {
            TimeSpan duration = new TimeSpan(0, 0, 0);
            DateTime now = DateTime.Now;

            if ((startTime > now && endTime > now) && (endTime > startTime))
            {
                duration = endTime - startTime;
            }

            var durationDay = duration.Days;
            int durationHour = duration.Hours;
            int durationMinute = duration.Minutes;

            string day = durationDay >= 1 ? $"{durationDay} day" : "";
            string hour = durationHour >= 1 ? $"{durationHour} hour" : "";
            string minute = durationMinute >= 1 ? $"{durationMinute} minute" : "";

            string finalDuration = (durationDay == 0 && durationHour == 0 && durationMinute == 0) ?
                    "Can't calculate duration, invalid start time or end time." :
                    $"{day} {hour} {minute}";

            return finalDuration;
        }
    }
}
