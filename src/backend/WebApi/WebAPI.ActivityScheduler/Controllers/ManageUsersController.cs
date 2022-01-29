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
            if (!getUsersProcess.IsSuccessful)
            {
                var response = new InfoResponseDTO { Info = getUsersProcess.Info };
                return StatusCode(getUsersProcess.StatusCode, response);
            }

            return StatusCode(getUsersProcess.StatusCode, getUsersProcess.Payload);
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // GET: manageUsers/single
        [HttpGet("single")]
        public async Task<ActionResult<UserDTO>> GetUserById(Guid id)
        {
            var getUserProcess = await _userService.GetUserDTOById(id, HttpContext);

            return StatusCode(getUserProcess.StatusCode, getUserProcess.Payload);
        }

        [Authorize(Roles = UserRoles.StandardUser)]
        // PUT manageUsers, Params: userId 
        [HttpPut]
        public async Task<ActionResult> UpdateUser(Guid userId, UserUpdateDTO newUser)
        {
            var userToUpdate = _userService.GetById(userId);
            var updateProcess = await _userService.Update(userToUpdate, newUser, HttpContext);
            var response = new InfoResponseDTO { Info = updateProcess.Info };

            return StatusCode(updateProcess.StatusCode, response);
        }

        [Authorize(Roles = UserRoles.Admin)]
        // DELETE: manageUsers, Params: userId
        [HttpDelete]
        public ActionResult DeleteUser(Guid userId)
        {
            var userFound = _userService.GetByIdWithDetails(userId);
            var deletionProcess = _userService.Delete(userFound);
            var response = new InfoResponseDTO { Info = deletionProcess.Info };

            return StatusCode(deletionProcess.StatusCode, response);
        }
    }
}
