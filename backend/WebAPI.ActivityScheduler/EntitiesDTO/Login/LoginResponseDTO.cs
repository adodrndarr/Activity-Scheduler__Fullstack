using System;


namespace WebAPI.ActivityScheduler.EntitiesDTO.Login
{
    public class LoginResponseDTO
    {
        public LoginResponseDTO(
            bool isLoginSuccessful, 
            bool isAdmin = false,
            string email = null, 
            string id = null, 
            string token = null, 
            DateTime? tokenExpirationDate = null,
            string errorMessage = null)
        {
            this.UserId = id;
            this.IsLoginSuccessful = isLoginSuccessful;
            this.IsAdmin = isAdmin;
            this.Email = email;
            this.Token = token;
            this.TokenExpirationDate = tokenExpirationDate;
            this.ErrorMessage = errorMessage;
        }


        public string UserId { get; set; }
        public bool IsLoginSuccessful { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? TokenExpirationDate { get; set; }
    }
}
