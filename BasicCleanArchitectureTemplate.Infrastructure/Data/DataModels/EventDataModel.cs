using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicCleanArchitectureTemplate.Infrastructure.Data.DataModels
{
    public class EventDataModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Place { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public string? Description { get; set; }

        public string? AdditionalInfo { get; set; }

        public string? ImageUrl { get; set; }

        public RecurrenceSettingDataModel? RecurrenceSetting { get; set; }
    }
}
