using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Core.Strategies
{
    public class CustomWeeklyOccurrenceStrategy : IEventOccurrenceStrategy
    {
        public IEnumerable<EventOccurrenceModel> GetEventOccurrences(EventModel eventModel,
            EventFilterRequest filterRequest)
        {
            var currentDate = eventModel.StartDate;
            var recurrenceSetting = eventModel.RecurrenceSetting;

            while (currentDate <= recurrenceSetting.EndDate)
            {
                if (currentDate >= filterRequest.StartDate &&
                    recurrenceSetting.DayOfWeeks.Contains(currentDate.DayOfWeek) &&
                    IsCorrectCustomWeekInterval(currentDate, eventModel))
                {
                    yield return new EventOccurrenceModel
                    {
                        EventInstance = eventModel,
                        OccurrenceDate = currentDate
                    };
                }

                currentDate = currentDate.AddDays(1);
            }
        }

        bool IsCorrectCustomWeekInterval(DateTime currentDate, EventModel eventModel)
        {
            int weeksDifferent = (currentDate - eventModel.StartDate).Days / 7;

            return weeksDifferent % eventModel.RecurrenceSetting.Interval == 0;
        }
    }
}
