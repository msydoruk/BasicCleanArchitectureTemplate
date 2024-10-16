using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Core.Strategies
{
    public class StandardRecurringOccurrenceStrategy : IEventOccurrenceStrategy
    {
        private static readonly Dictionary<RecurrenceType, Func<DateTime, int, DateTime>>
            NextStandardOccurrenceCalculators =
                new()
                {
                    { RecurrenceType.Daily, (currentDate, interval) => currentDate.AddDays(interval) },
                    { RecurrenceType.Weekly, (currentDate, interval) => currentDate.AddDays(7 * interval) },
                    { RecurrenceType.Monthly, (currentDate, interval) => currentDate.AddMonths(interval) },
                    { RecurrenceType.Yearly, (currentDate, interval) => currentDate.AddYears(interval) }
                };

        public IEnumerable<EventOccurrenceModel> GetEventOccurrences(EventModel eventModel,
            EventFilterRequest filterRequest)
        {
            var currentDate = eventModel.StartDate;
            var recurrenceSetting = eventModel.RecurrenceSetting;

            while (currentDate <= recurrenceSetting.EndDate)
            {
                if (currentDate >= filterRequest.StartDate)
                {
                    yield return new EventOccurrenceModel
                    {
                        EventInstance = eventModel,
                        OccurrenceDate = currentDate
                    };
                }

                currentDate = CalculateNextStandardOccurrence(currentDate, recurrenceSetting);
            }
        }

        private DateTime CalculateNextStandardOccurrence(DateTime currentDate, RecurrenceSettingModel recurrenceSetting)
        {
            if (NextStandardOccurrenceCalculators.TryGetValue(recurrenceSetting.Type, out var action))
            {
                currentDate = action(currentDate, recurrenceSetting.Interval);
            }

            return currentDate;
        }
    }
}
