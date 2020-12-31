using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using System.Linq;
using WebAPI.ActivityScheduler.DataAccess;
using WebAPI.ActivityScheduler.Entities;
using WebAPI.ActivityScheduler.EntitiesDTO;
using AutoMapper;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyActivitiesController : ControllerBase
    {
        public MyActivitiesController(ActivitySchedulerDbContext dbContext, IMapper mapper)
        {
            this._db = dbContext;
            this._mapper = mapper;
        }


        private readonly IMapper _mapper;
        private ActivitySchedulerDbContext _db;

        [Authorize(Roles = UserRoles.Admin)]
        // GET: myActivities
        [HttpGet("all")]
        public ActionResult<IEnumerable<ActivityDTO>> GetAllActivities()
        {
            var users = _db.Users
                .AsQueryable()
                .Include(u => u.Activities)
                .ThenInclude(a => a.ActivityEntity)
                .ToList();

            if (users.Count > 0)
            {
                var activities = users.SelectMany(u => u.Activities)
                                    .AsQueryable()
                                    .Include(a => a.User)
                                    .ToList();

                var activitiesDTOs = _mapper.Map<List<ActivityDTO>>(activities);

                if (activitiesDTOs.Count > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, activitiesDTOs);
                }
            }

            return StatusCode(
                StatusCodes.Status200OK,
                "Currently there are no activities available."
            );
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // GET: myActivities, Params: userId
        [HttpGet]
        public ActionResult<IEnumerable<ActivityDTO>> GetActivities(Guid userId)
        {
            var userFound = _db.Users
                .AsQueryable()
                .Include(u => u.Activities)
                .ThenInclude(a => a.ActivityEntity)
                .FirstOrDefault(u => u.Id == userId.ToString());

            if (userFound != null)
            {
                var activities = userFound.Activities
                                    .AsQueryable()                                    
                                    .Include(a => a.User)
                                    .ToList();

                var activitiesDTOs = _mapper.Map<List<ActivityDTO>>(activities);

                if (activitiesDTOs.Count > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, activitiesDTOs);
                }
            }

            return StatusCode(
                StatusCodes.Status200OK, 
                "Currently there are no activities available."
            );
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // PUT myActivities, Params: activityId
        [HttpPut]
        public ActionResult UpdateActivity(Guid activityId, [FromBody] ActivityDTO newActivity)
        {
            var activityToBeUpdated = _db.Activities
                .AsQueryable()
                .Include(a => a.ActivityEntity)
                .FirstOrDefault(a => a.Id == activityId);

            if (newActivity != null && activityToBeUpdated != null)
            {
                activityToBeUpdated.ActivityEntity.Name = newActivity.ActivityEntity?.Name;
                activityToBeUpdated.BookedForDate = newActivity.BookedForDate;
                activityToBeUpdated.ActivityEntity.Description = newActivity.ActivityEntity?.Description;
                activityToBeUpdated.StartTime = newActivity.StartTime;
                activityToBeUpdated.EndTime = newActivity.EndTime;
                activityToBeUpdated.ActivityEntity.ImageUrl = newActivity.ActivityEntity?.ImageUrl;
                activityToBeUpdated.ActivityEntity.Location = newActivity.ActivityEntity?.Location;
                activityToBeUpdated.ActivityEntity.MinUserCount = newActivity.ActivityEntity.MinUserCount;
                activityToBeUpdated.ActivityEntity.MaxUserCount = newActivity.ActivityEntity.MaxUserCount;
                activityToBeUpdated.OrganizerName = newActivity.OrganizerName;

                _db.SaveChanges();
                return StatusCode(
                    StatusCodes.Status201Created, 
                    $"Update for {newActivity.ActivityEntity.Name} was successful."
                );
            }

            return StatusCode(
                StatusCodes.Status400BadRequest, 
                $"Could not update the activity, please make sure you provided valid Id's and that the activity already exists."
            );
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // DELETE: myActivities, Params: activityId
        [HttpDelete]
        public ActionResult DeleteActivity(Guid activityId)
        {
            var activities = GetDbActivities()
                    .AsQueryable()
                    .Include(a => a.ActivityEntity)
                    .ToList();

            var activityToRemove = activities.FirstOrDefault(a => a.Id == activityId);

            if (activityToRemove != null)
            {
                _db.Activities.Remove(activityToRemove);
                _db.ActivityEntities.Remove(activityToRemove.ActivityEntity);
                _db.SaveChanges();

                return StatusCode(
                    StatusCodes.Status200OK, 
                    $"The activity: {activityToRemove.ActivityEntity?.Name}, has been deleted."
                );
            }

            return StatusCode(
                StatusCodes.Status400BadRequest, 
                $"The activity with id: {activityId}, could not be found."
            );
        }

        private IEnumerable<Activity> GetDbActivities()
        {
            return _db.Activities;
        }
    }
}
