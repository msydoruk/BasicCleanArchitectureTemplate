using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Core.Interfaces
{
    public interface IEventOccurrenceStrategy
    {
        IEnumerable<EventOccurrenceModel> GetEventOccurrences(EventModel eventModel, EventFilterRequest filterRequest);
    }
}
