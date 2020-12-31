using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using System.Linq;
using WebAPI.ActivityScheduler.DataAccess;
using WebAPI.ActivityScheduler.Entities;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActivityEntitiesController : ControllerBase
    {
        private ActivitySchedulerDbContext _db;
        public ActivityEntitiesController(ActivitySchedulerDbContext dbContext)
        {
            this._db = dbContext;
        }


        // GET: activityEntities
        [HttpGet]
        public ActionResult<IEnumerable<ActivityEntity>> GetActivityEntities()
        {
            var activities = GetActivities().ToList();

            if (activities.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, activities);
            }

            return StatusCode(
                StatusCodes.Status200OK, 
                "Currently there are no activities available."
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

                return StatusCode(
                    StatusCodes.Status201Created, 
                    $"The activity entity: {newActivityEntity.Name}, has been added."
                );
            }

            return StatusCode(
                StatusCodes.Status400BadRequest, 
                $"Please provide a valid activity entity."
            );
        }

        [Authorize(Roles = UserRoles.Admin)]
        // DELETE: activityEntities, Params: activityEntityId
        [HttpDelete]
        public ActionResult DeleteActivityEntity(Guid activityEntityId)
        {
            var activityFound = GetActivities()
                .FirstOrDefault(activityEntity => activityEntity.Id == activityEntityId);

            if (activityFound != null)
            {
                _db.ActivityEntities.Remove(activityFound);
                _db.SaveChanges();

                return StatusCode(
                    StatusCodes.Status200OK, 
                    $"The activity entity: {activityFound.Name}, has been deleted."
                );
            }

            return StatusCode(
                StatusCodes.Status400BadRequest, 
                $"The activity entity with id: {activityEntityId}, could not be found."
            );
        }

        private IEnumerable<ActivityEntity> GetActivities()
        {
            return _db.ActivityEntities;
        }
    }
}
