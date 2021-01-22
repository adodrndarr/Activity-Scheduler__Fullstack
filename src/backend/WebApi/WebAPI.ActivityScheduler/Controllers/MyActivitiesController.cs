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
using WebAPI.ActivityScheduler.Services;
using AutoMapper;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyActivitiesController : ControllerBase
    {
        public MyActivitiesController(
            ActivitySchedulerDbContext dbContext, 
            IMapper mapper,
            ILoggerManager logger)
        {
            this._db = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }


        private readonly IMapper _mapper;
        private ActivitySchedulerDbContext _db;
        private ILoggerManager _logger;

        [Authorize(Roles = UserRoles.Admin)]
        // GET: myActivities
        [HttpGet("all")]
        public ActionResult<IEnumerable<ActivityDTO>> GetAllActivities()
        {
            this._logger.LogInfo("MyActivitiesController GetAllActivities - Getting all users...");
            var users = _db.Users
                .AsQueryable()
                .Include(u => u.Activities)
                .ThenInclude(a => a.ActivityEntity)
                .ToList();

            if (users.Count > 0)
            {
                this._logger.LogInfo("MyActivitiesController GetAllActivities - Getting activities from all users...");
                var activities = users.SelectMany(u => u.Activities)
                                    .AsQueryable()
                                    .Include(a => a.User)
                                    .ToList();

                var activitiesDTOs = _mapper.Map<List<ActivityDTO>>(activities);
                if (activitiesDTOs.Count > 0)
                {
                    this._logger.LogInfo($"MyActivitiesController GetAllActivities - Returning {activitiesDTOs.Count} activities.");
                    return StatusCode(StatusCodes.Status200OK, activitiesDTOs);
                }
            }

            this._logger.LogInfo("MyActivitiesController GetAllActivities - No activities available 0.");
            return StatusCode(
                StatusCodes.Status200OK,
                new InfoResponseDTO
                {
                    Info = "Currently there are no activities available."
                }
            );
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // GET: myActivities, Params: userId
        [HttpGet]
        public ActionResult<IEnumerable<ActivityDTO>> GetActivities(Guid userId)
        {
            this._logger.LogInfo("MyActivitiesController GetActivities - Getting specific user...");
            var userFound = _db.Users
                .AsQueryable()
                .Include(u => u.Activities)
                .ThenInclude(a => a.ActivityEntity)
                .FirstOrDefault(u => u.Id == userId.ToString());

            if (userFound != null)
            {
                this._logger.LogInfo("MyActivitiesController GetActivities - Getting activities from the specific user...");
                var activities = userFound.Activities
                                    .AsQueryable()                                    
                                    .Include(a => a.User)
                                    .ToList();

                var activitiesDTOs = _mapper.Map<List<ActivityDTO>>(activities);
                if (activitiesDTOs.Count > 0)
                {
                    this._logger.LogInfo($"MyActivitiesController GetAActivities - Returning {activitiesDTOs.Count} activities.");
                    return StatusCode(StatusCodes.Status200OK, activitiesDTOs);
                }
            }

            this._logger.LogInfo("MyActivitiesController GetAActivities - No activities available 0.");
            return StatusCode(
                StatusCodes.Status200OK,
                new InfoResponseDTO
                {
                    Info = "Currently there are no activities available."
                }
            );
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // PUT myActivities, Params: activityId
        [HttpPut]
        public ActionResult UpdateActivity(Guid activityId, ActivityDTO newActivity)
        {
            this._logger.LogInfo("MyActivitiesController UpdateActivity - Getting specific activity to update...");
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

                this._logger.LogInfo("MyActivitiesController UpdateActivity - Update was successful.");
                return StatusCode(
                    StatusCodes.Status201Created, 
                    new InfoResponseDTO
                    {
                        Info = $"Update for {newActivity.ActivityEntity.Name} was successful."
                    }
                );
            }

            this._logger.LogInfo("MyActivitiesController UpdateActivity - Update failed.");
            return StatusCode(
                StatusCodes.Status400BadRequest,
                new InfoResponseDTO
                {
                    Info = $"Could not update the activity, please make sure you provided valid Id and that the activity already exists."
                }
            );
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // DELETE: myActivities, Params: activityId
        [HttpDelete]
        public ActionResult DeleteActivity(Guid activityId)
        {
            this._logger.LogInfo("MyActivitiesController DeleteActivity - Getting specific activity to delete...");
            var activityToRemove = GetDbActivities()
                    .AsQueryable()
                    .Include(a => a.ActivityEntity)
                    .FirstOrDefault(a => a.Id == activityId);

            if (activityToRemove != null)
            {
                _db.Activities.Remove(activityToRemove);
                _db.ActivityEntities.Remove(activityToRemove.ActivityEntity);
                _db.SaveChanges();

                this._logger.LogInfo("MyActivitiesController DeleteActivity - Deletion was successful.");
                return StatusCode(
                    StatusCodes.Status200OK, 
                    new InfoResponseDTO
                    {
                        Info = $"The activity: {activityToRemove.ActivityEntity?.Name}, has been deleted."
                    }
                );
            }

            this._logger.LogInfo("MyActivitiesController DeleteActivity - Deletion failed.");
            return StatusCode(
                StatusCodes.Status400BadRequest,
                new InfoResponseDTO
                {
                    Info = $"The activity with id: {activityId}, could not be found."
                }
            );
        }

        private IEnumerable<Activity> GetDbActivities()
        {
            return _db.Activities;
        }
    }
}
