namespace WebAPI.ActivityScheduler.EntitiesDTO.Login
{
    public class LoginResponseDTO
    {
        public LoginResponseDTO(bool isLoginSuccessful, string token = null, string errorMessage = null)
        {
            this.IsLoginSuccessful = isLoginSuccessful;
            this.Token = token;
            this.ErrorMessage = errorMessage;
        }


        public bool IsLoginSuccessful { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }
}
