using System.Collections.Generic;


namespace WebAPI.ActivityScheduler.EntitiesDTO.Registration
{
    public class RegistrationResponseDTO
    {
        public RegistrationResponseDTO(bool isRegistrationSuccessful, IEnumerable<string> errorMessages )
        {
            this.IsRegistrationSuccessful = isRegistrationSuccessful;
            this.ErrorMessages = errorMessages;
        }


        public bool IsRegistrationSuccessful { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
