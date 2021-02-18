using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ActivityScheduler.Domain.Structs;
using ActivityScheduler.Services.Interfaces;
using ActivityScheduler.Presentation.EntitiesDTO;
using Microsoft.AspNetCore.Identity;
using ActivityScheduler.Domain.Entities;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManageUsersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public ManageUsersController(
            IUserService userService,
            ILoggerManager logger,
            UserManager<User> userManager)
        {
            _logger = logger;
            _userService = userService;
            _userManager = userManager;
        }


        [Authorize(Roles = UserRoles.Admin)]
        // GET: manageUsers
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers([FromQuery] PaginationDTO pagination)
        {
            var getUsersProcess = _userService.GetAllDTOsWithDetails(pagination, Response);
            if (getUsersProcess.IsSuccessful)
            {
                return StatusCode(getUsersProcess.StatusCode, getUsersProcess.Payload);
            }

            return StatusCode(
                getUsersProcess.StatusCode, 
                new InfoResponseDTO
                {
                    Info = getUsersProcess.Info
                });
        }

        [Authorize(Roles = UserRoles.Admin)]
        // GET: manageUsers/single
        [HttpGet("single")]
        public ActionResult<ActivityEntityDTO> GetUserById(Guid id)
        {
            var getUserProcess = _userService.GetUserDTOById(id);

            return StatusCode(getUserProcess.StatusCode, getUserProcess.Payload);
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // PUT manageUsers, Params: userId 
        [HttpPut]
        public async Task<ActionResult> UpdateUser(Guid userId, UserUpdateDTO newUser)
        {
            var userToUpdate = _userService.GetById(userId);
            var updateProcess = await _userService.Update(userToUpdate, newUser, HttpContext);

            if (updateProcess.IsSuccessful)
            {
                return StatusCode(
                    updateProcess.StatusCode,
                    new InfoResponseDTO
                    {
                        Info = updateProcess.Info
                    }
                );
            }

            return StatusCode(
                updateProcess.StatusCode,
                new InfoResponseDTO
                {
                    Info = updateProcess.Info
                }
            );
        }

        [Authorize(Roles = UserRoles.Admin)]
        // DELETE: manageUsers, Params: userId
        [HttpDelete]
        public ActionResult DeleteUser(Guid userId)
        {
            var userFound = _userService.GetByIdWithDetails(userId);
            var deletionProcess = _userService.Delete(userFound);

            if (deletionProcess.IsSuccessful)
            {
                return StatusCode(
                    deletionProcess.StatusCode,
                    new InfoResponseDTO
                    {
                        Info = deletionProcess.Info
                    }
                );
            }

            return StatusCode(
                deletionProcess.StatusCode,
                new InfoResponseDTO
                {
                    Info = deletionProcess.Info
                }
            );
        }
    }
}
