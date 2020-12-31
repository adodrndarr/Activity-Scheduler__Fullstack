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
        public ScheduleActivityController(ActivitySchedulerDbContext dbContext, IMapper mapper, ActivityService activityService)
        {
            this._db = dbContext;
            this._mapper = mapper;
            this._activityService = activityService;
        }


        private readonly ActivitySchedulerDbContext _db;
        private readonly ActivityService _activityService;
        private readonly IMapper _mapper;
        
        [Authorize(Roles = UserRoles.StandardUser)]
        // POST: scheduleActivity
        [HttpPost]
        public ActionResult ScheduleActivity(Guid userId, [FromBody] ActivityDTO newActivityDTO)
        {
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
                
                return StatusCode(
                    StatusCodes.Status201Created, 
                    $"The activity entity: {newActivityDTO.ActivityEntity?.Name}, has been scheduled."
                );
            }

            return StatusCode(
                StatusCodes.Status400BadRequest, 
                $"Please provide a valid activity and id."
            );
        }
    }
}
