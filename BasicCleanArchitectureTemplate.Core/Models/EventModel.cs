namespace BasicCleanArchitectureTemplate.Core.Models
{
    public class EventModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Place { get; set; }

        public DateTime StartDate { get; set; }

        public string? Description { get; set; }

        public string? AdditionalInfo { get; set; }

        public string? ImageUrl { get; set; }

        public RecurrenceSettingModel? RecurrenceSetting { get; set; }
    }
}
