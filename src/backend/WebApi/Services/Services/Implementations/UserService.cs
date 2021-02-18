using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Services.Interfaces;
using ActivityScheduler.Services.Extensions;
using AutoMapper;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ActivityScheduler.Domain.Structs;
using ActivityScheduler.Presentation.EntitiesDTO;
using Newtonsoft.Json;
using System.Security.Claims;


namespace ActivityScheduler.Services
{
    public class UserService : IUserService
    {
        private IRepositoryContainer _repos;
        private IMapper _mapper;
        private ILoggerManager _logger;
        private IAccountService _accountService;

        public UserService(
            IRepositoryContainer repositoryContainer,
            IMapper mapper,
            ILoggerManager logger,
            IAccountService accountService
            )
        {
            _repos = repositoryContainer;
            _mapper = mapper;
            _logger = logger;
            _accountService = accountService;
        }


        public ResultDetails GetAllDTOsWithDetails(PaginationDTO pagination, HttpResponse response)
        {
            _logger.LogInfo("UserService GetAllDTOsWithDetails - Getting users with activities and activity entities...");

            var users = _repos.UserRepo.GetAllUsersWithDetails(pagination);
            var usersDTOs = _mapper.Map<PagedList<UserDTO>>(users);

            var paginationMetadata = new
            {
                users.TotalCount,
                users.CurrentPage,
                users.PageSize,
                users.TotalPages,
                users.HasPrevious,
                users.HasNext
            };

            response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

            if (usersDTOs.Count > 0)
            {
                _logger.LogInfo($"UserService GetAllDTOsWithDetails - Returning {usersDTOs.Count} users.");
                return new ResultDetails
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccessful = true,
                    Payload = usersDTOs
                };
            }

            _logger.LogInfo("UserService GetAllDTOsWithDetails - No users available 0.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status200OK,
                IsSuccessful = false,
                Info = "Currently there are no users available."
            };
        }

        public IEnumerable<User> GetAllWithDetails()
        {
            var users = _repos.UserRepo.GetAllUsersWithDetails();
            
            return users;
        }

        public User GetByIdWithDetails(Guid userId)
        {
            var user = _repos.UserRepo.GetUserWithDetailsById(userId);

            return user;
        }

        public User GetById(Guid userId)
        {
            _logger.LogInfo("UserService GetById - Getting a specific user...");
            var user = _repos.UserRepo
                .GetByCondition(u => u.Id == userId.ToString())
                .SingleOrDefault();

            return user;
        }

        public ResultDetails GetUserDTOById(Guid id)
        {
            var user = GetById(id);
            var userDTO = _mapper.Map<UserDTO>(user);

            return new ResultDetails
            {
                StatusCode = StatusCodes.Status200OK,
                IsSuccessful = true,
                Payload = userDTO
            };
        }

        public async Task<ResultDetails> Update(User userToUpdate, UserUpdateDTO newUser, HttpContext context)
        {
            bool canUpdateUser = await CheckCurrentUserValidity(context, userToUpdate.Id);
            if (userToUpdate != null && canUpdateUser)
            {
                userToUpdate.UserName = newUser?.UserName ?? userToUpdate.UserName;
                userToUpdate.NormalizedUserName = newUser?.NormalizedUserName ?? userToUpdate.NormalizedUserName;
                userToUpdate.Email = newUser?.Email ?? userToUpdate.Email;
                userToUpdate.NormalizedEmail = newUser?.NormalizedEmail ?? userToUpdate.NormalizedEmail;
                userToUpdate.LastName = newUser?.LastName ?? userToUpdate.LastName;

                var hasher = new PasswordHasher<User>();
                string PwdHash = string.Empty;

                if (newUser.Password != null)
                {
                    PwdHash = hasher.HashPassword(userToUpdate, newUser.Password);
                }

                userToUpdate.PasswordHash = (newUser.Password == null) ? userToUpdate.PasswordHash : PwdHash;

                _repos.UserRepo.Update(userToUpdate);
                SaveChanges();

                await UpdateRole(userToUpdate, newUser);

                _logger.LogInfo("UserService Update - Update was successful.");
                return new ResultDetails
                {
                    StatusCode = StatusCodes.Status201Created,
                    IsSuccessful = true,
                    Info = $"Update for {newUser.UserName} was successful."
                };
            }

            _logger.LogInfo("UserService Update - Update failed.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccessful = false,
                Info = "Could not update the user, please make sure you provided a valid user and that the user exists."
            };
        }

        private async Task<bool> CheckCurrentUserValidity(HttpContext context, string id)
        {
            var currentUserEmail = context.User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _repos.UserRepo.GetByCondition(u => u.Email == currentUserEmail)
                                             .SingleOrDefault();
            var curreUserRoles = await _accountService.GetRolesAsync(currentUser);

            bool isAdmin = curreUserRoles.Any(role => role == UserRoles.Admin);
            bool isValid = currentUser.Id == id || isAdmin;

            return isValid;
        }

        private async Task UpdateRole(User userToUpdate, UserUpdateDTO newUser)
        {
            if (newUser.IsAdmin)
            {
                await _accountService.AddUserRoleAsync(userToUpdate, UserRoles.Admin);
            }
            else
            {
                await _accountService.RemoveAdminRoleAsync(userToUpdate, UserRoles.Admin);
            }
        }

        public ResultDetails Delete(User user)
        {
            if (user != null)
            {
                _repos.ActivityRepo.DeleteMany(user.Activities);
                _repos.UserRepo.Delete(user);
                SaveChanges();

                _logger.LogInfo("UserService Delete - Deletion was successful.");
                return new ResultDetails
                {
                    StatusCode = StatusCodes.Status200OK,
                    Info = $"The user: {user.UserName}, has been deleted."
                };
            }

            _logger.LogInfo("UserService Delete - Deletion failed.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Info = $"The user with such id, could not be found."
            };
        }

        public IEnumerable<ActivityResponseDTO> GetActivitiesFor(List<User> users)
        {
            var activities = users.SelectMany(u => u.Activities);
                                    
            var activitiesDTOs = _mapper.Map<IEnumerable<ActivityResponseDTO>>(activities);
            ApplyActivityEntityDetailsTo(activitiesDTOs);

            return activitiesDTOs;
        }

        public PagedList<ActivityResponseDTO> GetActivitiesFor(User user, PaginationDTO pagination, HttpResponse response)
        {
            var activities = user.Activities.AsQueryable();
            
            if (pagination.IncludeName != null)
            {
                activities = activities.Where(a => a.Name.ToLower()
                                                         .Contains(pagination.IncludeName.ToLower())
                                   );
            }

            var pagedActivities = PagedList<Activity>.ToPagedList(
                                activities,
                                pagination.PageNumber,
                                pagination.PageSize
                            );

            var paginationMetadata = new
            {
                pagedActivities.TotalCount,
                pagedActivities.CurrentPage,
                pagedActivities.PageSize,
                pagedActivities.TotalPages,
                pagedActivities.HasPrevious,
                pagedActivities.HasNext
            };

            response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

            var activitiesDTOs = _mapper.Map<PagedList<ActivityResponseDTO>>(pagedActivities);
            ApplyActivityEntityDetailsTo(activitiesDTOs);

            return activitiesDTOs;
        }

        private void ApplyActivityEntityDetailsTo(IEnumerable<ActivityResponseDTO> activitiesDTOs)
        {
            var ids = activitiesDTOs.Select(a => new Guid(a.ActivityEntityId));
            var activityEntities = _repos.ActivityEntityRepo.GetActivityEntitiesByIds(ids);

            foreach (var activity in activitiesDTOs)
            {
                foreach (var activityEntity in activityEntities)
                {
                    if (activity.ActivityEntityId == activityEntity.Id.ToString())
                    {
                        activity.ApplyActivityEntityDetails(activityEntity);
                    }
                }
            }
        }

        public ResultDetails GetAllActivities()
        {
            _logger.LogInfo("UserService GetAllWithDetails - Getting all users...");
            var users = GetAllWithDetails().ToList();

            if (users.Count > 0)
            {
                _logger.LogInfo("UserService GetAllActivities - Getting activities from all users...");
                var activitiesDTOs = GetActivitiesFor(users).ToList();

                if (activitiesDTOs.Count > 0)
                {
                    _logger.LogInfo($"UserService GetAllActivities - Returning {activitiesDTOs.Count} activities.");
                    return new ResultDetails
                    {
                        StatusCode = StatusCodes.Status200OK,
                        IsSuccessful = true,
                        Payload = activitiesDTOs
                    };
                }
            }

            _logger.LogInfo("UserService GetAllActivities - No activities available 0.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status200OK,
                IsSuccessful = false,
                Info = "Currently there are no activities available."
            };
        }

        public ResultDetails GetActivitiesByUserId(Guid userId, PaginationDTO pagination, HttpResponse response)
        {
            _logger.LogInfo("UserService GetActivitiesByUserId - Getting specific user...");
            var userFound = GetByIdWithDetails(userId);

            if (userFound != null)
            {
                _logger.LogInfo("UserService GetActivitiesByUserId - Getting activities from a specific user...");
                var activitiesDTOs = GetActivitiesFor(userFound, pagination, response);

                if (activitiesDTOs.Count > 0)
                {
                    _logger.LogInfo($"UserService GetActivitiesByUserId - Returning {activitiesDTOs.Count} activities.");
                    return new ResultDetails
                    {
                        StatusCode = StatusCodes.Status200OK,
                        IsSuccessful = true,
                        Payload = activitiesDTOs
                    };
                }
            }

            _logger.LogInfo("UserService GetActivitiesByUserId - No activities available 0.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status200OK,
                IsSuccessful = false,
                Info = "Currently there are no activities available."
            };
        }

        public void SaveChanges()
        {
            _repos.SaveChanges();
        }
    }
}
