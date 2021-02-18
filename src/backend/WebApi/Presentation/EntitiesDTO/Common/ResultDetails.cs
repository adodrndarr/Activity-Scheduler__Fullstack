using System.Collections.Generic;


namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class ResultDetails
    {
        public bool IsSuccessful { get; set; }
        public int StatusCode { get; set; }
        public string Info { get; set; }
        public IEnumerable<string> Infos { get; set; }
        public dynamic Payload { get; set; }
    }
}
