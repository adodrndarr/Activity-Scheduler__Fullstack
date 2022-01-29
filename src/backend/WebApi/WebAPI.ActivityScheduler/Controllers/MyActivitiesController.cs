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
            if (!getActivitiesProcess.IsSuccessful)
            {
                var response = new InfoResponseDTO { Info = getActivitiesProcess.Info };
                return StatusCode(getActivitiesProcess.StatusCode, response);
            }

            return StatusCode(getActivitiesProcess.StatusCode, getActivitiesProcess.Payload);
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // GET: myActivities, Params: userId
        [HttpGet]
        public ActionResult<IEnumerable<ActivityResponseDTO>> GetActivities(Guid userId, [FromQuery] PaginationDTO pagination)
        {
            var getActivitiesProcess = _userService.GetActivitiesByUserId(userId, pagination, Response);
            if (!getActivitiesProcess.IsSuccessful)
            {
                var response = new InfoResponseDTO { Info = getActivitiesProcess.Info };
                return StatusCode(getActivitiesProcess.StatusCode, response);
            }

            return StatusCode(getActivitiesProcess.StatusCode, getActivitiesProcess.Payload);
        }

        [Authorize(Roles = UserRoles.Admin)]
        // PUT myActivities, Params: activityId
        [HttpPut]
        public ActionResult UpdateActivity(Guid activityId, ActivityRequestDTO newActivity)
        {
            var activityToBeUpdated = _activityService.GetActivityById(activityId);
            var updateProcess = _activityService.Update(activityToBeUpdated, newActivity);
            var response = new InfoResponseDTO { Info = updateProcess.Info };

            return StatusCode(updateProcess.StatusCode, response);
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // DELETE: myActivities, Params: activityId
        [HttpDelete]
        public ActionResult DeleteActivity(Guid activityId)
        {
            var deleteProcess = _activityService.Delete(activityId);
            var response = new InfoResponseDTO { Info = deleteProcess.Info };

            return StatusCode(deleteProcess.StatusCode, response);
        }
    }
}
