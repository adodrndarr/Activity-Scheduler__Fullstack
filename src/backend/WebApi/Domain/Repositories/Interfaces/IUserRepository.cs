using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using System;
using System.Collections.Generic;


namespace Domain.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        PagedList<User> GetAllUsersWithDetails(PaginationDTO pagination);
        IEnumerable<User> GetAllUsersWithDetails();
        User GetUserWithDetailsById(Guid userId);
    }
}
