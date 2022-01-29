using ActivityScheduler.Domain.DataAccess;
using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Domain.Repositories.Implementations
{
    public class ActivityEntityRepository : RepositoryBase<ActivityEntity>, IActivityEntityRepository
    {
        public ActivityEntityRepository(ActivitySchedulerDbContext dbContext) 
            : base(dbContext)
        {
        }


        public IEnumerable<ActivityEntity> GetAllActivityEntities()
        {
            var activityEntities = GetAll().OrderBy(a => a.Name);

            return activityEntities;
        }

        public PagedList<ActivityEntity> GetAllActivityEntities(PaginationDTO pagination)
        {
            var allActivities = GetAll().OrderBy(a => a.Name)
                                        .AsQueryable();

            if (pagination.IncludeName != null)
            {
                allActivities = allActivities.Where(a => a.Name.ToLower()
                                                         .Contains(pagination.IncludeName.ToLower()));
            }

            var activities = PagedList<ActivityEntity>.ToPagedList(
                                allActivities,
                                pagination.PageNumber,
                                pagination.PageSize);

            return activities;
        }

        public ActivityEntity GetActivityEntityById(Guid activityEntityId)
        {
            var activityEntity = GetByCondition(a => a.Id == activityEntityId).SingleOrDefault();

            return activityEntity;
        }
        
        public ActivityEntity GetActivityEntityByName(string name)
        {
            ActivityEntity activityEntity = null;

            if (name != null)
            {
                activityEntity = GetByCondition(a => a.Name.ToLower() == name.ToLower()).FirstOrDefault();
            }

            return activityEntity;
        }

        public IEnumerable<ActivityEntity> GetActivityEntitiesByIds(IEnumerable<Guid> activityEntityIds)
        {
            var activityEntities = GetByCondition(a => activityEntityIds.Contains(a.Id)).AsEnumerable();

            return activityEntities;
        }
    }
}
