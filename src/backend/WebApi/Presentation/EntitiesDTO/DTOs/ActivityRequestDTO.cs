using System;


namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class ActivityRequestDTO
    {
        public DateTime BookedForDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string ActivityEntityName { get; set; }
        public string ActivityEntityId { get; set; }
        public string OrganizerName { get; set; }
    }
}
