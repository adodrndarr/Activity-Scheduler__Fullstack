using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using System.Linq;
using WebAPI.ActivityScheduler.DataAccess;
using WebAPI.ActivityScheduler.Entities;
using WebAPI.ActivityScheduler.EntitiesDTO;
using WebAPI.ActivityScheduler.Services;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActivityEntitiesController : ControllerBase
    {
        public ActivityEntitiesController(ActivitySchedulerDbContext dbContext, ILoggerManager logger)
        {
            this._db = dbContext;
            this._logger = logger;
        }


        private ActivitySchedulerDbContext _db;
        private ILoggerManager _logger;

        // GET: activityEntities
        [HttpGet]
        public ActionResult<IEnumerable<ActivityEntity>> GetActivityEntities()
        {
            this._logger.LogInfo("ActivityEntitiesController GetActivityEntities - Getting activity entities...");
            var activities = GetActivities().ToList();

            if (activities.Count > 0)
            {
                this._logger.LogInfo($"ActivityEntitiesController GetActivityEntities - Returning {activities.Count} activity entities.");
                return StatusCode(StatusCodes.Status200OK, activities);
            }

            this._logger.LogInfo("ActivityEntitiesController GetActivityEntities - No activity entities available 0.");
            return StatusCode(
                StatusCodes.Status200OK, 
                new InfoResponseDTO
                {
                    Info = "Currently there are no activities available."
                }
            );
        }

        [Authorize(Roles = UserRoles.Admin)]
        // POST: activityEntities
        [HttpPost]
        public ActionResult AddActivityEntity([FromBody] ActivityEntity newActivityEntity)
        {
            if(newActivityEntity != null)
            {
                _db.ActivityEntities.Add(newActivityEntity);
                _db.SaveChanges();

                this._logger.LogInfo("ActivityEntitiesController AddActivityEntity - Addition of a new activity entity was successful.");
                return StatusCode(
                    StatusCodes.Status201Created,
                    new InfoResponseDTO
                    {
                        Info = $"The activity entity: {newActivityEntity.Name}, has been added."
                    }
                );
            }

            this._logger.LogInfo("ActivityEntitiesController AddActivityEntity - Addition of a new activity entity failed.");
            return StatusCode(
                StatusCodes.Status400BadRequest,
                new InfoResponseDTO
                {
                    Info = $"Please provide a valid activity entity."
                }
            );
        }

        [Authorize(Roles = UserRoles.Admin)]
        // DELETE: activityEntities, Params: activityEntityId
        [HttpDelete]
        public ActionResult DeleteActivityEntity(Guid activityEntityId)
        {
            this._logger.LogInfo("ActivityEntitiesController DeleteActivityEntity - Getting activity entity for deletion.");
            var activityFound = GetActivities()
                .FirstOrDefault(activityEntity => activityEntity.Id == activityEntityId);

            if (activityFound != null)
            {
                _db.ActivityEntities.Remove(activityFound);
                _db.SaveChanges();

                this._logger.LogInfo("ActivityEntitiesController DeleteActivityEntity - Deletion of an activity entity was successful.");
                return StatusCode(
                    StatusCodes.Status200OK,
                    new InfoResponseDTO
                    {
                        Info = $"The activity entity: {activityFound.Name}, has been deleted."
                    }
                );
            }

            this._logger.LogInfo("ActivityEntitiesController DeleteActivityEntity - Deletion of an activity entity failed.");
            return StatusCode(
                StatusCodes.Status400BadRequest,
                new InfoResponseDTO
                {
                    Info = $"The activity entity with id: {activityEntityId}, could not be found."
                }
            );
        }

        private IEnumerable<ActivityEntity> GetActivities()
        {
            return _db.ActivityEntities;
        }
    }
}
