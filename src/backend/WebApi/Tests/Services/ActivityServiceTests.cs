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


        User testUser;
        ActivityRequestDTO testNewActivityDTO;
        ActivityEntity testActivityEntity;
        Activity testActivity;
        List<Activity> testActivities;

        [SetUp]
        public void Init()
        {
            InitializeEntities();
        }

        #region Assert.That() syntax

        //[Test]
        //public void GetActivityById_UnknownGuidPassed_ReturnsNoActivity()
        //{
        //    // Arrange
        //    var unknownGuid = new Guid();
        //    SetupGetActivityById(null);

        //    // Act
        //    var expectedActivity = _activityService.GetActivityById(unknownGuid);

        //    // Assert
        //    Assert.That(null, Is.EqualTo(expectedActivity));
        //}

        //[Test]
        //public void GetActivityById_ExistingGuidPassed_ReturnsCorrectActivity()
        //{
        //    // Arrange
        //    var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
        //    var activity = new Activity
        //    {
        //        Id = testGuid
        //    };

        //    SetupGetActivityById(activity);

        //    // Act
        //    var resultActivity = _activityService.GetActivityById(testGuid);

        //    // Assert
        //    Assert.That(resultActivity, Is.InstanceOf<Activity>());
        //    Assert.That(resultActivity.Id, Is.EqualTo(testGuid));
        //}


        //[Test]
        //public void ScheduleActivity_InvalidUserPassed_ReturnsUnsuccessfulResult()
        //{
        //    // Arrange
        //    var expectedToSucceed = false;

        //    // Act
        //    var result = _activityService.ScheduleActivity(null, testNewActivityDTO);

        //    // Assert
        //    Assert.That(expectedToSucceed, Is.EqualTo(result.IsSuccessful));
        //    // Assert.AreEqual(expectedToSucceed, result.IsSuccessful);
        //}

        //[Test]
        //public void ScheduleActivity_InvalidDatePassed_ReturnsUnsuccessfulResult()
        //{
        //    // Arrange
        //    testNewActivityDTO.BookedForDate = testNewActivityDTO.BookedForDate.AddDays(-2);

        //    // Act
        //    var result = _activityService.ScheduleActivity(testUser, testNewActivityDTO);

        //    // Assert
        //    Assert.That(result.IsSuccessful, Is.False);
        //}

        //[Test]
        //public void ScheduleActivity_InvalidActivityEntityPassed_ReturnsUnsuccessfulResult()
        //{
        //    // Arrange
        //    SetupGetActivityEntityByName(null);
        //    testNewActivityDTO.ActivityEntityId = null;

        //    // Act
        //    var result = _activityService.ScheduleActivity(testUser, testNewActivityDTO);

        //    // Assert
        //    Assert.That(result.IsSuccessful, Is.False);
        //}

        //[Test]
        //public void ScheduleActivity_WhenTimeSlotIsNotAvailable_ReturnsUnsuccessfulResult()
        //{
        //    // Arrange
        //    SetupGetActivityEntityByName(testActivityEntity);
        //    SetupActivityGetByCondition();

        //    // Act
        //    var result = _activityService.ScheduleActivity(testUser, testNewActivityDTO);

        //    // Assert
        //    Assert.That(result.IsSuccessful, Is.False);
        //}

        //[Test]
        //public void ScheduleActivity_WhenThereAreNoBookedActivities_ReturnsSuccessfulResult()
        //{
        //    // Arrange
        //    testActivityEntity.ItemQuantity = 3;

        //    SetupGetActivityEntityByName(testActivityEntity);
        //    SetupActivityGetByCondition();
        //    SetupActivityMap(testActivity, testNewActivityDTO);

        //    // Act
        //    var result = _activityService.ScheduleActivity(testUser, testNewActivityDTO);

        //    // Assert
        //    Assert.That(result.IsSuccessful, Is.True);
        //}


        //[Test]
        //public void IsScheduleTimeAvailable_WhenTimeSlotIsNotAvailable_ReturnsFalse()
        //{
        //    // Arrange
        //    var expectedToSucceed = false;

        //    // Act
        //    var result = _activityService.IsScheduleTimeAvailable(testNewActivityDTO, testActivities);

        //    // Assert
        //    Assert.That(expectedToSucceed, Is.EqualTo(result));
        //}

        //[Test]
        //public void IsScheduleTimeAvailable_WhenTimeSlotIsAvailable_ReturnsTrue()
        //{
        //    // Arrange
        //    foreach (var testActivity in testActivities)
        //    {
        //        testActivity.StartTime = testNewActivityDTO.EndTime;
        //        testActivity.StartTime = testActivity.StartTime.AddHours(2);

        //        testActivity.EndTime = testActivity.StartTime;
        //        testActivity.EndTime = testActivity.EndTime.AddHours(2);
        //    }

        //    // Act
        //    bool result = _activityService.IsScheduleTimeAvailable(testNewActivityDTO, testActivities);

        //    // Assert
        //    Assert.That(result, Is.True);
        //}


        //[Test]
        //public void GetBookedActivities_WhenAllQuantitiesAreNotTaken_ReturnsNoBookedActivities()
        //{
        //    // Arrange
        //    var expectToHaveNoActivities = true;

        //    // Act
        //    testActivityEntity.ItemQuantity = 3;
        //    SetupActivityGetByCondition();

        //    var activities = _activityService.GetBookedActivities(testActivityEntity);
        //    bool activitiesResult = activities.Count < 1;

        //    // Assert
        //    Assert.That(expectToHaveNoActivities, Is.EqualTo(activitiesResult));
        //}

        //[Test]
        //public void GetBookedActivities_WhenAllQuantitiesAreTaken_ReturnsBookedActivities()
        //{
        //    // Arrange
        //    SetupActivityGetByCondition();

        //    // Act
        //    var resultActivities = _activityService.GetBookedActivities(testActivityEntity);
        //    bool haveBookedActivities = resultActivities.Count > 0;

        //    // Assert
        //    Assert.That(haveBookedActivities, Is.True);
        //}


        //[Test]
        //public void CheckAvailability_InvalidActivityEntityPassed_ReturnsInvalidResultDetails()
        //{
        //    // Arrange
        //    ActivityEntity invalidActivityEntity = null;

        //    // Act
        //    var resultDetails = _activityService.CheckAvailability(invalidActivityEntity, DateTime.Now);

        //    // Assert
        //    Assert.That(resultDetails, Is.InstanceOf<ResultDetails>());
        //    Assert.That(resultDetails.Payload.IsValid, Is.False);
        //}

        //[Test]
        //public void CheckAvailability_InvalidDatePassed_ReturnsInvalidResultDetails()
        //{
        //    // Arrange
        //    var invalidDate = DateTime.Now.AddDays(-1);

        //    // Act
        //    var resultDetails = _activityService.CheckAvailability(testActivityEntity, invalidDate);

        //    // Assert
        //    Assert.That(resultDetails, Is.InstanceOf<ResultDetails>());
        //    Assert.That(resultDetails.Payload.IsValid, Is.False);
        //}

        //[Test]
        //public void CheckAvailability_ActivityEntityDoesNotExist_ReturnsInvalidResultDetails()
        //{
        //    // Arrange
        //    SetupActivityGetByCondition();
        //    SetupActivityRequestMap(testActivity, null);

        //    // Act
        //    var resultDetails = _activityService.CheckAvailability(testActivityEntity, DateTime.Now);

        //    // Assert
        //    Assert.That(resultDetails, Is.InstanceOf<ResultDetails>());
        //    Assert.That(resultDetails.Payload.IsValid, Is.False);
        //}

        //[Test]
        //public void CheckAvailability_WhenNoBookedActivities_ReturnsValidResultDetails()
        //{
        //    // Arrange
        //    testActivityEntity.ItemQuantity = 3;

        //    SetupActivityGetByCondition();
        //    SetupActivityRequestMap(testActivity, testNewActivityDTO);

        //    // Act
        //    var resultDetails = _activityService.CheckAvailability(testActivityEntity, DateTime.Now);

        //    // Assert
        //    Assert.That(resultDetails, Is.InstanceOf<ResultDetails>());
        //    Assert.That(resultDetails.Payload.IsValid, Is.True);
        //}

        //[Test]
        //public void CheckAvailability_WhenHaveBookedActivities_ReturnsBookedDetails()
        //{
        //    // Arrange
        //    SetupActivityGetByCondition();
        //    SetupActivityRequestMap(testActivity, testNewActivityDTO);
        //    SetupBookedActivityMap(testActivities, new List<BookedActivityDTO>());

        //    // Act
        //    var resultDetails = _activityService.CheckAvailability(testActivityEntity, DateTime.Now);

        //    // Assert
        //    Assert.That(resultDetails, Is.InstanceOf<ResultDetails>());
        //    Assert.That(resultDetails.Payload, Is.InstanceOf<IEnumerable<BookedActivityDTO>>());
        //}


        //[Test]
        //public void Delete_UnknownGuidPassed_ReturnsInvalidResultDetails()
        //{
        //    // Arrange
        //    var unknownGuid = new Guid();
        //    SetupGetActivityById(null);

        //    // Act
        //    var deleteResult = _activityService.Delete(unknownGuid);

        //    // Assert
        //    Assert.That(deleteResult.IsSuccessful, Is.False);
        //}

        //[Test]
        //public void Delete_ExistingGuidPassed_ReturnsCorrectResultDetails()
        //{
        //    // Arrange
        //    var testGuid = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");
        //    var activity = new Activity
        //    {
        //        Id = testGuid
        //    };

        //    SetupGetActivityById(activity);

        //    // Act
        //    var deleteResult = _activityService.Delete(testGuid);

        //    // Assert
        //    Assert.That(deleteResult.IsSuccessful, Is.True);
        //}


        //[Test]
        //public void ActivityEntityExists_UnknownNamePassed_ReturnsFalse()
        //{
        //    // Arrange
        //    var invalidName = "Non existing Name";
        //    SetupGetActivityEntityByName(null);

        //    // Act
        //    bool existsResult = _activityService.ActivityEntityExists(invalidName);

        //    // Assert
        //    Assert.That(existsResult, Is.False);
        //}

        //[Test]
        //public void ActivityEntityExists_ExistingNamePassed_ReturnsTrue()
        //{
        //    // Arrange
        //    var existingName = "Billiard";
        //    SetupGetActivityEntityByName(testActivityEntity);

        //    // Act
        //    bool existsResult = _activityService.ActivityEntityExists(existingName);

        //    // Assert
        //    Assert.That(existsResult, Is.True);
        //}


        //[Test]
        //public void Add_WhenCalled_CreatesNewActivity()
        //{
        //    // Arrange
        //    var activityRequestDTO = testNewActivityDTO;
        //    SetupActivityMap(testActivity, activityRequestDTO);
        //    SetupGetActivityEntityByName(testActivityEntity);
        //    SetupActivityCreate();

        //    // Act
        //    _activityService.Add(activityRequestDTO, testUser);

        //    // Assert
        //    _mockRepos.Verify(container => container.ActivityRepo.Create(testActivity), Times.Once);
        //}

        //[Test]
        //[Order(1)]
        //public void AddMany_WhenCreatingNewActivities_ExecutesSaveChangesOnce()
        //{
        //    // Arrange
        //    var activityRequestDTOs = new List<ActivityRequestDTO>()
        //    {
        //        testNewActivityDTO,
        //        testNewActivityDTO
        //    };

        //    SetupActivityMap(testActivity, testNewActivityDTO);
        //    SetupGetActivityEntityByName(testActivityEntity);
        //    SetupActivityCreate();

        //    // Act
        //    _activityService.AddMany(activityRequestDTOs, testUser);

        //    // Assert
        //    _mockRepos.Verify(container => container.ActivityRepo.Create(testActivity),
        //                                   Times.Exactly(activityRequestDTOs.Count));

        //    _mockRepos.Verify(container => container.SaveChanges(), Times.Once);
        //}

        //[Test]
        //public void Update_WhenCalled_ShouldUpdateActivity()
        //{
        //    // Arrange
        //    SetupGetActivityEntityByName(testActivityEntity);
        //    SetupActivityUpdate();

        //    // Act
        //    _activityService.Update(testActivity, testNewActivityDTO);

        //    // Assert
        //    _mockRepos.Verify(container => container.ActivityRepo.Update(testActivity), Times.Once);
        //}

        //[Test]
        //public void Update_InvalidItemPassed_ShouldThrowNullReferenceException()
        //{
        //    // Arrange
        //    SetupActivityUpdate();
        //    SetupGetActivityEntityByName(testActivityEntity);

        //    // Act
        //    Action updateWithInvalidActivity = () => _activityService.Update(null, testNewActivityDTO);
        //    Action updateWithInvalidActivityRequest = () => _activityService.Update(testActivity, null);
        //    Action updateWithInvalidParams = () => _activityService.Update(null, null);

        //    // Assert
        //    Assert.Throws<NullReferenceException>(() => updateWithInvalidActivity());
        //    Assert.Throws<NullReferenceException>(() => updateWithInvalidActivityRequest());
        //    Assert.Throws<NullReferenceException>(() => updateWithInvalidParams());
        //}

        #endregion



        #region Fluent Assert syntax

        [Test]
        public void GetActivityById_UnknownGuidPassed_ReturnsNoActivity()
        {
            // Arrange
            var unknownGuid = new Guid();
            SetupGetActivityById(null);

            // Act
            var expectedActivity = _activityService.GetActivityById(unknownGuid);

            // Assert
            expectedActivity.Should().Be(null);
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
            var result = _activityService.ScheduleActivity(null, testNewActivityDTO);

            // Assert
            expectedToSucceed.Should().Be(result.IsSuccessful);
        }

        [Test]
        public void ScheduleActivity_InvalidDatePassed_ReturnsUnsuccessfulResult()
        {
            // Arrange
            testNewActivityDTO.BookedForDate = testNewActivityDTO.BookedForDate.AddDays(-2);

            // Act
            var result = _activityService.ScheduleActivity(testUser, testNewActivityDTO);

            // Assert
            result.IsSuccessful.Should().Be(false);
        }

        [Test]
        public void ScheduleActivity_InvalidActivityEntityPassed_ReturnsUnsuccessfulResult()
        {
            // Arrange
            SetupGetActivityEntityByName(null);
            testNewActivityDTO.ActivityEntityId = null;

            // Act
            var result = _activityService.ScheduleActivity(testUser, testNewActivityDTO);

            // Assert
            result.IsSuccessful.Should().Be(false);
        }

        [Test]
        public void ScheduleActivity_WhenTimeSlotIsNotAvailable_ReturnsUnsuccessfulResult()
        {
            // Arrange
            SetupGetActivityEntityByName(testActivityEntity);
            SetupActivityGetByCondition();

            // Act
            var result = _activityService.ScheduleActivity(testUser, testNewActivityDTO);

            // Assert
            result.IsSuccessful.Should().Be(false);
        }

        [Test]
        public void ScheduleActivity_WhenThereAreNoBookedActivities_ReturnsSuccessfulResult()
        {
            // Arrange
            testActivityEntity.ItemQuantity = 3;

            SetupGetActivityEntityByName(testActivityEntity);
            SetupActivityGetByCondition();
            SetupActivityMap(testActivity, testNewActivityDTO);

            // Act
            var result = _activityService.ScheduleActivity(testUser, testNewActivityDTO);

            // Assert
            result.IsSuccessful.Should().Be(true);
        }


        [Test]
        public void IsScheduleTimeAvailable_WhenTimeSlotIsNotAvailable_ReturnsFalse()
        {
            // Arrange
            var expectedToSucceed = false;

            // Act
            var result = _activityService.IsScheduleTimeAvailable(testNewActivityDTO, testActivities);

            // Assert
            expectedToSucceed.Should().Be(result);
        }

        [Test]
        public void IsScheduleTimeAvailable_WhenTimeSlotIsAvailable_ReturnsTrue()
        {
            // Arrange
            foreach (var testActivity in testActivities)
            {
                testActivity.StartTime = testNewActivityDTO.EndTime;
                testActivity.StartTime = testActivity.StartTime.AddHours(2);

                testActivity.EndTime = testActivity.StartTime;
                testActivity.EndTime = testActivity.EndTime.AddHours(2);
            }

            // Act
            bool result = _activityService.IsScheduleTimeAvailable(testNewActivityDTO, testActivities);

            // Assert
            result.Should().Be(true);
        }


        [Test]
        public void GetBookedActivities_WhenAllQuantitiesAreNotTaken_ReturnsNoBookedActivities()
        {
            // Arrange
            var expectToHaveNoActivities = true;

            // Act
            testActivityEntity.ItemQuantity = 3;
            SetupActivityGetByCondition();

            var activities = _activityService.GetBookedActivities(testActivityEntity);
            bool activitiesResult = activities.Count < 1;

            // Assert
            expectToHaveNoActivities.Should().Be(activitiesResult);
        }

        [Test]
        public void GetBookedActivities_WhenAllQuantitiesAreTaken_ReturnsBookedActivities()
        {
            // Arrange
            SetupActivityGetByCondition();

            // Act
            var resultActivities = _activityService.GetBookedActivities(testActivityEntity);
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
            var resultDetails = _activityService.CheckAvailability(testActivityEntity, invalidDate);
            bool isValidResult = resultDetails.Payload.IsValid;

            // Assert
            resultDetails.Should().BeOfType(typeof(ResultDetails));
            isValidResult.Should().Be(false);
        }

        [Test]
        public void CheckAvailability_ActivityEntityDoesNotExist_ReturnsInvalidResultDetails()
        {
            // Arrange
            SetupActivityGetByCondition();
            SetupActivityRequestMap(testActivity, null);

            // Act
            var resultDetails = _activityService.CheckAvailability(testActivityEntity, DateTime.Now);
            bool isValidResult = resultDetails.Payload.IsValid;

            // Assert
            resultDetails.Should().BeOfType(typeof(ResultDetails));
            isValidResult.Should().Be(false);
        }

        [Test]
        public void CheckAvailability_WhenNoBookedActivities_ReturnsValidResultDetails()
        {
            // Arrange
            testActivityEntity.ItemQuantity = 3;

            SetupActivityGetByCondition();
            SetupActivityRequestMap(testActivity, testNewActivityDTO);

            // Act
            var resultDetails = _activityService.CheckAvailability(testActivityEntity, DateTime.Now);
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
            SetupActivityRequestMap(testActivity, testNewActivityDTO);
            SetupBookedActivityMap(testActivities, new List<BookedActivityDTO>());

            // Act
            var resultDetails = _activityService.CheckAvailability(testActivityEntity, DateTime.Now);

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
            SetupGetActivityEntityByName(testActivityEntity);

            // Act
            bool existsResult = _activityService.ActivityEntityExists(existingName);

            // Assert
            existsResult.Should().Be(true);
        }


        [Test]
        public void Add_WhenCalled_CreatesNewActivity()
        {
            // Arrange
            var activityRequestDTO = testNewActivityDTO;
            SetupActivityMap(testActivity, activityRequestDTO);
            SetupGetActivityEntityByName(testActivityEntity);
            SetupActivityCreate();

            // Act
            _activityService.Add(activityRequestDTO, testUser);

            // Assert
            _mockRepos.Verify(container => container.ActivityRepo.Create(testActivity), Times.Once);
        }

        [Test]
        [Order(1)]
        public void AddMany_WhenCreatingNewActivities_ExecutesSaveChangesOnce()
        {
            // Arrange
            var activityRequestDTOs = new List<ActivityRequestDTO>()
            {
                testNewActivityDTO,
                testNewActivityDTO
            };

            SetupActivityMap(testActivity, testNewActivityDTO);
            SetupGetActivityEntityByName(testActivityEntity);
            SetupActivityCreate();

            // Act
            _activityService.AddMany(activityRequestDTOs, testUser);

            // Assert
            _mockRepos.Verify(container => container.ActivityRepo.Create(testActivity),
                                           Times.Exactly(activityRequestDTOs.Count));

            _mockRepos.Verify(container => container.SaveChanges(), Times.Once);
        }

        [Test]
        public void Update_WhenCalled_ShouldUpdateActivity()
        {
            // Arrange
            SetupGetActivityEntityByName(testActivityEntity);
            SetupActivityUpdate();

            // Act
            _activityService.Update(testActivity, testNewActivityDTO);

            // Assert
            _mockRepos.Verify(container => container.ActivityRepo.Update(testActivity), Times.Once);
        }

        [Test]
        public void Update_InvalidItemPassed_ShouldThrowNullReferenceException()
        {
            // Arrange
            SetupActivityUpdate();
            SetupGetActivityEntityByName(testActivityEntity);

            // Act
            Action updateWithInvalidActivity = () => _activityService.Update(null, testNewActivityDTO);
            Action updateWithInvalidActivityRequest = () => _activityService.Update(testActivity, null);
            Action updateWithInvalidParams = () => _activityService.Update(null, null);

            // Assert
            updateWithInvalidActivity.Should().Throw<NullReferenceException>();
            updateWithInvalidActivityRequest.Should().Throw<NullReferenceException>();
            updateWithInvalidParams.Should().Throw<NullReferenceException>();
        }
        #endregion














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
                      .Returns(testActivities.AsQueryable());
        }


        private void SetupActivityMap(Activity activity, ActivityRequestDTO activityDTO)
        {
            _mockMapper.Setup(mapper => mapper.Map<Activity>(activityDTO))
                .Returns(activity);
        }

        private void SetupActivityRequestMap(Activity activity, ActivityRequestDTO activityDTO)
        {
            _mockMapper.Setup(mapper => mapper.Map<ActivityRequestDTO>(activity))
                .Returns(activityDTO);
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

        private void InitializeEntities()
        {
            testUser = new User
            {
                Id = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c200",
                UserName = "Bob",
                Email = "bob@gmail.com",
                IsAdmin = false,
                LastName = "Bobsky"
            };

            testNewActivityDTO = new ActivityRequestDTO
            {
                ActivityEntityId = "ab2bd817-98cd-4cf3-a80a-53ea0cd9c201",
                BookedForDate = DateTime.Now.AddDays(2),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2),
                ActivityEntityName = "Billiard"
            };

            testActivityEntity = new ActivityEntity
            {
                Name = "Billiard",
                Id = new Guid(testNewActivityDTO.ActivityEntityId),
                Description = "Test description",
                ImageUrl = "Test url",
                ItemQuantity = 2,
                Location = "Test Location",
                MaxUserCount = 3,
                MinUserCount = 2
            };

            testActivity = new Activity
            {
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c202"),
                ActivityEntityId = testActivityEntity.Id.ToString(),
                BookedForDate = testNewActivityDTO.BookedForDate,
                DateBooked = DateTime.Now,
                Duration = "Test duration",
                EndTime = testNewActivityDTO.EndTime,
                Name = "Billiard",
                OrganizerName = testUser.UserName,
                StartTime = testNewActivityDTO.StartTime,
                UserId = testUser.Id
            };

            testActivities = new List<Activity>
            {
                testActivity,
                testActivity
            };
        }
    }
}
