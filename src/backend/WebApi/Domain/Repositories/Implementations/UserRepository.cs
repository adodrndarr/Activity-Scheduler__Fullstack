using ActivityScheduler.Domain.DataAccess;
using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Domain.Repositories.Implementations
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ActivitySchedulerDbContext dbContext) 
            : base(dbContext)
        {
        }


        public PagedList<User> GetAllUsersWithDetails(PaginationDTO pagination)
        {
            var allUsers = GetAll().OrderBy(u => u.UserName).AsQueryable();

            if (pagination.IncludeAdmin)
            {
                allUsers = allUsers.Where(u => u.IsAdmin == pagination.IncludeAdmin);
            }
            
            if (pagination.IncludeName != null)
            {
                allUsers = allUsers.Where(u => u.UserName.ToLower()
                                                         .Contains(pagination.IncludeName.ToLower())
                                   );
            }

            var users = PagedList<User>.ToPagedList(
                                allUsers,
                                pagination.PageNumber,
                                pagination.PageSize
                            );
            return users;
        }

        public IEnumerable<User> GetAllUsersWithDetails()
        {
            var users = GetAll()
                            .Include(u => u.Activities)
                            .OrderBy(u => u.UserName)
                            .AsEnumerable();

            return users;
        }

        public User GetUserWithDetailsById(Guid userId)
        {
            var user = GetByCondition(u => u.Id == userId.ToString())
                            .Include(u => u.Activities)
                            .SingleOrDefault();

            return user;
        }
    }
}
