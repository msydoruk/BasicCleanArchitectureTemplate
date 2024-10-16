using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Core.Interfaces
{
    public interface IEventRepository
    {
        Task<EventModel> Add(EventModel eventItem);

        Task<IEnumerable<EventModel>> GetFilteredEvents(EventFilterRequest filterRequest);

        Task<EventModel> Get(Guid id);

        Task<bool> Update(EventModel eventItem);

        Task<bool> Delete(Guid id);
    }
}
