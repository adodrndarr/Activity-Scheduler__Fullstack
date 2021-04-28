using System.Collections.Generic;


namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class ScheduleResponseDTO
    {
        public string Info { get; set; }
        public List<string> UnsuccessfulInfo { get; set; }
    }
}
