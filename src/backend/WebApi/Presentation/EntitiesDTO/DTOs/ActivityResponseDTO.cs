using System;


namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class ActivityResponseDTO
    {
        public Guid Id { get; set; }

        public DateTime DateBooked { get; set; }
        public DateTime BookedForDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string ActivityEntityId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public int ItemQuantity { get; set; }
        public int MinUserCount { get; set; }
        public int MaxUserCount { get; set; }

        public string Description { get; set; }
        public string Location { get; set; }

        public string UserId { get; set; }

        public string OrganizerName { get; set; }
        public string Duration { get; set; }
    }
}
