using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebAPI.ActivityScheduler.Entities;
using WebAPI.ActivityScheduler.DataAccess;
using WebAPI.ActivityScheduler.EntitiesDTO;
using System.Linq;
using System;
using AutoMapper;
using WebAPI.ActivityScheduler.Services;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ScheduleActivityController : ControllerBase
    {
        public ScheduleActivityController(
            ActivitySchedulerDbContext dbContext, 
            IMapper mapper, 
            IActivityService activityService,
            ILoggerManager logger)
        {
            this._db = dbContext;
            this._mapper = mapper;
            this._activityService = activityService;
            this._logger = logger;
        }


        private readonly ActivitySchedulerDbContext _db;
        private readonly IActivityService _activityService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        
        [Authorize(Roles = UserRoles.StandardUser)]
        // POST: scheduleActivity
        [HttpPost]
        public ActionResult ScheduleActivity(Guid userId, [FromBody] ActivityDTO newActivityDTO)
        {
            this._logger.LogInfo("ScheduleActivityController ScheduleActivity - Getting specific user...");
            var userFound = _db.Users
                .AsQueryable()
                .Include(u => u.Activities)
                .ThenInclude(u => u.ActivityEntity)
                .FirstOrDefault(u => u.Id == userId.ToString());

            if (newActivityDTO != null && userFound != null)
            {
                var newActivity = _mapper.Map<Activity>(newActivityDTO);
                var duration = _activityService.CalculateDuration(newActivity.StartTime, newActivity.EndTime);

                newActivity.User = userFound;
                newActivity.Duration = duration;
                newActivity.DateBooked = DateTime.Now;

                _db.Activities.Add(newActivity);
                _db.SaveChanges();

                this._logger.LogInfo("ScheduleActivityController ScheduleActivity - Scheduling of an activity was successful.");
                return StatusCode(
                    StatusCodes.Status201Created, 
                    new InfoResponseDTO
                    {
                        Info = $"The activity entity: {newActivityDTO.ActivityEntity?.Name}, has been scheduled."
                    }
                );
            }

            this._logger.LogInfo("ScheduleActivityController ScheduleActivity - Scheduling of an activity failed.");
            return StatusCode(
                StatusCodes.Status400BadRequest, 
                new InfoResponseDTO
                {
                    Info = $"Please provide a valid activity and id."
                }
            );
        }
    }
}
