using System;


namespace ActivityScheduler.Services.HelperServices.Interfaces
{
    public interface ICommonService
    {
        string CalculateDuration(DateTime startTime, DateTime endTime);
    }
}