using System.Collections.Generic;


namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class RegistrationResponseDTO
    {
        public bool IsRegistrationSuccessful { get; set; }
        public string Info { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
