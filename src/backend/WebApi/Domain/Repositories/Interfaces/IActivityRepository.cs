using ActivityScheduler.Domain.Entities;
using System;


namespace Domain.Repositories.Interfaces
{
    public interface IActivityRepository : IRepositoryBase<Activity>
    {
        Activity GetActivityById(Guid activityId);
        bool HasActivityEntityWithId(Guid activityId);
    }
}
