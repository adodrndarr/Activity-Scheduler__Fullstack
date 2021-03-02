using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;
using ActivityScheduler.Domain.Structs;
using ActivityScheduler.Services.Interfaces;
using ActivityScheduler.Presentation.EntitiesDTO;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActivityEntitiesController : ControllerBase
    {
        public ActivityEntitiesController(
            IActivityEntityService activityEntityService,
            ILoggerManager logger)
        {
            _logger = logger;
            _activityEntityService = activityEntityService;
        }


        private ILoggerManager _logger;
        private IActivityEntityService _activityEntityService;

        [Authorize(Roles = UserRoles.StandardUser)]
        // GET: activityEntities/all
        [HttpGet("all")]
        public ActionResult<IEnumerable<ActivityEntityDTO>> GetAllActivityEntities()
        {
            var getUsersProcess = _activityEntityService.GetAllActivityEntities();

            return StatusCode(getUsersProcess.StatusCode, getUsersProcess.Payload);
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // GET: activityEntities
        [HttpGet]
        public ActionResult<PagedList<ActivityEntityDTO>> GetActivityEntities([FromQuery] PaginationDTO pagination)
        {
            var getActivitiesProcess = _activityEntityService.GetAll(pagination, Response);
           
            if (getActivitiesProcess.IsSuccessful)
            {
                return StatusCode(
                    getActivitiesProcess.StatusCode, 
                    getActivitiesProcess.Payload);
            }

            return StatusCode(
                getActivitiesProcess.StatusCode,
                new InfoResponseDTO
                {
                    Info = getActivitiesProcess.Info
                });
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // GET: activityEntities/single
        [HttpGet("single")]
        public ActionResult<ActivityEntityDTO> GetActivityEntityById(Guid id)
        {
            var getUsersProcess = _activityEntityService.GetActivityEntityDTOById(id);

            return StatusCode(getUsersProcess.StatusCode, getUsersProcess.Payload);
        }

        [Authorize(Roles = UserRoles.Admin)]
        // POST: activityEntities
        [HttpPost]
        public ActionResult AddActivityEntity(ActivityEntityDTO newActivityEntity)
        {
            var additionProcess = _activityEntityService.Add(newActivityEntity);

            return StatusCode(
                additionProcess.StatusCode,
                new InfoResponseDTO
                {
                    Info = additionProcess.Info
                });
        }

        [Authorize(Roles = UserRoles.Admin)]
        // PUT activityEntities/upload
        [HttpPost("upload")]
        public ActionResult UploadFile()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var file = Request.Form.Files[0];

            var uploadProcess = _activityEntityService.UploadFile(file, currentDirectory);
            if (uploadProcess.IsSuccessful)
            {
                return StatusCode(
                    uploadProcess.StatusCode, 
                    new 
                    { 
                        serverFilePath = uploadProcess.Payload 
                    });
            }

            return StatusCode(
                uploadProcess.StatusCode, 
                new InfoResponseDTO
                {
                    Info = uploadProcess.Info
                });
        }

        [Authorize(Roles = UserRoles.Admin)]
        // PUT activityEntities, Params: activityId, body: newActivity
        [HttpPut]
        public ActionResult UpdateActivityEntity(Guid activityId, ActivityEntityDTO newActivity)
        {
            var activityFound = _activityEntityService.GetById(activityId);
            var updateProcess = _activityEntityService.Update(activityFound, newActivity);

            if (updateProcess.IsSuccessful)
            {
                return StatusCode(
                    updateProcess.StatusCode,
                    new InfoResponseDTO
                    {
                        Info = updateProcess.Info
                    });
            }

            return StatusCode(
                   updateProcess.StatusCode,
                   new InfoResponseDTO
                   {
                       Info = updateProcess.Info
                   });
        }

        [Authorize(Roles = UserRoles.Admin)]
        // DELETE: activityEntities, Params: activityEntityId
        [HttpDelete]
        public ActionResult DeleteActivityEntity(Guid activityEntityId)
        {
            var deletionProcess = _activityEntityService.Delete(activityEntityId);
            if (deletionProcess.IsSuccessful)
            {
                return StatusCode(
                    deletionProcess.StatusCode,
                    new InfoResponseDTO
                    {
                        Info = deletionProcess.Info
                    });
            }

            return StatusCode(
                deletionProcess.StatusCode,
                new InfoResponseDTO
                {
                    Info = deletionProcess.Info
                });
        }
    }
}
