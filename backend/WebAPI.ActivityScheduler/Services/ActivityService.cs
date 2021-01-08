using System;


namespace WebAPI.ActivityScheduler.Services
{
    public class ActivityService : IActivityService
    {
        public string CalculateDuration(DateTime startTime, DateTime endTime)
        {
            TimeSpan duration = new TimeSpan(0, 0, 0);
            DateTime defaultDate = new DateTime(); // 1/1/0001

            if (endTime > defaultDate && startTime > defaultDate)
            {
                duration = endTime - startTime;
            }

            var durationDay = duration.Days;
            int durationHour = duration.Hours;
            int durationMinute = duration.Minutes;

            string day = durationDay >= 1 ? $"{durationDay} day/s" : "";
            string hour = durationHour >= 1 ? $"{durationHour} hour/s" : "";
            string minute = durationMinute >= 1 ? $"{durationMinute} minute/s" : "";

            string finalDuration = (durationDay == 0 && durationHour == 0 && durationMinute == 0) ?
                    "Can't calculate duration, invalid start time or end time." :
                    $"{day} {hour} {minute}";

            return finalDuration;
        }
    }
}
