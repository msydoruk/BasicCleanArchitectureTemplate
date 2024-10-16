using BasicCleanArchitectureTemplate.Core.Models;
using BasicCleanArchitectureTemplate.Web.ViewModels;
using System.Diagnostics;

namespace BasicCleanArchitectureTemplate.Tests.Factories
{
    public static class EventModelFactory
    {
        public static EventModel CreateTestEventModel(
            Guid id,
            string name = "Test Event",
            DateTime? startDate = null,
            DateTime? recurrenceEndDate = null,
            RecurrenceType? recurrenceType = null,
            int? recurrenceInterval = null)
        {
            var eventModel = new EventModel
            {
                Id = id,
                Name = name,
                Category = "Test Category",
                Place = "Test Place",
                StartDate = startDate ?? DateTime.UtcNow,
                Description = "Test Description",
                AdditionalInfo = "Test Additional Info",
                ImageUrl = "http://example.com/test.jpg"
            };

            if (recurrenceEndDate.HasValue || recurrenceType.HasValue || recurrenceInterval.HasValue)
            {
                eventModel.RecurrenceSetting = new RecurrenceSettingModel
                {
                    Type = recurrenceType ?? RecurrenceType.Daily,
                    Interval = recurrenceInterval ?? 1,
                    EndDate = recurrenceEndDate.Value
                };
            }

            return eventModel;
        }

        public static EventViewModel CreateTestEventViewModel(Guid id, string name = "Test Event")
        {
            return new EventViewModel
            {
                Id = id,
                Name = name,
                Category = "Test Category",
                Place = "Test Place",
                StartDate = DateTime.UtcNow,
                Description = "Test Description",
                AdditionalInfo = "Test Additional Info",
                ImageUrl = "http://example.com/test.jpg"
            };
        }

        public static List<EventModel> CreateTestEventModelList(int count = 2, DateTime? startDate = null, RecurrenceSettingModel recurrenceSetting = null)
        {
            var list = new List<EventModel>();

            for (int i = 0; i < count; i++)
            {
                var eventModel = CreateTestEventModel(Guid.NewGuid(), $"Test Event {i + 1}");
                eventModel.StartDate = startDate ?? DateTime.UtcNow.AddDays(i);
                eventModel.RecurrenceSetting = recurrenceSetting;
                list.Add(eventModel);
            }

            return list;
        }

        public static List<EventOccurrenceModel> CreateTestEventOccurrenceModelList(int count = 2)
        {
            var list = new List<EventOccurrenceModel>();

            for (int i = 0; i < count; i++)
            {
                var eventInstance = new EventModel()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Test Event {i}",
                    Category = "Test Category",
                    Description = "Test Description",
                    Place = "Test Place",
                    AdditionalInfo = "Test Additional Info",
                    ImageUrl = "Test Image URL"
                };

                var occurrence = new EventOccurrenceModel
                {
                    EventInstance = eventInstance,
                    OccurrenceDate = DateTime.UtcNow.AddDays(i)
                };

                list.Add(occurrence);
            }

            return list;
        }
    }
}
