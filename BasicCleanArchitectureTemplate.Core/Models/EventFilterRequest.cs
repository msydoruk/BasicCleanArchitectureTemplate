namespace BasicCleanArchitectureTemplate.Core.Models
{
    public class EventFilterRequest
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Category { get; set; }

        public string Place { get; set; }
    }
}

