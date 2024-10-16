using AutoMapper;
using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;
using BasicCleanArchitectureTemplate.Tests.Factories;
using BasicCleanArchitectureTemplate.Web.Controllers;
using BasicCleanArchitectureTemplate.Web.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BasicCleanArchitectureTemplate.Tests.Controllers
{
    [TestFixture]
    public class EventsControllerTests
    {
        private Mock<IEventService> _mockEventService;
        private Mock<IMapper> _mockMapper;
        private EventsController _eventsController;

        [SetUp]
        public void SetUp()
        {
            _mockEventService = new Mock<IEventService>();
            _mockMapper = new Mock<IMapper>();
            _eventsController = new EventsController(_mockEventService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetCalendarEvents_ShouldGetAllEventsAndReturnCorrectViewModels()
        {
            // Arrange
            var filterRequest = new EventFilterRequest();
            var testEventModelList = EventModelFactory.CreateTestEventOccurrenceModelList();

            _mockEventService.Setup(service => service.GetEventOccurrences(filterRequest)).ReturnsAsync(testEventModelList);

            // Act
            var result = await _eventsController.GetCalendarEvents(filterRequest) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<JsonResult>(result);
            var resultData = result.Value as IEnumerable<EventOccurrenceModel>;
            Assert.AreEqual(testEventModelList.Count(), resultData.Count());

            _mockEventService.Verify(service => service.GetEventOccurrences(filterRequest), Times.Once);
        }

        [Test]
        public async Task Details_ShouldThrowValidationExceptionForInvalidEventId()
        {
            //Arrange
            MockValidationException();

            //Assert
            var exception = Assert.ThrowsAsync<ValidationException>(() =>
                _eventsController.Details(new EventIdRequest { Id = Guid.Empty }));
            Assert.That(exception.Message, Is.EqualTo("Validation failed."));
        }

        [Test]
        public async Task Details_ShouldReturnNotFoundForNonExistentEventId()
        {
            // Arrange
            Guid nonExistentEventId = Guid.NewGuid();
            var eventIdRequest = new EventIdRequest() { Id = nonExistentEventId };
            _mockEventService.Setup(service => service.GetEventById(eventIdRequest)).ReturnsAsync((EventModel)null);

            // Act
            var result = await _eventsController.Details(eventIdRequest) as NotFoundResult;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Details_ShouldGetEventAndMapToViewModel()
        {
            //Arrange
            var eventId = Guid.NewGuid();
            var eventIdRequest = new EventIdRequest { Id = eventId };

            var testEventModel = EventModelFactory.CreateTestEventModel(eventId);
            var testEventViewModel = EventModelFactory.CreateTestEventViewModel(eventId);
            _mockEventService.Setup(service => service.GetEventById(eventIdRequest)).ReturnsAsync(testEventModel);
            _mockMapper.Setup(mapper => mapper.Map<EventViewModel>(testEventModel)).Returns(testEventViewModel);

            //Act
            var result = await _eventsController.Details(eventIdRequest) as ViewResult;


            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testEventViewModel, result.Model);

            _mockEventService.Verify(service => service.GetEventById(eventIdRequest), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<EventViewModel>(testEventModel), Times.Once);
        }

        [Test]
        public async Task Create_Post_ShouldAddEventAndRedirectToIndex()
        {
            //Arrange
            var testEventModel = EventModelFactory.CreateTestEventModel(Guid.NewGuid());
            var testEventViewModel = EventModelFactory.CreateTestEventViewModel(Guid.NewGuid());
            _mockMapper.Setup(mapper => mapper.Map<EventViewModel, EventModel>(testEventViewModel)).Returns(testEventModel);

            //Act
            var result = await _eventsController.Create(testEventViewModel) as RedirectToActionResult;


            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            _mockEventService.Verify(service => service.AddEvent(testEventModel), Times.Once);
        }

        [Test]
        public async Task Edit_Get_ShouldReturnNotFoundForNullId()
        {
            //Arrange
            MockValidationException();

            //Assert
            var exception = Assert.ThrowsAsync<ValidationException>(() =>
                _eventsController.Edit(new EventIdRequest { Id = Guid.Empty }));
            Assert.That(exception.Message, Is.EqualTo("Validation failed."));
        }

        [Test]
        public async Task Edit_Get_ShouldGetEventAndMapToViewModel()
        {
            //Arrange
            var eventId = Guid.NewGuid();
            var eventIdRequest = new EventIdRequest { Id = eventId };
            var testEventModel = EventModelFactory.CreateTestEventModel(eventId);
            var testEventViewModel = EventModelFactory.CreateTestEventViewModel(eventId);
            _mockEventService.Setup(service => service.GetEventById(eventIdRequest))
                .ReturnsAsync(testEventModel);
            _mockMapper.Setup(mapper => mapper.Map<EventModel, EventViewModel>(testEventModel)).Returns(testEventViewModel);

            //Act
            var result = await _eventsController.Edit(eventIdRequest) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testEventViewModel, result.Model);

            _mockEventService.Verify(service => service.GetEventById(eventIdRequest), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<EventModel, EventViewModel>(testEventModel), Times.Once);
        }

        [Test]
        public async Task Edit_Post_ShouldUpdateEventAndRedirectToIndex()
        {
            //Arrange
            var testEventModel = EventModelFactory.CreateTestEventModel(Guid.NewGuid());
            var testEventViewModel = EventModelFactory.CreateTestEventViewModel(Guid.NewGuid());
            _mockMapper.Setup(mapper => mapper.Map<EventViewModel, EventModel>(testEventViewModel)).Returns(testEventModel);

            //Act
            var eventId = Guid.NewGuid();
            var result = await _eventsController.Edit(eventId, testEventViewModel) as RedirectToActionResult;


            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            _mockEventService.Verify(service => service.UpdateEvent(testEventModel), Times.Once);
        }

        [Test]
        public async Task Delete_Get_ShouldThrowValidationExceptionForInvalidEventId()
        {
            //Arrange
            MockValidationException();

            //Assert
            var exception = Assert.ThrowsAsync<ValidationException>(() =>
                _eventsController.Delete(new EventIdRequest { Id = Guid.Empty }));
            Assert.That(exception.Message, Is.EqualTo("Validation failed."));
        }

        [Test]
        public async Task Delete_Get_ShouldGetEventAndMapToViewModel()
        {
            //Arrange
            var eventId = Guid.NewGuid();
            var eventIdRequest = new EventIdRequest { Id = eventId };
            var testEventModel = EventModelFactory.CreateTestEventModel(eventId);
            var testEventViewModel = EventModelFactory.CreateTestEventViewModel(eventId);
            
            _mockEventService.Setup(service => service.GetEventById(eventIdRequest)).ReturnsAsync(testEventModel);
            _mockMapper.Setup(mapper => mapper.Map<EventModel, EventViewModel>(testEventModel)).Returns(testEventViewModel);

            //Act
            var result = await _eventsController.Delete(eventIdRequest) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testEventViewModel, result.Model);

            _mockEventService.Verify(service => service.GetEventById(eventIdRequest), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<EventModel, EventViewModel>(testEventModel), Times.Once);
        }

        [Test]
        public async Task DeleteConfirmed_ShouldDeleteEventAndRedirectToIndex()
        {
            //Act
            var eventId = Guid.NewGuid();
            var eventIdRequest = new EventIdRequest { Id = eventId };
            var result = await _eventsController.DeleteConfirmed(eventIdRequest) as RedirectToActionResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);

            _mockEventService.Verify(service => service.DeleteEvent(eventIdRequest), Times.Once);
        }

        private void MockValidationException()
        {
            //Arrange
            _mockEventService.Setup(service => service.GetEventById(It.IsAny<EventIdRequest>()))
                .Throws(new ValidationException("Validation failed."));
        }
    }
}
