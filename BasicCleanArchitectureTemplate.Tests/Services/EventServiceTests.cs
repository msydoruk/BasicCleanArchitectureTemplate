using BasicCleanArchitectureTemplate.Core.Exceptions;
using BasicCleanArchitectureTemplate.Core.FluentValidation;
using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;
using BasicCleanArchitectureTemplate.Core.Services;
using BasicCleanArchitectureTemplate.Tests.Base;
using BasicCleanArchitectureTemplate.Tests.Factories;
using Moq;
using NUnit.Framework;
using BasicCleanArchitectureTemplate.Core.Factories;

namespace BasicCleanArchitectureTemplate.Tests.Services
{
    [TestFixture]
    public class EventServiceTests : TestBase
    {
        private Mock<IEventRepository> _mockEventRepository;
        private Mock<IValidatorServiceFactory> _mockValidatorServiceFactory;
        private EventService _eventService;
        private ValidationService _validationService;
        private IEventOccurrenceStrategyFactory _eventOccurrenceStrategyFactory;

        [SetUp]
        public void SetUp()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _mockValidatorServiceFactory = new Mock<IValidatorServiceFactory>();
            RegisterCustomValidators(_mockValidatorServiceFactory);
            _validationService = new ValidationService(_mockValidatorServiceFactory.Object);
            _eventOccurrenceStrategyFactory = new EventOccurrenceStrategyFactory();
            _eventService = new EventService(_mockEventRepository.Object, _eventOccurrenceStrategyFactory, _validationService );
        }

        [Test]
        public async Task AddEvent_ShouldCallAddOnRepository()
        {
            //Arrange
            var eventModel = EventModelFactory.CreateTestEventModel(Guid.NewGuid());

            //Act
            await _eventService.AddEvent(eventModel);

            //Assert
            _mockEventRepository.Verify(repository => repository.Add(eventModel), Times.Once);
        }

        [Test]
        public async Task GetEventOccurrences_WithNoRecurrence_ShouldReturnAllEvents()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-1);
            var endDate = startDate.AddDays(10);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var eventModelList = EventModelFactory.CreateTestEventModelList(5, startDate: startDate);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public async Task GetEventOccurrences_WithDailyRecurrence_ShouldReturnMultipleOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-1);
            var endDate = startDate.AddDays(10);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Daily,
                Interval = 1,
                EndDate = endDate
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.Count()); // original + 10 daily occurrences
        }

        [Test]
        public async Task GetEventOccurrences_WithWeeklyRecurrence_ShouldReturnMultipleOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-1);
            var endDate = startDate.AddDays(30); // 4 weeks
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Weekly,
                Interval = 1,
                EndDate = endDate
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count()); // original + 4 weekly occurrences
        }

        [Test]
        public async Task GetEventOccurrences_WithMonthlyRecurrence_ShouldReturnMultipleOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddMonths(-1);
            var endDate = startDate.AddMonths(3);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Monthly,
                Interval = 1,
                EndDate = endDate
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count()); // original + 3 monthly occurrences
        }

        [Test]
        public async Task GetEventOccurrences_WithYearlyRecurrence_ShouldReturnMultipleOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddYears(-1);
            var endDate = startDate.AddYears(3);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Yearly,
                Interval = 1,
                EndDate = endDate
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count()); // original + 3 yearly occurrences
        }

        [Test]
        public async Task GetEventOccurrences_WithCustomWeeklyRecurrence_ShouldReturnMultipleOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-7);
            var endDate = startDate.AddMonths(5);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Custom,
                PeriodType = PeriodType.Week,
                Interval = 1,
                EndDate = endDate,
                DayOfWeeks = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday }
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(44, result.Count());
        }

        [Test]
        public async Task GetEventOccurrences_WithCustomMonthlyRecurrenceFirst_ShouldReturnCorrectOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = startDate.AddMonths(6);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Custom,
                PeriodType = PeriodType.Month,
                Interval = 1,
                EndDate = endDate,
                OccurrencePosition = PositionIdPeriod.First,
                DayOfWeeks = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday }
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(14, result.Count());
        }

        [Test]
        public async Task GetEventOccurrences_WithCustomMonthlyRecurrenceLast_ShouldReturnCorrectOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = startDate.AddMonths(6);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Custom,
                PeriodType = PeriodType.Month,
                Interval = 1,
                EndDate = endDate,
                OccurrencePosition = PositionIdPeriod.Last,
                DayOfWeeks = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday }
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(14, result.Count());
        }

        [Test]
        public async Task GetEventOccurrences_DailyWithIntervalOfTen_ShouldReturnCorrectOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-1);
            var endDate = startDate.AddDays(50);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Daily,
                Interval = 10,
                EndDate = endDate
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count()); // original + 5 occurrences each in 10 days
        }

        [Test]
        public async Task GetEventOccurrences_WeeklyWithIntervalOfTwo_ShouldReturnCorrectOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-1);
            var endDate = startDate.AddDays(35);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Weekly,
                Interval = 2,
                EndDate = endDate
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count()); // original + 2 occurrences each in 2 weeks
        }

        [Test]
        public async Task GetEventOccurrences_MonthlyWithIntervalOfThree_ShouldReturnCorrectOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddMonths(-1);
            var endDate = startDate.AddMonths(12);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Monthly,
                Interval = 3,
                EndDate = endDate
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count()); // original + 4 occurrences each in 3 months
        }

        [Test]
        public async Task GetEventOccurrences_YearlyWithIntervalOfTwo_ShouldReturnCorrectOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddYears(-1);
            var endDate = startDate.AddYears(10);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Yearly,
                Interval = 2,
                EndDate = endDate
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Count()); // original + 5 occurrences each in 2 years
        }

        [Test]
        public async Task GetEventOccurrences_CustomWeeklyWithIntervalOfThree_ShouldReturnCorrectOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-7);
            var endDate = startDate.AddMonths(5);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Custom,
                PeriodType = PeriodType.Week,
                Interval = 3,
                EndDate = endDate,
                DayOfWeeks = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday }
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(16, result.Count());
        }

        [Test]
        public async Task GetEventOccurrences_CustomMonthlyWithIntervalOfFourFirst_ShouldReturnCorrectOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = startDate.AddMonths(12);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Custom,
                PeriodType = PeriodType.Month,
                Interval = 4,
                EndDate = endDate,
                OccurrencePosition = PositionIdPeriod.First,
                DayOfWeeks = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday }
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Count());
        }

        [Test]
        public async Task GetEventOccurrences_CustomMonthlyWithIntervalOfFourLast_ShouldReturnCorrectOccurrences()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = startDate.AddMonths(12);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };
            var recurrenceSetting = new RecurrenceSettingModel
            {
                Type = RecurrenceType.Custom,
                PeriodType = PeriodType.Month,
                Interval = 4,
                EndDate = endDate,
                OccurrencePosition = PositionIdPeriod.Last,
                DayOfWeeks = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday }
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(1, startDate: startDate, recurrenceSetting: recurrenceSetting);

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(eventModelList);

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Count());
        }

        [Test]
        public async Task GetEventOccurrences_ByCategory_ShouldReturnFilteredEvents()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-1);
            var endDate = startDate.AddDays(10);
            var filterRequest = new EventFilterRequest
            {
                StartDate = startDate,
                EndDate = endDate,
                Category = "Test Category 1"
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(5, startDate: startDate);
            eventModelList[0].Category = "Test Category 1";

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(new List<EventModel> { eventModelList[0] });

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Test Category 1", result.First().EventInstance.Category);
        }

        [Test]
        public async Task GetEventOccurrences_ByPlace_ShouldReturnFilteredEvents()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-1);
            var endDate = startDate.AddDays(10);
            var filterRequest = new EventFilterRequest
            {
                StartDate = startDate,
                EndDate = endDate,
                Place = "Test Place 1"
            };
            var eventModelList = EventModelFactory.CreateTestEventModelList(5, startDate: startDate);
            eventModelList[0].Place = "Test Place 1";

            _mockEventRepository.Setup(repository => repository.GetFilteredEvents(filterRequest)).ReturnsAsync(new List<EventModel> { eventModelList[0] });

            // Act
            var result = await _eventService.GetEventOccurrences(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Test Place 1", result.First().EventInstance.Place);
        }

        [Test]
        public async Task GetEvent_ShouldReturnEventById()
        {
            //Arrange
            var eventModel = EventModelFactory.CreateTestEventModel(Guid.NewGuid());
            _mockEventRepository.Setup(repository => repository.Get(It.IsAny<Guid>())).ReturnsAsync(eventModel);

            //Act
            var result = await _eventService.GetEventById(new EventIdRequest { Id = Guid.NewGuid() });

            //Assert
            Assert.AreEqual(eventModel, result);
        }

        [Test]
        public async Task GetEvent_ShouldThrowExceptionForNonExistingEvent()
        {
            //Arrange
            _mockEventRepository.Setup(repository => repository.Get(It.IsAny<Guid>())).ReturnsAsync((EventModel)null);

            //Assert
            Assert.ThrowsAsync<EntityNotFoundException>(() => _eventService.GetEventById(new EventIdRequest { Id = Guid.NewGuid() }));
        }

        [Test]
        public async Task Update_ShouldCallUpdateOnRepository()
        {
            //Arrange
            var eventModel = EventModelFactory.CreateTestEventModel(Guid.NewGuid());

            //Act
            await _eventService.UpdateEvent(eventModel);

            //Assert
            _mockEventRepository.Verify(repository => repository.Update(eventModel), Times.Once);
        }

        [Test]
        public async Task Delete_ShouldCallDeleteOnRepository()
        {
            //Arrange
            var eventId = Guid.NewGuid();

            //Act
            await _eventService.DeleteEvent(new EventIdRequest { Id = eventId });

            //Assert
            _mockEventRepository.Verify(repository => repository.Delete(eventId), Times.Once);
        }

        private void RegisterCustomValidators(Mock<IValidatorServiceFactory> mockValidatorServiceFactory)
        {
            mockValidatorServiceFactory.Setup(validator => validator.GetValidator<EventFilterRequest>())
                .Returns(new EventFilterRequestValidator());

            mockValidatorServiceFactory.Setup(validator => validator.GetValidator<EventIdRequest>())
                .Returns(new EventIdRequestValidator());

            mockValidatorServiceFactory.Setup(validator => validator.GetValidator<EventModel>())
                .Returns(new EventModelValidator());
        }
    }
}
