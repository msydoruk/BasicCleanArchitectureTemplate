using AutoMapper;
using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;
using BasicCleanArchitectureTemplate.Infrastructure.Data;
using BasicCleanArchitectureTemplate.Infrastructure.Data.DataModels;
using BasicCleanArchitectureTemplate.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BasicCleanArchitectureTemplate.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EventRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventModel> Add(EventModel eventItem)
        {
            var eventEntity = _mapper.Map<EventModel, EventDataModel>(eventItem);
            var result = await _context.Events.AddAsync(eventEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<EventDataModel, EventModel>(result.Entity);
        }

        public async Task<IEnumerable<EventModel>> GetFilteredEvents(EventFilterRequest filterRequest)
        {
            IQueryable<EventDataModel> query = _context.Events.Include(setting => setting.RecurrenceSetting);

            query = query.FilterByDataConsideringRecurrence(filterRequest.StartDate, filterRequest.EndDate)
                .FilterByCategory(filterRequest.Category)
                .FilterByPlace(filterRequest.Place);

            var eventEntities = await query.AsNoTracking().ToListAsync();

            return _mapper.Map<IEnumerable<EventDataModel>, IEnumerable<EventModel>>(eventEntities);
        }

        public async Task<EventModel> Get(Guid id)
        {
            var eventEntity = await _context.Events
                .Include(setting => setting.RecurrenceSetting)
                .FirstOrDefaultAsync(@event => @event.Id == id);

            return _mapper.Map<EventDataModel, EventModel>(eventEntity);
        }

        public async Task<bool> Update(EventModel eventItem)
        {
            var existingEventEntity = await _context.Events.FindAsync(eventItem.Id);

            if (existingEventEntity == null)
                return false;

            _mapper.Map(eventItem, existingEventEntity);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
                return false;

            _context.Events.Remove(eventItem);
            var changes = await _context.SaveChangesAsync();

            return changes > 0;
        }
    }
}
