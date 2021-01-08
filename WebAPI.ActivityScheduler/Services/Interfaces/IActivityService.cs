using System;


namespace WebAPI.ActivityScheduler.Services
{
    public interface IActivityService
    {
        string CalculateDuration(DateTime startTime, DateTime endTime);
    }
}
