using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using ActivityScheduler.Domain.Structs;
using ActivityScheduler.Services.Interfaces;
using ActivityScheduler.Presentation.EntitiesDTO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Authorize(Roles = UserRoles.StandardUser)]
    [Route("[controller]")]
    [ApiController]
    public class ScheduleActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly IUserService _userService;
        private readonly IActivityEntityService _activityEntityService;

        public ScheduleActivityController(
            IActivityService activityService, 
            IUserService userService,
            IActivityEntityService activityEntityService)
        {
            _activityService = activityService;
            _userService = userService;
            _activityEntityService = activityEntityService;
        }


        // POST: scheduleActivity
        [HttpPost]
        public ActionResult ScheduleActivity(Guid userId, ActivityRequestDTO newActivityDTO)
        {
            var userFound = _userService.GetByIdWithDetails(userId);
            var scheduleProcess = _activityService.ScheduleActivity(userFound, newActivityDTO);
            var response = new InfoResponseDTO { Info = scheduleProcess.Info };

            return StatusCode(scheduleProcess.StatusCode, response);
        }

        // POST: scheduleActivity/multiple
        [HttpPost("multiple")]
        public ActionResult ScheduleActivities(Guid userId, List<ActivityRequestDTO> newActivityDTOs)
        {
            var userFound = _userService.GetByIdWithDetails(userId);
            var scheduleResponse = _activityService.ScheduleActivities(userFound, newActivityDTOs);

            return StatusCode(StatusCodes.Status200OK, scheduleResponse);
        }

        // GET: scheduleActivity/booked-activities
        [HttpGet("booked-activities")]
        public ActionResult GetBookedActivities(Guid activityEntityId, DateTime forDate)
        {
            var activityEntity = _activityEntityService.GetById(activityEntityId);
            var checkProcess = _activityService.CheckAvailability(activityEntity, forDate);

            return StatusCode(checkProcess.StatusCode, checkProcess.Payload);
        }
    }
}
