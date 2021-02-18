using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using ActivityScheduler.Services;
using ActivityScheduler.Services.Interfaces;
using AutoMapper;
using Domain.Repositories.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.ActivityScheduler.Controllers;


namespace Tests.Services
{
    [TestFixture]
    public class ActivityEntityServiceTests
    {
        private readonly Mock<IRepositoryContainer> _mockRepos;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly IActivityEntityService _activityEntityService;
        private readonly ActivityEntitiesController _activityEntityController;

        public ActivityEntityServiceTests()
        {
            _mockRepos = new Mock<IRepositoryContainer>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();
            
            _activityEntityService = new ActivityEntityService(
                _mockRepos.Object, 
                _mockMapper.Object, 
                _mockLogger.Object);
            _activityEntityController = new ActivityEntitiesController(
                _activityEntityService, 
                _mockLogger.Object);
        }


        PaginationDTO testPagination;
        PagedList<ActivityEntity> testActivityEntities;
        PagedList<ActivityEntityDTO> testMappedActivityEntities;

        [SetUp]
        public void Init()
        {
            testPagination = new PaginationDTO();
            testActivityEntities = new PagedList<ActivityEntity>() { new ActivityEntity(), new ActivityEntity() };
            testMappedActivityEntities = new PagedList<ActivityEntityDTO>() { new ActivityEntityDTO(), new ActivityEntityDTO() };

            SetupGetAllActivityEntities(testPagination, testActivityEntities);
            SetupPagedListMap(testActivityEntities, testMappedActivityEntities);
        }

        [Test]
        public void GetAll_PaginationPassed_ReturnsResultDetailsWithMappedActivityEntities()
        {
            // Act
            var result = _activityEntityService.GetAll(testPagination, _activityEntityController.Response);

            // Assert
            Assert.That(result, Is.InstanceOf<ResultDetails>());
            Assert.That(result.Payload, Is.InstanceOf<PagedList<ActivityEntityDTO>>());
        }

        [Test]
        public void GetAll_PaginationPassed_ReturnsRightNumberOfActivityEntityDTOs()
        {
            // Arrange
            var expectedCount = 2;
            var expectedToSucceed = true;

            // Act
            var result = _activityEntityService.GetAll(testPagination, _activityEntityController.Response);

            // Assert
            Assert.That(expectedCount, Is.EqualTo(result.Payload.Count));
            Assert.That(expectedToSucceed, Is.EqualTo(result.IsSuccessful));
        }

        [Test]
        public void GetAll_WhenNoActivityEntitiesAvailable_ReturnsNoActivityEntities()
        {
            // Arrange
            var expectedToSucceed = false;

            var testActivityEntities = new PagedList<ActivityEntity>() { };
            var testMappedActivityEntities = new PagedList<ActivityEntityDTO>() { };

            SetupGetAllActivityEntities(testPagination, testActivityEntities);
            SetupPagedListMap(testActivityEntities, testMappedActivityEntities);

            // Act
            var result = _activityEntityService.GetAll(testPagination, _activityEntityController.Response);

            // Assert
            Assert.That(null, Is.EqualTo(result.Payload));
            Assert.That(expectedToSucceed, Is.EqualTo(result.IsSuccessful));
        }


        [Test]
        public void GetAllActivityEntities_WhenCalled_ReturnsResultDetailsWithActivityEntities()
        {
            // Act
            var result = _activityEntityService.GetAllActivityEntities();

            // Assert
            Assert.That(result, Is.InstanceOf<ResultDetails>());
            Assert.That(result.Payload, Is.InstanceOf<IEnumerable<ActivityEntityDTO>>());
        }

        [Test]
        public void GetAllActivityEntities_WhenCalled_ReturnsRightNumberOfActivityEntityDTOs()
        {
            // Arrange
            var expectedCount = 2;

            SetupGetAllActivityEntities(testActivityEntities);
            SetupIEnumerableMap(testActivityEntities, testMappedActivityEntities);

            // Act
            var result = _activityEntityService.GetAllActivityEntities();

            // Assert
            Assert.That(expectedCount, Is.EqualTo(result.Payload.Count));
        }

        [Test]
        public void GetAllAsDTOs_WhenCalled_ReturnsRightNumberOfActivityEntityDTOs()
        {
            // Arrange
            var expectedCount = 2;

            SetupGetAllActivityEntities(testActivityEntities);
            SetupIEnumerableMap(testActivityEntities, testMappedActivityEntities);

            // Act
            var result = _activityEntityService.GetAllAsDTOs()
                                               .ToList();

            // Assert
            Assert.That(result, Is.InstanceOf<IEnumerable<ActivityEntityDTO>>());
            Assert.That(result.Count, Is.EqualTo(expectedCount));
        }

        [Test]

        public void GetById_UnknownGuidPassed_ReturnsNoActivityEntity()
        {
            // Arrange
            var unknownGuid = new Guid();
            SetupGetActivityEntityById(unknownGuid, null);

            // Act
            var resultActivityEntity = _activityEntityService.GetById(unknownGuid);

            // Assert
            Assert.That(null, Is.EqualTo(resultActivityEntity));
        }

        [Test]
        public void GetById_ExistingGuidPassed_ReturnsCorrectActivityEntity()
        {
            // Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var activityEntity = new ActivityEntity
            {
                Id = testGuid
            };

            SetupGetActivityEntityById(testGuid, activityEntity);

            // Act
            var resultActivityEntity = _activityEntityService.GetById(testGuid);

            // Assert
            Assert.That(resultActivityEntity, Is.InstanceOf<ActivityEntity>());
            Assert.That(resultActivityEntity.Id, Is.EqualTo(testGuid));
        }



        private void SetupGetAllActivityEntities(PaginationDTO paginationDTO, PagedList<ActivityEntity> activityEntities)
        {
            _mockRepos.Setup(container => container.ActivityEntityRepo.GetAllActivityEntities(paginationDTO))
                .Returns(activityEntities);
        }

        private void SetupGetAllActivityEntities(PagedList<ActivityEntity> activityEntities)
        {
            _mockRepos.Setup(container => container.ActivityEntityRepo.GetAllActivityEntities())
                .Returns(activityEntities);
        }

        private void SetupGetActivityEntityById(Guid id, ActivityEntity activityEntity)
        {
            _mockRepos.Setup(container => container.ActivityEntityRepo.GetActivityEntityById(id))
                .Returns(activityEntity);
        }

        private void SetupPagedListMap(PagedList<ActivityEntity> activityEntities, PagedList<ActivityEntityDTO> activityEntityDTOs)
        {
            _mockMapper.Setup(mapper => mapper.Map<PagedList<ActivityEntityDTO>>(activityEntities))
                .Returns(activityEntityDTOs);
        }

        private void SetupIEnumerableMap(PagedList<ActivityEntity> activityEntities, IEnumerable<ActivityEntityDTO> activityEntityDTOs)
        {
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ActivityEntityDTO>>(activityEntities))
                .Returns(activityEntityDTOs);
        }
    }
}
