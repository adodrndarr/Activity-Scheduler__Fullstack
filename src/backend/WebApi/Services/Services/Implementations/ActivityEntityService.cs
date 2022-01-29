using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using ActivityScheduler.Services.Extensions;
using ActivityScheduler.Services.Interfaces;
using AutoMapper;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;


namespace ActivityScheduler.Services
{
    public class ActivityEntityService : IActivityEntityService
    {
        private IMapper _mapper;
        private IRepositoryContainer _repos;
        private ILoggerManager _logger;


        public ActivityEntityService(
            IRepositoryContainer repositoryContainer,
            IMapper mapper,
            ILoggerManager logger)
        {
            _mapper = mapper;
            _repos = repositoryContainer;
            _logger = logger;
        }

        public ResultDetails GetAll(PaginationDTO pagination, HttpResponse response)
        {
            _logger.LogInfo("ActivityEntityService GetAll - Getting activity entities...");

            var activities = _repos.ActivityEntityRepo.GetAllActivityEntities(pagination);
            var activitiesDTOs = _mapper.Map<PagedList<ActivityEntityDTO>>(activities);

            var paginationMetadata = new
            {
                activities.TotalCount,
                activities.CurrentPage,
                activities.PageSize,
                activities.TotalPages,
                activities.HasPrevious,
                activities.HasNext
            };

            response?.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            response?.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

            if (activitiesDTOs.Count > 0)
            {
                _logger.LogInfo($"ActivityEntityService GetAll - Returning { activitiesDTOs.Count } activity entities.");
                return new ResultDetails
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccessful = true,
                    Payload = activitiesDTOs
                };
            }

            _logger.LogInfo("ActivityEntityService GetAll - No activity entities available 0.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status200OK,
                IsSuccessful = false,
                Info = "Currently there are no activities available."
            };
        }

        public ResultDetails GetAllActivityEntities()
        {
            var activityEntitiesDTOs = GetAllAsDTOs();

            return new ResultDetails
            {
                StatusCode = StatusCodes.Status200OK,
                Payload = activityEntitiesDTOs
            };
        }

        public ResultDetails GetActivityEntityDTOById(Guid id)
        {
            var activityEntity = GetById(id);
            var activityEntityDTO = _mapper.Map<ActivityEntityDTO>(activityEntity);

            return new ResultDetails
            {
                StatusCode = StatusCodes.Status200OK,
                Payload = activityEntityDTO
            };
        }

        public ActivityEntity GetById(Guid activityEntityId)
        {
            _logger.LogInfo("ActivityEntityService GetById - Getting specific activity entity...");
            var activityEntity = _repos.ActivityEntityRepo.GetActivityEntityById(activityEntityId);

            return activityEntity;
        }

        public ResultDetails Add(ActivityEntityDTO newActivityEntity)
        {
            AddSingle(newActivityEntity);
            _repos.SaveChanges();

            _logger.LogInfo("ActivityEntityService Add - Addition of a new activity entity was successful.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status201Created,
                IsSuccessful = true,
                Info = $"The activity entity: {newActivityEntity.Name}, has been created."
            };
        }

        private void AddSingle(ActivityEntityDTO newActivityEntity)
        {
            var activityEntity = _mapper.Map<ActivityEntity>(newActivityEntity);
            _repos.ActivityEntityRepo.Create(activityEntity);
        }

        public void AddMany(List<ActivityEntityDTO> newActivityEntityDTOs)
        {
            newActivityEntityDTOs.ForEach(a => AddSingle(a));
            _repos.SaveChanges();
        }

        public ResultDetails Update(ActivityEntity entityToUpdate, ActivityEntityDTO newActivityEntity)
        {
            if (entityToUpdate != null)
            {
                entityToUpdate.ApplyActivityEntityDetails(newActivityEntity);

                _repos.ActivityEntityRepo.Update(entityToUpdate);
                _repos.SaveChanges();

                _logger.LogInfo("ActivityEntityService Update - Update was successful.");
                return new ResultDetails
                {
                    StatusCode = StatusCodes.Status201Created,
                    IsSuccessful = true,
                    Info = $"Update for {newActivityEntity?.Name} was successful."
                };
            }

            _logger.LogInfo("ActivityEntityService Update - Update failed.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccessful = false,
                Info = $"Could not update the activity, please make sure you provided valid Id"
            };
        }

        public ResultDetails Delete(Guid activityEntityId)
        {
            _logger.LogInfo("ActivityEntityService Delete - Getting activity entity for deletion.");

            var activityFound = GetById(activityEntityId);
            if (activityFound != null)
            {
                if (IsActivityEntityDeletable(activityEntityId))
                {
                    _repos.ActivityEntityRepo.Delete(activityFound);
                    _repos.SaveChanges();

                    _logger.LogInfo("ActivityEntityService Delete - Deletion of an activity entity was successful.");
                    return new ResultDetails
                    {
                        StatusCode = StatusCodes.Status200OK,
                        IsSuccessful = true,
                        Info = $"The activity entity: {activityFound.Name}, has been deleted."
                    };
                }
                else
                {
                    _logger.LogInfo("ActivityEntityService Delete - Deletion of an activity entity failed.");
                    return new ResultDetails
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        IsSuccessful = false,
                        Info = $"The activity with id: {activityEntityId}, could not be deleted."
                    };
                }
            }

            _logger.LogInfo("ActivityEntityService Delete - Deletion of an activity entity failed.");
            return new ResultDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccessful = false,
                Info = $"The activity entity with id: {activityEntityId}, could not be found."
            };
        }

        private bool IsActivityEntityDeletable(Guid activityEntityId)
        {
            var hasActivityEntity = _repos.ActivityRepo.HasActivityEntityWithId(activityEntityId);

            var isDeletable = (hasActivityEntity == true) ? false : true;
            return isDeletable;
        }

        public IEnumerable<ActivityEntityDTO> GetAllAsDTOs()
        {
            var activityEntities = _repos.ActivityEntityRepo.GetAllActivityEntities();
            var activityEntitiesDTOs = _mapper.Map<IEnumerable<ActivityEntityDTO>>(activityEntities);

            return activityEntitiesDTOs;
        }

        public ResultDetails UploadFile(IFormFile file, string currentDirectory)
        {
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var pathToSaveFile = Path.Combine("Ressources", "Images");
                var serverFilePath = Path.Combine(pathToSaveFile, fileName);

                var fullPathToSaveFile = Path.Combine(currentDirectory, pathToSaveFile);
                var fullPathWithFileName = Path.Combine(fullPathToSaveFile, fileName);

                using (var stream = new FileStream(fullPathWithFileName, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                CheckForFileCleanUp(fullPathToSaveFile, fullPathWithFileName, currentDirectory);

                return new ResultDetails
                {
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccessful = true,
                    Payload = serverFilePath
                };
            }

            return new ResultDetails
            {
                StatusCode = StatusCodes.Status400BadRequest,
                IsSuccessful = false,
                Info = "Invalid file provided."
            };
        }

        private void CheckForFileCleanUp(string fullPathToSaveFile, string fullPathWithCurrentFileName, string currentDirectory)
        {
            var activityEntities = GetAllAsDTOs();
            bool imageIsUsed = false;

            var fullImageFilesPaths = Directory.GetFiles(fullPathToSaveFile);
            foreach (var fullImgFilePath in fullImageFilesPaths)
            {
                var skipCheckForCurrentFile = (fullImgFilePath == fullPathWithCurrentFileName);
                if (skipCheckForCurrentFile)
                    continue;

                foreach (var activityEntity in activityEntities)
                {
                    var activityEntityImgPath = activityEntity?.ImagePath ?? "";
                    string currentActivityFullImgPath = Path.Combine(currentDirectory, activityEntityImgPath);

                    if (fullImgFilePath == currentActivityFullImgPath)
                    {
                        imageIsUsed = true;
                        break;
                    }

                    imageIsUsed = false;
                }

                if (!imageIsUsed)
                {
                    File.Delete(fullImgFilePath);
                }
            }
        }
    }
}
