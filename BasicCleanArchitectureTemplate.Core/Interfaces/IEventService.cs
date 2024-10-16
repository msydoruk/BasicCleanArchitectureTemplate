using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Core.Interfaces
{
    public interface IEventService
    {
        Task<EventModel> AddEvent(EventModel eventItem);

        Task<IEnumerable<EventOccurrenceModel>> GetEventOccurrences(EventFilterRequest filterRequest);

        Task<EventModel> GetEventById(EventIdRequest eventIdRequest);

        Task<bool> UpdateEvent(EventModel eventItem);

        Task<bool> DeleteEvent(EventIdRequest eventIdRequest);
    }
}
