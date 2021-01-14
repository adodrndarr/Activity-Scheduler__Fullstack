using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;
using System.Linq;
using WebAPI.ActivityScheduler.DataAccess;
using WebAPI.ActivityScheduler.Entities;
using WebAPI.ActivityScheduler.EntitiesDTO.ManageUsers;
using WebAPI.ActivityScheduler.EntitiesDTO;
using WebAPI.ActivityScheduler.Services;
using AutoMapper;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManageUsersController : ControllerBase
    {
        public ManageUsersController(
            ActivitySchedulerDbContext dbContext, 
            IMapper mapper,
            ILoggerManager logger)
        {
            this._db = dbContext;
            this._mapper = mapper;
            this._logger = logger;
        }


        private readonly ActivitySchedulerDbContext _db;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        [Authorize(Roles = UserRoles.Admin)] 
        // GET: manageUsers
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers()
        {
            this._logger.LogInfo("ManageUsersController GetUsers - Getting users with activities and activity entities...");
            var users = GetDbUsers()
                    .AsQueryable()
                    .Include(u => u.Activities)
                    .ThenInclude(a => a.ActivityEntity)
                    .AsEnumerable();
            var usersDTO = _mapper.Map<List<UserDTO>>(users);
  
            if (usersDTO.Count > 0)
            {
                this._logger.LogInfo($"ManageUsersController GetUsers - Returning {usersDTO.Count} users.");
                return StatusCode(StatusCodes.Status200OK, usersDTO);
            }

            this._logger.LogInfo("ManageUsersController GetUsers - No users available 0.");
            return StatusCode(
                StatusCodes.Status404NotFound, 
                new InfoResponseDTO
                {
                    Info = "Currently there are no users available."
                }
            );
        }

        // PUT manageUsers, Params: userId  !!! danger: any user can update other users? :O how to prevent user to update any other users?
        [Authorize(Roles = UserRoles.StandardUser)]
        [HttpPut]
        public ActionResult UpdateUser(Guid userId, [FromBody] UserUpdateDTO newUser)
        {
            this._logger.LogInfo("ManageUsersController UpdateUser - Getting a user to update...");
            var userToBeUpdated = GetDbUsers().FirstOrDefault(u => u.Id == userId.ToString());
            if (newUser != null && userToBeUpdated != null)
            {                
                userToBeUpdated.UserName = newUser.UserName;
                userToBeUpdated.NormalizedUserName = newUser.NormalizedUserName;
                userToBeUpdated.Email = newUser.Email;
                userToBeUpdated.NormalizedEmail = newUser.NormalizedEmail;
                userToBeUpdated.LastName = newUser.LastName;

                var hasher = new PasswordHasher<User>();
                string PwdHash = string.Empty;

                if (newUser.Password != null)
                {
                    PwdHash = hasher.HashPassword(userToBeUpdated, newUser.Password);
                }

                userToBeUpdated.PasswordHash = (newUser.Password == null) ? userToBeUpdated.PasswordHash : PwdHash;                
                _db.SaveChanges();

                this._logger.LogInfo("ManageUsersController UpdateUser - Update was successful.");
                return StatusCode(
                    StatusCodes.Status201Created,
                    new InfoResponseDTO
                    {
                        Info = $"Update for {newUser.UserName} was successful."
                    }
                );
            }

            this._logger.LogInfo("ManageUsersController UpdateUser - Update failed.");
            return StatusCode(
                StatusCodes.Status400BadRequest,
                new InfoResponseDTO
                {
                    Info = "Could not update the user, please make sure you provided a valid user and that the user exists."
                }
            );
        }

        [Authorize(Roles = UserRoles.Admin)]
        // DELETE: manageUsers, Params: userId
        [HttpDelete]
        public ActionResult DeleteUser(Guid userId)
        {
            this._logger.LogInfo("ManageUsersController DeleteUser - Getting a user for deletion...");
            var userFound = GetDbUsers()
                .AsQueryable()
                .Include(u => u.Activities)
                .ThenInclude(a => a.ActivityEntity)
                .FirstOrDefault(u => u.Id == userId.ToString());

            if (userFound != null)
            {
                var userActivityEntities = userFound.Activities.Select(a => a.ActivityEntity);

                _db.ActivityEntities.RemoveRange(userActivityEntities);
                _db.Activities.RemoveRange(userFound.Activities);                
                _db.Users.Remove(userFound);
                _db.SaveChanges();

                this._logger.LogInfo("ManageUsersController DeleteUser - Deletion was successful.");
                return StatusCode(
                    StatusCodes.Status200OK,
                    new InfoResponseDTO
                    {
                        Info = $"The user: {userFound.UserName}, has been deleted."
                    }
                );
            }

            this._logger.LogInfo("ManageUsersController DeleteUser - Deletion failed.");
            return StatusCode(
                StatusCodes.Status400BadRequest,
                new InfoResponseDTO
                {
                    Info = $"The user with id: {userId}, could not be found."
                }
            );
        }

        private IEnumerable<User> GetDbUsers()
        {
            return _db.Users;
        }
    }
}
