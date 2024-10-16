using BasicCleanArchitectureTemplate.Core.Exceptions;
using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventOccurrenceStrategyFactory _eventOccurrenceStrategyFactory;
        private readonly IValidationService _validationService;

        public EventService(
            IEventRepository eventRepository,
            IEventOccurrenceStrategyFactory eventOccurrenceStrategyFactory,
            IValidationService validationService)
        {
            _eventRepository = eventRepository;
            _eventOccurrenceStrategyFactory = eventOccurrenceStrategyFactory;
            _validationService = validationService;
        }

        public async Task<EventModel> AddEvent(EventModel eventItem)
        {
            await _validationService.ValidateModel(eventItem);

            return await _eventRepository.Add(eventItem);
        }

        public async Task<IEnumerable<EventOccurrenceModel>> GetEventOccurrences(EventFilterRequest filterRequest)
        {
            await _validationService.ValidateModel(filterRequest);

            IEnumerable<EventModel> filteredEvents = await _eventRepository.GetFilteredEvents(filterRequest);

            return filteredEvents.SelectMany(eventModel =>
            {
                var eventOccurrenceStrategy = _eventOccurrenceStrategyFactory.GetStrategy(eventModel.RecurrenceSetting);
                return eventOccurrenceStrategy.GetEventOccurrences(eventModel, filterRequest);
            });
        }

        public async Task<EventModel> GetEventById(EventIdRequest eventIdRequest)
        {
            await _validationService.ValidateModel(eventIdRequest);

            return await EnsureEventExists(eventIdRequest.Id);
        }

        public async Task<bool> UpdateEvent(EventModel eventItem)
        {
            await _validationService.ValidateModel(eventItem);

            return await _eventRepository.Update(eventItem);
        }

        public async Task<bool> DeleteEvent(EventIdRequest eventIdRequest)
        {
            await _validationService.ValidateModel(eventIdRequest);

            return await _eventRepository.Delete(eventIdRequest.Id);
        }

        private async Task<EventModel> EnsureEventExists(Guid id)
        {
            var eventItem = await _eventRepository.Get(id);
            if (eventItem == null)
                throw new EntityNotFoundException($"Event with id {id} was not found");

            return eventItem;
        }
    }
}
