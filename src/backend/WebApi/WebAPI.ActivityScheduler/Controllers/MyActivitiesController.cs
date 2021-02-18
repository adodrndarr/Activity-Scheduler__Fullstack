using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using ActivityScheduler.Domain.Structs;
using ActivityScheduler.Services.Interfaces;
using ActivityScheduler.Presentation.EntitiesDTO;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyActivitiesController : ControllerBase
    {
        public MyActivitiesController(
            IUserService userService,
            IActivityService activityService,
            ILoggerManager logger)
        {
            _logger = logger;
            _activityService = activityService;
            _userService = userService;
        }

        private ILoggerManager _logger;
        private IActivityService _activityService;
        private IUserService _userService;

        [Authorize(Roles = UserRoles.Admin)]
        // GET: myActivities
        [HttpGet("all")]
        public ActionResult<IEnumerable<ActivityRequestDTO>> GetAllActivities()
        {
            var getActivitiesProcess = _userService.GetAllActivities();
            if (getActivitiesProcess.IsSuccessful)
            {
                return StatusCode(getActivitiesProcess.StatusCode, getActivitiesProcess.Payload);
            }

            return StatusCode(
                    getActivitiesProcess.StatusCode,
                    new InfoResponseDTO
                    {
                        Info = getActivitiesProcess.Info
                    }
                );
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // GET: myActivities, Params: userId
        [HttpGet]
        public ActionResult<IEnumerable<ActivityResponseDTO>> GetActivities(Guid userId, [FromQuery] PaginationDTO pagination)
        {
            var getActivitiesProcess = _userService.GetActivitiesByUserId(userId, pagination, Response);
            if (getActivitiesProcess.IsSuccessful)
            {
                return StatusCode(getActivitiesProcess.StatusCode, getActivitiesProcess.Payload);
            }

            return StatusCode(
                    getActivitiesProcess.StatusCode,
                    new InfoResponseDTO
                    {
                        Info = getActivitiesProcess.Info
                    }
                );
        }

        [Authorize(Roles = UserRoles.Admin)]
        // PUT myActivities, Params: activityId
        [HttpPut]
        public ActionResult UpdateActivity(Guid activityId, ActivityRequestDTO newActivity)
        {
            _logger.LogInfo("MyActivitiesController UpdateActivity - Getting specific activity to update...");
            var activityToBeUpdated = _activityService.GetActivityById(activityId);

            if (activityToBeUpdated != null)
            {
                _activityService.Update(activityToBeUpdated, newActivity);

                _logger.LogInfo("MyActivitiesController UpdateActivity - Update was successful.");
                return StatusCode(
                    StatusCodes.Status201Created, 
                    new InfoResponseDTO
                    {
                        Info = $"Update for {newActivity.ActivityEntityName} was successful."
                    }
                );
            }

            _logger.LogInfo("MyActivitiesController UpdateActivity - Update failed.");
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
            var deleteProcess = _activityService.Delete(activityId);

            return StatusCode(
                deleteProcess.StatusCode,
                new InfoResponseDTO
                {
                    Info = deleteProcess.Info
                });
        }
    }
}
