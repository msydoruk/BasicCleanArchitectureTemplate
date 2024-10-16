using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BasicCleanArchitectureTemplate.Core.Models;

namespace BasicCleanArchitectureTemplate.Web.ViewModels
{
    public class RecurrenceSettingViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public RecurrenceType Type { get; set; }

        [Required]
        [DefaultValue(1)]
        public int Interval { get; set; }

        public PositionIdPeriod? OccurrencePosition { get; set; }

        public PeriodType? PeriodType { get; set; }

        public List<DayOfWeek>? DayOfWeeks { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
    }
}

