using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Core.Strategies
{
    public class SingleOccurrenceStrategy : IEventOccurrenceStrategy
    {
        public IEnumerable<EventOccurrenceModel> GetEventOccurrences(EventModel eventModel,
            EventFilterRequest filterRequest)
        {
            yield return new EventOccurrenceModel
            {
                EventInstance = eventModel,
                OccurrenceDate = eventModel.StartDate
            };
        }
    }
}
