namespace BasicCleanArchitectureTemplate.Core.Models
{
    public class RecurrenceSettingModel
    {
        public Guid Id { get; set; }

        public RecurrenceType Type { get; set; }

        public int Interval { get; set; }

        public PositionIdPeriod OccurrencePosition { get; set; }

        public PeriodType PeriodType { get; set; }

        public List<DayOfWeek> DayOfWeeks { get; set; }

        public DateTime EndDate { get; set; }
    }
}
