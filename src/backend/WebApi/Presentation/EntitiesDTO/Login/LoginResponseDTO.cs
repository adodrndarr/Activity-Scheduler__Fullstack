using System;


namespace ActivityScheduler.Presentation.EntitiesDTO
{
    public class LoginResponseDTO
    {
        public string UserId { get; set; }
        public bool IsLoginSuccessful { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? TokenExpirationDate { get; set; }
    }
}
