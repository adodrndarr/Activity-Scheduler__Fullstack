using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;


namespace ActivityScheduler.Services.Interfaces
{
    public interface IActivityEntityService
    {
        ResultDetails GetAll(PaginationDTO pagination, HttpResponse response);
        IEnumerable<ActivityEntityDTO> GetAllAsDTOs();
        ResultDetails GetAllActivityEntities();

        ActivityEntity GetById(Guid activityEntityId);
        ResultDetails GetActivityEntityDTOById(Guid id);

        ResultDetails Add(ActivityEntityDTO newActivityEntity);
        void AddMany(List<ActivityEntityDTO> newActivityEntityDTOs);

        ResultDetails Update(ActivityEntity entityToUpdate, ActivityEntityDTO newActivityEntity);
        ResultDetails Delete(Guid activityEntityId);

        ResultDetails UploadFile(IFormFile file, string currentDirectory);
    }
}
