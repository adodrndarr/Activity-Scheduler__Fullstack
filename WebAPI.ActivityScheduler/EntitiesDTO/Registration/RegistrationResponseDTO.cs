using System.Collections.Generic;


namespace WebAPI.ActivityScheduler.EntitiesDTO.Registration
{
    public class RegistrationResponseDTO
    {
        public RegistrationResponseDTO(
            bool isRegistrationSuccessful, 
            IEnumerable<string> errorMessages = null, 
            string info = null)
        {
            this.IsRegistrationSuccessful = isRegistrationSuccessful;
            this.ErrorMessages = errorMessages;
            this.Info = info;
        }


        public bool IsRegistrationSuccessful { get; set; }
        public string Info { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
