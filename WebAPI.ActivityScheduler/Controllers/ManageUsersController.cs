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
using AutoMapper;


namespace WebAPI.ActivityScheduler.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ManageUsersController : ControllerBase
    {
        public ManageUsersController(ActivitySchedulerDbContext dbContext, IMapper mapper)
        {
            this._db = dbContext;
            this._mapper = mapper;
        }


        private readonly ActivitySchedulerDbContext _db;
        private readonly IMapper _mapper;

        [Authorize(Roles = UserRoles.Admin)] 
        // GET: manageUsers
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers()
        {
            var users = GetDbUsers()
                    .AsQueryable()
                    .Include(u => u.Activities)
                    .ThenInclude(a => a.ActivityEntity)
                    .AsEnumerable();
            var usersDTO = _mapper.Map<List<UserDTO>>(users);
  
            if (usersDTO.Count > 0)
            {
                return StatusCode(StatusCodes.Status200OK, usersDTO);
            }

            return StatusCode(
                StatusCodes.Status404NotFound, 
                "Currently there are no users available."
            );
        }

        // PUT manageUsers, Params: userId  !!! danger: any user can update other users? :O how to prevent user to update any other users?
        [Authorize(Roles = UserRoles.StandardUser)]
        [HttpPut]
        public ActionResult UpdateUser(Guid userId, [FromBody] UserUpdateDTO newUser)
        {
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
                return StatusCode(
                    StatusCodes.Status201Created, 
                    $"Update for {newUser.UserName} was successful."
                );
            }

            return StatusCode(
                StatusCodes.Status400BadRequest, 
                "Could not update the user, please make sure you provided a valid user and that the user exists."
            );
        }

        [Authorize(Roles = UserRoles.Admin)]
        // DELETE: manageUsers, Params: userId
        [HttpDelete]
        public ActionResult DeleteUser(Guid userId)
        {
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

                return StatusCode(
                    StatusCodes.Status200OK, 
                    $"The user: {userFound.UserName}, has been deleted."
                );
            }

            return StatusCode(
                StatusCodes.Status400BadRequest, 
                $"The user with id: {userId}, could not be found."
            );
        }

        private IEnumerable<User> GetDbUsers()
        {
            return _db.Users;
        }
    }
}
