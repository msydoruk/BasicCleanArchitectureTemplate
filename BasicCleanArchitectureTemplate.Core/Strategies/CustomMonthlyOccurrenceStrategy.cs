using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Core.Strategies
{
    public class CustomMonthlyOccurrenceStrategy : IEventOccurrenceStrategy
    {
        public IEnumerable<EventOccurrenceModel> GetEventOccurrences(EventModel eventModel,
            EventFilterRequest filterRequest)
        {
            var eventOccurrenceModels = new List<EventOccurrenceModel>();
            var currentDate = eventModel.StartDate;
            var recurrenceSetting = eventModel.RecurrenceSetting;

            while (currentDate <= recurrenceSetting.EndDate)
            {
                var selectedDays = GetSelectedMonthlyDays(currentDate, recurrenceSetting.DayOfWeeks)
                    .Where(date => date >= eventModel.StartDate);

                if (recurrenceSetting.OccurrencePosition == PositionIdPeriod.First)
                {
                    selectedDays = selectedDays.GroupBy(date => date.DayOfWeek).Select(date => date.First());
                }

                if (recurrenceSetting.OccurrencePosition == PositionIdPeriod.Last)
                {
                    selectedDays = selectedDays.GroupBy(date => date.DayOfWeek).Select(date => date.Last());
                }

                eventOccurrenceModels.AddRange(selectedDays.Select(date => new EventOccurrenceModel
                {
                    EventInstance = eventModel,
                    OccurrenceDate = date
                }));

                currentDate = currentDate.AddMonths(recurrenceSetting.Interval);
                currentDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            }

            return eventOccurrenceModels;
        }

        private IEnumerable<DateTime> GetSelectedMonthlyDays(DateTime currentDate, List<DayOfWeek> dayOfWeeks)
        {
            var monthDays = Enumerable.Range(1, DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
                .Select(day => new DateTime(currentDate.Year, currentDate.Month, day));

            var selectedDays =
                monthDays.Where(date => dayOfWeeks.Contains(date.DayOfWeek));

            return selectedDays;
        }
    }
}
