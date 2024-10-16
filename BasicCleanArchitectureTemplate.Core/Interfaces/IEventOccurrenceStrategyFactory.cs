using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Core.Interfaces
{
    public interface IEventOccurrenceStrategyFactory
    {
        IEventOccurrenceStrategy GetStrategy(RecurrenceSettingModel? recurrenceSetting);
    }
}

