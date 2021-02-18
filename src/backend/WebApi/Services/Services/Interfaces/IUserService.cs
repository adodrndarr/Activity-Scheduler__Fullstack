using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ActivityScheduler.Services.Interfaces
{
    public interface IUserService
    {
        ResultDetails GetAllDTOsWithDetails(PaginationDTO pagination, HttpResponse response);
        IEnumerable<User> GetAllWithDetails();

        User GetById(Guid userId);
        ResultDetails GetUserDTOById(Guid id);

        User GetByIdWithDetails(Guid userId);
        Task<ResultDetails> Update(User userToUpdate, UserUpdateDTO newUser, HttpContext context);
        ResultDetails Delete(User user);
        void SaveChanges();

        IEnumerable<ActivityResponseDTO> GetActivitiesFor(List<User> users);
        PagedList<ActivityResponseDTO> GetActivitiesFor(User user, PaginationDTO pagination, HttpResponse response);
        ResultDetails GetAllActivities();
        ResultDetails GetActivitiesByUserId(Guid userId, PaginationDTO pagination, HttpResponse response);
    }
}
