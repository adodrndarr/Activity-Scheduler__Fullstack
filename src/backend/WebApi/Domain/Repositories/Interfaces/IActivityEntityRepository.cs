using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using System;
using System.Collections.Generic;


namespace Domain.Repositories.Interfaces
{
    public interface IActivityEntityRepository : IRepositoryBase<ActivityEntity>
    {
        IEnumerable<ActivityEntity> GetAllActivityEntities();
        PagedList<ActivityEntity> GetAllActivityEntities(PaginationDTO pagination);

        ActivityEntity GetActivityEntityById(Guid activityEntityId);
        ActivityEntity GetActivityEntityByName(string name);
        IEnumerable<ActivityEntity> GetActivityEntitiesByIds(IEnumerable<Guid> activityEntityIds);
    }
}
