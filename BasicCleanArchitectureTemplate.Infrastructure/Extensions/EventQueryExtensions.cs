using BasicCleanArchitectureTemplate.Infrastructure.Data.DataModels;

namespace BasicCleanArchitectureTemplate.Infrastructure.Extensions
{
    public static class EventFilters
    {
        public static IQueryable<EventDataModel> FilterByDataConsideringRecurrence(
            this IQueryable<EventDataModel> query,
            DateTime startDate,
            DateTime endDate)
        {
            return query.Where(@event =>
                (@event.RecurrenceSetting == null &&
                 @event.StartDate >= startDate &&
                 @event.StartDate <= endDate)
                ||
                (@event.RecurrenceSetting != null &&
                 @event.StartDate <= endDate &&
                 @event.RecurrenceSetting.EndDate >= startDate)
            );
        }

        public static IQueryable<EventDataModel> FilterByCategory(this IQueryable<EventDataModel> query,string category)
        {
            return string.IsNullOrWhiteSpace(category)
                ? query
                : query.Where(@event => @event.Category.Contains(category));
        }

        public static IQueryable<EventDataModel> FilterByPlace(this IQueryable<EventDataModel> query, string place)
        {
            return string.IsNullOrWhiteSpace(place)
                ? query
                : query.Where(@event => @event.Place.Contains(place));
        }
    }
}

