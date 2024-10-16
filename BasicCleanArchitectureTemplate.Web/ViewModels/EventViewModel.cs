using System.ComponentModel.DataAnnotations;

namespace BasicCleanArchitectureTemplate.Web.ViewModels
{
    public class EventViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Place { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        public string? Description { get; set; }

        public string? AdditionalInfo { get; set; }

        public string? ImageUrl { get; set; }

        public bool HasRecurrence { get; set; }

        public RecurrenceSettingViewModel? RecurrenceSetting { get; set; }
    }
}
