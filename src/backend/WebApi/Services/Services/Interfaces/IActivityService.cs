using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using System;
using System.Collections.Generic;


namespace ActivityScheduler.Services.Interfaces
{
    public interface IActivityService
    {
        Activity GetActivityById(Guid activityId);
        void Add(ActivityRequestDTO activityDTO, User user);
        void AddMany(List<ActivityRequestDTO> activityRequestDTOs, User user);
        void Update(Activity activityToUpdate, ActivityRequestDTO newActivity);
        ResultDetails Delete(Guid activityId);
        void SaveChanges();

        ResultDetails ScheduleActivity(User user, ActivityRequestDTO newActivityDTO);
        ScheduleResponseDTO ScheduleActivities(User user, List<ActivityRequestDTO> newActivityDTOs);
        ResultDetails CheckAvailability(ActivityEntity activityEntity, DateTime forDate);
        List<Activity> GetBookedActivities(ActivityEntity activityEntity, DateTime forDate);

        string CalculateDuration(DateTime startTime, DateTime endTime);
        bool ActivityEntityExists(string name);
        bool IsScheduleTimeAvailable(ActivityRequestDTO newActivityDTO, List<Activity> bookedActivities);
    }
}
