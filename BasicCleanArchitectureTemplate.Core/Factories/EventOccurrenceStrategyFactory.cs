using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;
using BasicCleanArchitectureTemplate.Core.Strategies;

namespace BasicCleanArchitectureTemplate.Core.Factories
{
    public class EventOccurrenceStrategyFactory : IEventOccurrenceStrategyFactory
    {
        private readonly Dictionary<(RecurrenceType?, PeriodType?), Func<IEventOccurrenceStrategy>>
            mapStrategy;

        public EventOccurrenceStrategyFactory()
        {
            mapStrategy =
                new Dictionary<(RecurrenceType? recurrenceType, PeriodType? periodType), Func<IEventOccurrenceStrategy>>
                {
                    { (null, null), () => new SingleOccurrenceStrategy() },
                    { (RecurrenceType.Custom, PeriodType.Week), () => new CustomWeeklyOccurrenceStrategy() },
                    { (RecurrenceType.Custom, PeriodType.Month), () => new CustomMonthlyOccurrenceStrategy() },
                    { (RecurrenceType.Daily, null), () => new StandardRecurringOccurrenceStrategy() },
                    { (RecurrenceType.Weekly, null), () => new StandardRecurringOccurrenceStrategy() },
                    { (RecurrenceType.Monthly, null), () => new StandardRecurringOccurrenceStrategy() },
                    { (RecurrenceType.Yearly, null), () => new StandardRecurringOccurrenceStrategy() },
                };
        }

        public IEventOccurrenceStrategy GetStrategy(RecurrenceSettingModel? recurrenceSetting)
        {
            var key = (recurrenceSetting?.Type, recurrenceSetting?.PeriodType);
            if (mapStrategy.TryGetValue(key, out var strategyFactory))
            {
                return strategyFactory();
            }

            throw new NotImplementedException("Strategy for the given recurrence setting is not implemented.");
        }
    }
}
