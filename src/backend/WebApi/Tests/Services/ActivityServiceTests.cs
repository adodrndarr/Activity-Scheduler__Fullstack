using ActivityScheduler.Domain.Entities;
using ActivityScheduler.Presentation.EntitiesDTO;
using ActivityScheduler.Services;
using ActivityScheduler.Services.HelperServices.Interfaces;
using ActivityScheduler.Services.Interfaces;
using AutoMapper;
using Domain.Repositories.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Services.Test.Services
{
    [TestFixture]
    public class ActivityServiceTests
    {
        private readonly Mock<IRepositoryContainer> _mockRepos;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<ICommonService> _commonService;
        private readonly IActivityService _activityService;

        public ActivityServiceTests()
        {
            _mockRepos = new Mock<IRepositoryContainer>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerManager>();
            _commonService = new Mock<ICommonService>();

            _activityService = new ActivityService(
                _mockRepos.Object,
                _commonService.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }


        private User _testUser;
        private ActivityRequestDTO _testNewActivityDTO;
        private ActivityEntity _testActivityEntity;
        private Activity _testActivity;
        private List<Activity> _testActivities;

        [SetUp]
        public void Init()
        {
            InitializeEntities();
        }

        [Test]
        public void GetActivityById_UnknownGuidPassed_ReturnsNoActivity()
        {
            // Arrange
            var unknownGuid = new Guid();
            SetupGetActivityById(null);

            // Act
            var expectedActivity = _activityService.GetActivityById(unknownGuid);

            // Assert
            expectedActivity.Should().Be(null, because: "Invalid Guid was passed");
        }

        [Test]
        public void GetActivityById_ExistingGuidPassed_ReturnsCorrectActivity()
        {
            // Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var activity = new Activity
            {
                Id = testGuid
            };

            SetupGetActivityById(activity);

            // Act
            var resultActivity = _activityService.GetActivityById(testGuid);

            // Assert
            resultActivity.Should().BeOfType(typeof(Activity));
            resultActivity.Id.Should().Be(testGuid);
        }


        [Test]
        public void ScheduleActivity_InvalidUserPassed_ReturnsUnsuccessfulResult()
        {
            // Arrange
            var expectedToSucceed = false;

            // Act
            var result = _activityService.ScheduleActivity(null, _testNewActivityDTO);

            // Assert
            result.IsSuccessful.Should().Be(expectedToSucceed);
        }

        [Test]
        public void ScheduleActivity_InvalidDatePassed_ReturnsUnsuccessfulResult()
        {
            // Arrange
            _testNewActivityDTO.BookedForDate = _testNewActivityDTO.BookedForDate.AddDays(-2);

            // Act
            var result = _activityService.ScheduleActivity(_testUser, _testNewActivityDTO);

            // Assert
            result.IsSuccessful.Should().Be(false);
        }

        [Test]
        public void ScheduleActivity_InvalidActivityEntityPassed_ReturnsUnsuccessfulResult()
        {
            // Arrange
            SetupGetActivityEntityByName(null);
            _testNewActivityDTO.ActivityEntityId = null;

            // Act
            var result = _activityService.ScheduleActivity(_testUser, _testNewActivityDTO);

            // Assert
            result.IsSuccessful.Should().Be(false);
        }

        [Test]
        public void ScheduleActivity_WhenTimeSlotIsNotAvailable_ReturnsUnsuccessfulResult()
        {
            // Arrange
            SetupGetActivityEntityByName(_testActivityEntity);
            SetupActivityGetByCondition();

            // Act
            var result = _activityService.ScheduleActivity(_testUser, _testNewActivityDTO);

            // Assert
            result.IsSuccessful.Should().Be(false);
        }

        [Test]
        public void ScheduleActivity_WhenThereAreNoBookedActivities_ReturnsSuccessfulResult()
        {
            // Arrange
            _testActivityEntity.ItemQuantity = 3;

            SetupGetActivityEntityByName(_testActivityEntity);
            SetupActivityGetByCondition();
            SetupActivityMap(_testActivity, _testNewActivityDTO);
            _mockRepos.Setup(container => container.SaveChanges());

            // Act
            var result = _activityService.ScheduleActivity(_testUser, _testNewActivityDTO);

            // Assert
            result.IsSuccessful.Should().Be(true);
        }


        [Test]
        public void IsScheduleTimeAvailable_WhenTimeSlotIsNotAvailable_ReturnsFalse()
        {
            // Arrange
            var expectedToSucceed = false;

            // Act
            var result = _activityService.IsScheduleTimeAvailable(_testNewActivityDTO, _testActivities);

            // Assert
            expectedToSucceed.Should().Be(result);
        }

        [Test]
        public void IsScheduleTimeAvailable_WhenTimeSlotIsAvailable_ReturnsTrue()
        {
            // Arrange
            foreach (var testActivity in _testActivities)
            {
                testActivity.StartTime = _testNewActivityDTO.EndTime;
                testActivity.StartTime = testActivity.StartTime.AddHours(2);

                testActivity.EndTime = testActivity.StartTime;
                testActivity.EndTime = testActivity.EndTime.AddHours(2);
            }

            // Act
            bool result = _activityService.IsScheduleTimeAvailable(_testNewActivityDTO, _testActivities);

            // Assert
            result.Should().Be(true);
        }


        [Test]
        public void GetBookedActivities_WhenAllQuantitiesAreNotTaken_ReturnsNoBookedActivities()
        {
            // Arrange
            var expectToHaveNoActivities = true;

            // Act
            _testActivityEntity.ItemQuantity = 3;
            SetupActivityGetByCondition();

            var activities = _activityService.GetBookedActivities(_testActivityEntity, _testNewActivityDTO.BookedForDate);
            bool activitiesResult = activities.Count < 1;

            // Assert
            activitiesResult.Should().Be(expectToHaveNoActivities);
        }

        [Test]
        public void GetBookedActivities_WhenAllQuantitiesAreTaken_ReturnsBookedActivities()
        {
            // Arrange
            SetupActivityGetByCondition();

            // Act
            var resultActivities = _activityService.GetBookedActivities(_testActivityEntity, _testNewActivityDTO.BookedForDate);
            bool haveBookedActivities = resultActivities.Count > 0;

            // Assert
            haveBookedActivities.Should().Be(true);
        }


        [Test]
        public void CheckAvailability_InvalidActivityEntityPassed_ReturnsInvalidResultDetails()
        {
            // Arrange
            ActivityEntity invalidActivityEntity = null;

            // Act
            var resultDetails = _activityService.CheckAvailability(invalidActivityEntity, DateTime.Now);
            bool isValidResult = resultDetails.Payload.IsValid;

            // Assert
            resultDetails.Should().BeOfType(typeof(ResultDetails));
            isValidResult.Should().Be(false);
        }

        [Test]
        public void CheckAvailability_InvalidDatePassed_ReturnsInvalidResultDetails()
        {
            // Arrange
            var invalidDate = DateTime.Now.AddDays(-1);

            // Act
            var resultDetails = _activityService.CheckAvailability(_testActivityEntity, invalidDate);
            bool isValidResult = resultDetails.Payload.IsValid;

            // Assert
            resultDetails.Should().BeOfType(typeof(ResultDetails));
            isValidResult.Should().Be(false);
        }

        [Test]
        public void CheckAvailability_WhenNoBookedActivities_ReturnsValidResultDetails()
        {
            // Arrange
            _testActivityEntity.ItemQuantity = 3;

            SetupActivityGetByCondition();

            // Act
            var resultDetails = _activityService.CheckAvailability(_testActivityEntity, DateTime.Now);
            bool isValidResult = resultDetails.Payload.IsValid;

            // Assert
            resultDetails.Should().BeOfType(typeof(ResultDetails));
            isValidResult.Should().Be(true);
        }

        [Test]
        public void CheckAvailability_WhenHaveBookedActivities_ReturnsBookedDetails()
        {
            // Arrange
            SetupActivityGetByCondition();
            SetupBookedActivityMap(_testActivities, new List<BookedActivityDTO>());

            // Act
            var resultDetails = _activityService.CheckAvailability(_testActivityEntity, _testActivity.BookedForDate);

            // Assert
            resultDetails.Should().BeOfType(typeof(ResultDetails));
            Assert.That(resultDetails.Payload, Is.InstanceOf<IEnumerable<BookedActivityDTO>>());
        }


        [Test]
        public void Delete_UnknownGuidPassed_ReturnsInvalidResultDetails()
        {
            // Arrange
            var unknownGuid = new Guid();
            SetupGetActivityById(null);

            // Act
            var deleteResult = _activityService.Delete(unknownGuid);

            // Assert
            deleteResult.IsSuccessful.Should().Be(false);
        }

        [Test]
        public void Delete_ExistingGuidPassed_ReturnsCorrectResultDetails()
        {
            // Arrange
            var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
            var activity = new Activity
            {
                Id = testGuid
            };

            SetupGetActivityById(activity);

            // Act
            var deleteResult = _activityService.Delete(testGuid);

            // Assert
            deleteResult.IsSuccessful.Should().Be(true);
        }


        [Test]
        public void ActivityEntityExists_UnknownNamePassed_ReturnsFalse()
        {
            // Arrange
            var invalidName = "Non existing Name";
            SetupGetActivityEntityByName(null);

            // Act
            bool existsResult = _activityService.ActivityEntityExists(invalidName);

            // Assert
            existsResult.Should().Be(false);
        }

        [Test]
        public void ActivityEntityExists_ExistingNamePassed_ReturnsTrue()
        {
            // Arrange
            var existingName = "Billiard";
            SetupGetActivityEntityByName(_testActivityEntity);

            // Act
            bool existsResult = _activityService.ActivityEntityExists(existingName);

            // Assert
            existsResult.Should().Be(true, because: "existing name was passed");
        }


        [Test]
        public void Add_WhenCalled_CreatesNewActivity()
        {
            // Arrange
            var activityRequestDTO = _testNewActivityDTO;
            SetupActivityMap(_testActivity, activityRequestDTO);
            SetupGetActivityEntityByName(_testActivityEntity);
            SetupActivityCreate();

            // Act
            _activityService.Add(activityRequestDTO, _testUser);

            // Assert
            _mockRepos.Verify(container => container.ActivityRepo.Create(_testActivity), Times.Once);
        }

        [Test]
        [Order(1)]
        public void AddMany_WhenCreatingNewActivities_ExecutesSaveChangesOnce()
        {
            // Arrange
            var activityRequestDTOs = new List<ActivityRequestDTO>()
            {
                _testNewActivityDTO,
                _testNewActivityDTO
            };

            SetupActivityMap(_testActivity, _testNewActivityDTO);
            SetupGetActivityEntityByName(_testActivityEntity);
            SetupActivityCreate();

            // Act
            _activityService.AddMany(activityRequestDTOs, _testUser);

            // Assert
            _mockRepos.Verify(container => container.ActivityRepo.Create(_testActivity),
                                           Times.Exactly(activityRequestDTOs.Count));

            _mockRepos.Verify(container => container.SaveChanges(), Times.Once);
        }

        [Test]
        public void Update_WhenCalled_ShouldUpdateActivity()
        {
            // Arrange
            SetupGetActivityEntityByName(_testActivityEntity);
            SetupActivityUpdate();
            _mockRepos.Setup(container => container.SaveChanges());

            // Act
            _activityService.Update(_testActivity, _testNewActivityDTO);

            // Assert
            _mockRepos.Verify(container => container.ActivityRepo.Update(_testActivity), Times.Once);
        }

        [Test]
        public void Update_InvalidItemPassed_ShouldThrowNullReferenceException()
        {
            // Arrange
            SetupActivityUpdate();
            SetupGetActivityEntityByName(_testActivityEntity);

            // Act
            var updateWithInvalidActivity = _activityService.Update(null, _testNewActivityDTO);
            var updateWithInvalidParams = _activityService.Update(null, null);
            Action updateWithInvalidActivityRequest = () => _activityService.Update(_testActivity, null);

            // Assert           
            updateWithInvalidActivity.Should().BeOfType(typeof(ResultDetails));
            updateWithInvalidActivity.IsSuccessful.Should().Be(false);
            
            updateWithInvalidParams.Should().BeOfType(typeof(ResultDetails));
            updateWithInvalidParams.IsSuccessful.Should().Be(false);

            updateWithInvalidActivityRequest.Should().Throw<NullReferenceException>();
        }

        [Test]
        public void SaveChanges_WhenUnableToSaveChanges_ShouldThrowInvalidOperationException()
        {
            // Arrange
            SetupSaveChanges();

            // Act
            Action saveChangesUnsuccessfully = () => _activityService.SaveChanges();

            // Assert           
            saveChangesUnsuccessfully.Should().Throw<InvalidOperationException>();
        }



        private void SetupGetActivityById(Activity activity)
        {
            _mockRepos.Setup(container => container.ActivityRepo.GetActivityById(It.IsAny<Guid>()))
                .Returns(activity);
        }

        private void SetupGetActivityEntityByName(ActivityEntity activityEntity)
        {
            _mockRepos.Setup(container =>
                             container.ActivityEntityRepo
                                      .GetActivityEntityByName(It.IsAny<string>())
                      )
                      .Returns(activityEntity);
        }

        private void SetupActivityGetByCondition()
        {
            _mockRepos.Setup(container => 
                             container.ActivityRepo.GetByCondition(It.IsAny<Expression<Func<Activity, bool>>>()))
                      .Returns(_testActivities.AsQueryable());
        }


        private void SetupActivityMap(Activity activity, ActivityRequestDTO activityDTO)
        {
            _mockMapper.Setup(mapper => mapper.Map<Activity>(activityDTO))
                .Returns(activity);
        }

        private void SetupBookedActivityMap(List<Activity> bookedActivities, IEnumerable<BookedActivityDTO> bookedActivityDTOs)
        {
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<BookedActivityDTO>>(bookedActivities))
                .Returns(bookedActivityDTOs);
        }


        private void SetupActivityCreate()
        {
            _mockRepos.Setup(container => container.ActivityRepo.Create(It.IsAny<Activity>()));
        }

        private void SetupActivityUpdate()
        {
            _mockRepos.Setup(container => container.ActivityRepo.Update(It.IsAny<Activity>()));
        }

        private void SetupSaveChanges()
        {
            _mockRepos.Setup(container => container.SaveChanges())
                .Throws<InvalidOperationException>();
        }


        private void InitializeEntities()
        {
            _testUser = new User
            {
                Id = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c200",
                UserName = "Bob",
                Email = "bob@gmail.com",
                IsAdmin = false,
                LastName = "Bobsky"
            };

            _testNewActivityDTO = new ActivityRequestDTO
            {
                ActivityEntityId = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c201",
                BookedForDate = DateTime.Now.AddDays(2),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2),
                ActivityEntityName = "Billiard"
            };

            _testActivityEntity = new ActivityEntity
            {
                Name = "Billiard",
                Id = new Guid(_testNewActivityDTO.ActivityEntityId),
                Description = "Test description",
                ImagePath = "Test url",
                ItemQuantity = 2,
                Location = "Test Location",
                MaxUserCount = 3,
                MinUserCount = 2
            };

            _testActivity = new Activity
            {
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c202"),
                ActivityEntityId = _testActivityEntity.Id.ToString(),
                BookedForDate = _testNewActivityDTO.BookedForDate,
                DateBooked = DateTime.Now,
                Duration = "Test duration",
                EndTime = _testNewActivityDTO.EndTime,
                Name = "Billiard",
                OrganizerName = _testUser.UserName,
                StartTime = _testNewActivityDTO.StartTime,
                UserId = _testUser.Id
            };

            _testActivities = new List<Activity>
            {
                _testActivity,
                _testActivity
            };
        }

    }
}
