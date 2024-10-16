using BasicCleanArchitectureTemplate.Core.Models;
using BasicCleanArchitectureTemplate.Infrastructure.Data;
using BasicCleanArchitectureTemplate.Infrastructure.Repositories;
using BasicCleanArchitectureTemplate.Tests.Base;
using BasicCleanArchitectureTemplate.Tests.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace BasicCleanArchitectureTemplate.Tests.Repositories
{
    [TestFixture]
    public class EventRepositoryTests : TestBase
    {
        private ApplicationDbContext _applicationDbContext;
        private EventRepository _eventRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryEventDatabase")
                .Options;

            _applicationDbContext = new ApplicationDbContext(options);

            _eventRepository = new EventRepository(_applicationDbContext, Mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _applicationDbContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task Add_ShouldAddAndReturnEvent()
        {
            //Arrange
            var eventModel = EventModelFactory.CreateTestEventModel(Guid.NewGuid());

            //Act
            var result = await _eventRepository.Add(eventModel);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<EventModel>(result);
            Assert.IsTrue(_applicationDbContext.Events.Any());
        }

        [Test]
        public async Task GetFilteredEvents_WithSpecificRecurrenceType_ShouldReturnEventsMatchingRecurrence()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-5);
            var endDate = DateTime.UtcNow.AddDays(5);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var eventId = Guid.NewGuid();
            var eventModel = EventModelFactory.CreateTestEventModel(
                id: eventId,
                startDate: startDate.AddDays(-10),
                recurrenceEndDate: endDate.AddDays(1),
                recurrenceType: RecurrenceType.Weekly,
                recurrenceInterval: 2);

            await _eventRepository.Add(eventModel);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<EventModel>>(result);
            var recurrenceEvent = result.FirstOrDefault(e => e.Id == eventId);
            Assert.IsNotNull(recurrenceEvent);
            Assert.AreEqual(RecurrenceType.Weekly, recurrenceEvent.RecurrenceSetting.Type);
            Assert.AreEqual(2, recurrenceEvent.RecurrenceSetting.Interval);
        }

        [Test]
        public async Task GetFilteredEvents_ByDates_ShouldReturnEventsWithinDateRange()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(-5);
            var endDate = DateTime.UtcNow.AddDays(5);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var dailyEvent = EventModelFactory.CreateTestEventModel(
                id: Guid.NewGuid(),
                startDate: startDate.AddDays(-3),
                recurrenceEndDate: endDate.AddDays(-2),
                recurrenceType: RecurrenceType.Daily);

            var weeklyEvent = EventModelFactory.CreateTestEventModel(
                id: Guid.NewGuid(),
                startDate: startDate.AddDays(-4),
                recurrenceEndDate: endDate.AddDays(-3),
                recurrenceType: RecurrenceType.Weekly);

            await _eventRepository.Add(dailyEvent);
            await _eventRepository.Add(weeklyEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetFilteredEvents_DailyRecurrence_ShouldReturnEventsMatchingDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(10);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var eventId = Guid.NewGuid();
            var dailyEvent = EventModelFactory.CreateTestEventModel(
                id: eventId,
                startDate: startDate.AddDays(-1), // 1 day before start date
                recurrenceEndDate: endDate.AddDays(5), // 5 days after end date
                recurrenceType: RecurrenceType.Daily);

            await _eventRepository.Add(dailyEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetFilteredEvents_WeeklyRecurrence_ShouldReturnEventsMatchingDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(28); // 4 weeks
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var weeklyEvent = EventModelFactory.CreateTestEventModel(
                id: Guid.NewGuid(),
                startDate: startDate,
                recurrenceEndDate: endDate.AddDays(10),
                recurrenceType: RecurrenceType.Weekly);

            await _eventRepository.Add(weeklyEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetFilteredEvents_MonthlyRecurrence_ShouldReturnEventsMatchingDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddMonths(3);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var monthlyEvent = EventModelFactory.CreateTestEventModel(
                id: Guid.NewGuid(),
                startDate: startDate,
                recurrenceEndDate: endDate.AddMonths(2),
                recurrenceType: RecurrenceType.Monthly);

            await _eventRepository.Add(monthlyEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetFilteredEvents_YearlyRecurrence_ShouldReturnEventsMatchingDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddYears(3);
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var yearlyEvent = EventModelFactory.CreateTestEventModel(
                id: Guid.NewGuid(),
                startDate: startDate,
                recurrenceEndDate: endDate.AddYears(2),
                recurrenceType: RecurrenceType.Yearly);

            await _eventRepository.Add(yearlyEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetFilteredEvents_DailyRecurrenceWithIntervalOfTen_ShouldReturnEventsMatchingDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(50);  // cover 50 days
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var dailyEvent = EventModelFactory.CreateTestEventModel(
                id: Guid.NewGuid(),
                startDate: startDate.AddDays(-5), // 5 days before start date
                recurrenceEndDate: endDate.AddDays(15), // 15 days after end date
                recurrenceType: RecurrenceType.Daily,
                recurrenceInterval: 10);

            await _eventRepository.Add(dailyEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetFilteredEvents_WeeklyRecurrenceWithIntervalOfTwo_ShouldReturnEventsMatchingDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(30);  // cover 4 weeks
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var weeklyEvent = EventModelFactory.CreateTestEventModel(
                id: Guid.NewGuid(),
                startDate: startDate.AddDays(-7), // 1 week before start date
                recurrenceEndDate: endDate.AddDays(14), // 2 weeks after end date
                recurrenceType: RecurrenceType.Weekly,
                recurrenceInterval: 2);

            await _eventRepository.Add(weeklyEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetFilteredEvents_MonthlyRecurrenceWithIntervalOfThree_ShouldReturnEventsMatchingDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddMonths(12);  // cover a year
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var monthlyEvent = EventModelFactory.CreateTestEventModel(
                id: Guid.NewGuid(),
                startDate: startDate.AddMonths(-2), // 2 months before start date
                recurrenceEndDate: endDate.AddMonths(4), // 4 months after end date
                recurrenceType: RecurrenceType.Monthly,
                recurrenceInterval: 3);

            await _eventRepository.Add(monthlyEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetFilteredEvents_YearlyRecurrenceWithIntervalOfTwo_ShouldReturnEventsMatchingDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddYears(6);  // cover 6 years
            var filterRequest = new EventFilterRequest { StartDate = startDate, EndDate = endDate };

            var yearlyEvent = EventModelFactory.CreateTestEventModel(
                id: Guid.NewGuid(),
                startDate: startDate.AddYears(-1), // 1 year before start date
                recurrenceEndDate: endDate.AddYears(3), // 3 years after end date
                recurrenceType: RecurrenceType.Yearly,
                recurrenceInterval: 2);

            await _eventRepository.Add(yearlyEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetFilteredEvents_ByCategoryAndDates_ShouldReturnEventsMatchingCategoryAndDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(10);
            var filterRequest = new EventFilterRequest { Category = "Test Category", StartDate = startDate, EndDate = endDate };

            var eventId = Guid.NewGuid();
            var matchingEvent = EventModelFactory.CreateTestEventModel(id: eventId, startDate: startDate.AddDays(1));
            var nonMatchingDateEvent = EventModelFactory.CreateTestEventModel(id: Guid.NewGuid(), startDate: startDate.AddDays(-11)); // out of date range
            var nonMatchingCategoryEvent = EventModelFactory.CreateTestEventModel(id: Guid.NewGuid(), name: "Another Event", startDate: startDate.AddDays(1));
            nonMatchingCategoryEvent.Category = "Another Category";

            await _eventRepository.Add(matchingEvent);
            await _eventRepository.Add(nonMatchingDateEvent);
            await _eventRepository.Add(nonMatchingCategoryEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(e => e.Id == eventId));
        }

        [Test]
        public async Task GetFilteredEvents_ByPlaceAndDates_ShouldReturnEventsMatchingPlaceAndDates()
        {
            // Arrange
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(10);
            var filterRequest = new EventFilterRequest { Place = "Test Place", StartDate = startDate, EndDate = endDate };

            var eventId = Guid.NewGuid();
            var matchingEvent = EventModelFactory.CreateTestEventModel(id: eventId, startDate: startDate.AddDays(1));
            var nonMatchingDateEvent = EventModelFactory.CreateTestEventModel(id: Guid.NewGuid(), startDate: startDate.AddDays(-11)); // out of date range
            var nonMatchingPlaceEvent = EventModelFactory.CreateTestEventModel(id: Guid.NewGuid(), name: "Different Event", startDate: startDate.AddDays(1));
            nonMatchingPlaceEvent.Place = "Another Place";

            await _eventRepository.Add(matchingEvent);
            await _eventRepository.Add(nonMatchingDateEvent);
            await _eventRepository.Add(nonMatchingPlaceEvent);

            // Act
            var result = await _eventRepository.GetFilteredEvents(filterRequest);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Any(e => e.Id == eventId));
        }

        [Test]
        public async Task Get_ShouldReturnEventById()
        {
            //Arrange
            var eventModel = EventModelFactory.CreateTestEventModel(Guid.NewGuid());
            var addedEvent = await _eventRepository.Add(eventModel);

            //Act
            var result = await _eventRepository.Get(addedEvent.Id);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<EventModel>(result);
            Assert.IsTrue(result.Name == eventModel.Name);
        }

        [Test]
        public async Task Update_ShouldUpdateAndReturnTrue()
        {
            //Arrange
            var eventModel = EventModelFactory.CreateTestEventModel(Guid.NewGuid());
            var addedEvent = await _eventRepository.Add(eventModel);

            //Act
            addedEvent.Name = "Updated";
            var isUpdated = await _eventRepository.Update(addedEvent);

            //Assert
            Assert.IsTrue(isUpdated);
        }

        [Test]
        public async Task Delete_ShouldDeleteAndReturnTrue()
        {
            //Arrange
            var eventModel = EventModelFactory.CreateTestEventModel(Guid.NewGuid());
            var addedEvent = await _eventRepository.Add(eventModel);

            //Act
            var isDeleted = await _eventRepository.Delete(addedEvent.Id);

            //Assert
            Assert.IsTrue(isDeleted);
        }

        [Test]
        public async Task Delete_ShouldReturnFalseIfEventNotFound()
        {
            //Act
            var eventId = Guid.NewGuid();
            var isDeleted = await _eventRepository.Delete(eventId);

            //Assert
            Assert.IsFalse(isDeleted);
        }
    }
}
