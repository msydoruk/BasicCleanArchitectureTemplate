using AutoMapper;
using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EventViewModel = BasicCleanArchitectureTemplate.Web.ViewModels.EventViewModel;

namespace BasicCleanArchitectureTemplate.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventsController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCalendarEvents([FromQuery] EventFilterRequest filterRequest)
        {
            var eventOccurrenceModels = await _eventService.GetEventOccurrences(filterRequest);

            return Json(eventOccurrenceModels);
        }

        public async Task<IActionResult> Details(EventIdRequest eventIdRequest)
        {
            var @event = await _eventService.GetEventById(eventIdRequest);
            var eventViewModel = _mapper.Map<EventViewModel>(@event);

            return View(eventViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {
            if (!eventViewModel.HasRecurrence)
            {
                HandleRecurrenceValidation(ModelState, eventViewModel);
            }

            if (ModelState.IsValid)
            {
                var @event = _mapper.Map<EventViewModel, EventModel>(eventViewModel);
                await _eventService.AddEvent(@event);

                return RedirectToAction(nameof(Index));
            }

            return View(eventViewModel);
        }

        public async Task<IActionResult> Edit(EventIdRequest eventIdRequest)
        {
            var @event = await _eventService.GetEventById(eventIdRequest);
            var eventViewModel = _mapper.Map<EventModel, EventViewModel>(@event);
            eventViewModel.HasRecurrence = eventViewModel.RecurrenceSetting != null;

            return View(eventViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EventViewModel eventViewModel)
        {
            if (!eventViewModel.HasRecurrence)
            {
                HandleRecurrenceValidation(ModelState, eventViewModel);
            }

            if (ModelState.IsValid)
            {
                var @event = _mapper.Map<EventViewModel, EventModel>(eventViewModel);
                await _eventService.UpdateEvent(@event);

                return RedirectToAction(nameof(Index));
            }

            return View(eventViewModel);
        }

        public async Task<IActionResult> Delete(EventIdRequest eventIdRequest)
        {
            var @event = await _eventService.GetEventById(eventIdRequest);
            var eventViewModel = _mapper.Map<EventModel, EventViewModel>(@event);

            return View(eventViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(EventIdRequest eventIdRequest)
        {
            await _eventService.DeleteEvent(eventIdRequest);

            return RedirectToAction(nameof(Index));
        }

        private void HandleRecurrenceValidation(ModelStateDictionary modelState, EventViewModel eventViewModel)
        {
            eventViewModel.RecurrenceSetting = null;
            modelState.Remove("RecurrenceSetting.Type");
            modelState.Remove("RecurrenceSetting.Interval");
            modelState.Remove("RecurrenceSetting.OccurrencePosition");
            modelState.Remove("RecurrenceSetting.PeriodType");
            modelState.Remove("RecurrenceSetting.DayOfWeeks");
            modelState.Remove("RecurrenceSetting.EndDate");
        }
    }
}
