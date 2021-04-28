using System;


namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class BookedActivityDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsValid { get; set; }
        public string Info { get; set; }
    }
}
