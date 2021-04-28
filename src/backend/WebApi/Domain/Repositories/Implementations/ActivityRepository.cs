using ActivityScheduler.Domain.DataAccess;
using ActivityScheduler.Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace Domain.Repositories.Implementations
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(ActivitySchedulerDbContext dbContext) 
            : base(dbContext)
        {
        }


        public Activity GetActivityById(Guid activityId)
        {
            var activity = this.GetByCondition(a => a.Id == activityId)
                               .SingleOrDefault();

            return activity;
        }

        public bool HasActivityEntityWithId(Guid activityId)
        {
            var hasActivityEntity = this.GetAll().Any(a => a.ActivityEntityId == activityId.ToString());

            return hasActivityEntity;
        }
    }
}
