using System.ComponentModel.DataAnnotations;


namespace WebAPI.ActivityScheduler.EntitiesDTO.Login
{
    public class UserToLoginDTO
    {

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
