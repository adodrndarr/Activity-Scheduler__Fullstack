using System.ComponentModel.DataAnnotations;


namespace WebAPI.ActivityScheduler.EntitiesDTO.Registration
{
    public class UserToRegisterDTO
    {
        public string UserName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; }
    }
}
