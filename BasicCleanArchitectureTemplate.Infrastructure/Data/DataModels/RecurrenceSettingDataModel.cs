using BasicCleanArchitectureTemplate.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace BasicCleanArchitectureTemplate.Infrastructure.Data.DataModels
{
    public class RecurrenceSettingDataModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public RecurrenceType Type { get; set; }

        public int Interval { get; set; }

        public PositionIdPeriod OccurrencePosition { get; set; }

        public PeriodType PeriodType { get; set; }

        public List<DayOfWeek> DayOfWeeks { get; set; }

        public DateTime EndDate { get; set; }

        public Guid EventId { get; set; }

        public EventDataModel Event { get; set; }
    }
}
